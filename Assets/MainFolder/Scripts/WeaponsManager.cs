using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class WeaponsManager : MonoBehaviour
{
    public WeaponsList weaponsData;
    public List<WeaponTypeParent> _availWeapons = new List<WeaponTypeParent>();
    public List<WeaponTypeParent> _copiesWeapons = new List<WeaponTypeParent>();
    public List<WeaponTypeParent> _boughtWeapons = new List<WeaponTypeParent>();

    public List<GameObject> _boughtActives = new List<GameObject>();
    public List<GameObject> _boughtPassives = new List<GameObject>();
    private int _actIndex;
    private int _pasIndex;


    [SerializeField]
    private GameObject chooseScreen;

    public Button but1;
    public Button but2;
    public Button but3;

    private Dictionary<Button, WeaponTypeParent> _buttonWeaponList = new Dictionary<Button, WeaponTypeParent>();

    public static WeaponsManager Instance;

    public List<float> levelUpsToShow;

    [SerializeField] private Button _rerollGems;
    [SerializeField] private Button _rerollVideo;

    public MatchManager _matchManager;
    [SerializeField] private UIManager _uiManager;

    private InstGemsAdd _instGemsAdd;

    private void Start()
    {
        Instance = this;
        _availWeapons = weaponsData.availableWeapons;

        ConstraintSource cs = new ConstraintSource();
        cs.sourceTransform = PlayerMoveController.Instance.transform;
        cs.weight = 1;

        foreach (var weapon in _availWeapons)
        {
            var wp = Instantiate(weapon, transform);
            var pc = wp.GetComponent<ParentConstraint>();
            pc.AddSource(cs);
            _copiesWeapons.Add(wp);

            if (weapon == PlayerMoveController.Instance._baseWeapon)
            {
                PlayerMoveController.Instance._baseWeapon = wp;
                wp.SetAsBase();
            }
        }

        but1.onClick.AddListener(() => ChooseButton(but1));
        but2.onClick.AddListener(() => ChooseButton(but2));
        but3.onClick.AddListener(() => ChooseButton(but3));

        _rerollGems.onClick.AddListener(() => RerollGems());

        _instGemsAdd = GetComponent<InstGemsAdd>();
    }

    public void CreateScreenArray(float curLevel)
    {
        StartCoroutine(ICreateScreenArray(curLevel));
    }

    public IEnumerator ICreateScreenArray(float curLevel)
    {
        yield return new WaitForSeconds(1);
        levelUpsToShow.Add(curLevel);

        if (levelUpsToShow.Count == 1)
        {
            CreateScreen();
        }
    }

    public void CreateScreen()
    {
        _buttonWeaponList.Clear();
        _actIndex = 0;
        _pasIndex = 0;

        foreach (var a in _boughtActives)
        {
            a.GetComponent<BoughtCell>().Reset();
        }

        foreach (var a in _boughtPassives)
        {
            a.GetComponent<BoughtCell>().Reset();
        }

        for (var i = 0; i < _boughtWeapons.Count; i++)
        {
            if (_boughtWeapons[i].active)
            {
                try
                {

                    var cell = _boughtActives[_actIndex].GetComponent<BoughtCell>();
                    cell.image.sprite = _boughtWeapons[i].curInfo.Image;
                    cell.ShowLevel(_boughtWeapons[i].curInfo.Level);
                    _actIndex++;
                }
                catch
                {
                    Debug.Log("Error, too many weapons for cells;");
                    break;
                }

            }
            else
            {
                try
                {
                    var cell = _boughtPassives[_pasIndex].GetComponent<BoughtCell>();
                    cell.image.sprite = _boughtWeapons[i].curInfo.Image;
                    cell.ShowLevel(_boughtWeapons[i].curInfo.Level);
                    _pasIndex++;
                }
                catch
                {
                    Debug.Log("Error, too many weapons for cells;");
                    break;
                }
            }
        }


        CreateFirstButton(but1);
        CreateButton(but2);
        CreateButton(but3);

        chooseScreen.SetActive(true);

        Time.timeScale = 0;

        if (_matchManager.needTutor && PlayerMoveController.Instance._level == 2)
        {
            _uiManager.ShowTutor(_uiManager._tutorLevelUpScreen);
        }
    }

    private void RerollGems()
    {
        if (_instGemsAdd.SpendGems(10))
        {
            Reroll();
        }
    }

    private void Reroll()
    {
        _buttonWeaponList.Clear();

        CreateFirstButton(but1);
        CreateButton(but2);
        CreateButton(but3);
    }

    private void CreateButton(Button button)
    {
        var chosenWeapon = _copiesWeapons[Random.Range(0, _copiesWeapons.Count)];
        if (_buttonWeaponList.ContainsValue(chosenWeapon))
        {
            CreateButton(button);
        }
        else
        {
            button.GetComponent<WeaponPanel>().UpdateCard(chosenWeapon.ReturnInfo());
            _buttonWeaponList.Add(button, chosenWeapon);
        }
    }

    private void CreateFirstButton(Button button)
    {
        var chosenWeapon = _copiesWeapons[Random.Range(0, _copiesWeapons.Count)];
        if (!chosenWeapon.active)
        {
            int countActives = 0;
            foreach (var wp in _copiesWeapons)
            {
                if (wp.active)
                {
                    countActives++;
                }
            }
            if (countActives > 0)
            {
                CreateFirstButton(button);
            }
            else
            {
                CreateButton(but1);
            }
        }
        else
        {
            button.GetComponent<WeaponPanel>().UpdateCard(chosenWeapon.ReturnInfo());
            _buttonWeaponList.Add(button, chosenWeapon);
        }
    }

    private void ChooseButton(Button button)
    {
        Time.timeScale = 1;
        _buttonWeaponList[button].Upgrade();
        levelUpsToShow.Remove(levelUpsToShow[0]);

        if (levelUpsToShow.Count == 0)
        {
            chooseScreen.SetActive(false);

        }
        else
        {
            CreateScreen();
        }
    }

    public void UnlockWeapon(WeaponTypeParent weapon)
    {
        weaponsData.availableWeapons.Add(weapon);
    }
}

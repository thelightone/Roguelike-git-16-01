using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTypeParent : MonoBehaviour
{
    public bool active;

    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;

    public int _level = 0;

    public WeaponInfo curInfo;

    public List<WeaponInfo> weaponsLevels = new List<WeaponInfo>();

    public bool isBaseWeapon = false;

    private void Awake()
    {
        _level = 0;
        isBaseWeapon = false;

        for (var i = 0; i<3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetAsBase()
    {
        isBaseWeapon = true;
        Upgrade();
    }

    public void Upgrade()
    {
        if (_level < 3)
        {

            switch (_level)
            {
                case 0:
                    level1.SetActive(true);
                    WeaponsManager.Instance._boughtWeapons.Add(this);
                    break;
                case 1:
                    level1.SetActive(false);
                    level2.SetActive(true);
                    break;
                case 2:
                    Debug.Log("3");
                    level2.SetActive(false);
                    level3.SetActive(true);
                    WeaponsManager.Instance._copiesWeapons.Remove(this);
                    if (WeaponsManager.Instance._copiesWeapons.Count<3)
                    {
                        PlayerMoveController.Instance.MaxLevel();
                    }

                    break;
                default:
                    break;
            }
            curInfo = weaponsLevels[_level];
            _level++;
        }
    }

    public WeaponInfo ReturnInfo()
    {
        weaponsLevels[_level].Level = _level+1;
        return weaponsLevels[_level];
    }
}

[System.Serializable]
public class WeaponInfo
{
    public string Name;
    public string Descr;
    public Sprite Image;
    [HideInInspector]
    public int Level;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _coins = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> _gems = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> _energys = new List<TMP_Text>();

    [SerializeField] private List<Button> _toMain = new List<Button>();
    [SerializeField] private List<Button> _toShop = new List<Button>();
    [SerializeField] private List<Button> _toUpgrades = new List<Button>();
    [SerializeField] private List<Button> _toGame = new List<Button>();
    [SerializeField] private List<ParticleSystem> _overlayEffects = new List<ParticleSystem>();

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _noEnergyPanel;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _upgradesPanel;

    [SerializeField] private GameObject _tutorToBattle;
    [SerializeField] private GameObject _tutor2ToUpgrade;
    [SerializeField] private GameObject _tutor3BuyUpgrade;

    [SerializeField] private InstGemsAdd _instGemsAdd;
    [SerializeField] private Button _addEnergyGems; 
    [SerializeField] private Button _addEnergyGemsSep;

    [SerializeField] private Button _prevLevel;
    [SerializeField] private Button _nextLevel;

    [SerializeField] private Slider battleLoadingSlider;

    [SerializeField] private List<Button> _chosenLevels = new List<Button>();

    [SerializeField] private TMP_Text _lvlText;
    [SerializeField] private Slider _xpSlider;

    public Transform _musicCheck;
    public AudioManager _audioManager; 
  
    private EnergyManager _energyManager;
    private LevelsManager _levelsManager;

    public GameObject startBut2;
    public GameObject lock2;

    public GameObject reward;


    private void Awake()
    {

        JSONSaver.CheckFile();

    }

    private void Start()
    {

  
        UpdateBalances();

        foreach (var but in _toMain)
        {
            but.onClick.AddListener(() => OpenMain());
        }


        foreach (var but in _toShop)
        {
            but.onClick.AddListener(() => OpenShop());
        }

        foreach (var but in _toUpgrades)
        {
            but.onClick.AddListener(() => OpenUpgrades());
        }

        //foreach (var but in _toGame)
        //{
        //    but.onClick.AddListener(() => StartGame(but.));
        //}

        //foreach (var but in _chosenLevels)
        //{
        //    but.onClick.AddListener(() => ChooseLevel(but));
        //}

        _energyManager = GetComponent<EnergyManager>();
        _levelsManager = GetComponent<LevelsManager>();
        _addEnergyGems.onClick.AddListener(() => AddEnergyGems());
        _addEnergyGemsSep.onClick.AddListener(() => OpenEnergyPanel());

        if (PrefsManager.GetFirstPlay() == 0)
        {
            OpenMain();
            Debug.Log("zero");
        }
        else if (PrefsManager.GetFirstPlay() == 1)
        {
            PrefsManager.ChangeFirstPlay(2);
            StartGame(0);
            Debug.Log("one");
            reward.SetActive(false);

        }
        else if (PrefsManager.GetFirstPlay() == 2)
        {
            PrefsManager.ChangeFirstPlay(0);
            OpenMain();
            UpdateBalances();
            _tutor2ToUpgrade.SetActive(true);
            Debug.Log("two");
            reward.SetActive(false);

        }

        var msc = PrefsManager.GetSound();
        if (msc == 1)
        {
            _musicCheck.GetChild(1).gameObject.SetActive(true);
            _musicCheck.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            _musicCheck.GetChild(0).gameObject.SetActive(true);
            _musicCheck.GetChild(1).gameObject.SetActive(false);

        }
        ////_prevLevel.onClick.AddListener(() => ChangeLevel(-1));
        ////_nextLevel.onClick.AddListener(() => ChangeLevel(1));

        var openLvl2 = PlayerPrefs.GetInt("charter1UnlockedLevel", 0);

        if (openLvl2 >= 4)
        {
            startBut2.SetActive(false);
            lock2.SetActive(false);
        }
        else
        {
            startBut2.SetActive(true);
            lock2.SetActive(true);
        }
    }

    private void ChangeLevel(int change)
    {
        if (PrefsManager.GetChosenLevelCharter1()+change >=0 && PrefsManager.GetChosenLevelCharter1() + change <= (PrefsManager.GetUnlockedLevelCharter1()-1))
        {
            PrefsManager.ChangeChosenLevel1(PrefsManager.GetChosenLevelCharter1() + change);
        }

        _levelsManager.ChooseLevel();
    }

    private void SetLevel(int newIndex)
    {
        if (newIndex >= 0 && newIndex <= (PrefsManager.GetUnlockedLevelCharter1()))
        {
            PrefsManager.ChangeChosenLevel1(newIndex);
        }

        _levelsManager.ChooseLevel();
    }

    public void ChooseLevel(Button but)
    {
        int index = 0;

        foreach (var button in _chosenLevels)
        {
            if (button == but)
            {
                break;
            }
            index++;
        }
        SetLevel(index);
    }

    private void AddEnergyGems()
    {
       if (_instGemsAdd.SpendGems(10))
        {
            PrefsManager.ChangeEnergy(200);
            UpdateBalances();
            OpenBasePanels(_mainPanel);
        }
    }

    public void UpdateBalances()
    {
        foreach (var coin in _coins)
        {
            coin.text = PrefsManager.GetCoins().ToString();
        }

        foreach (var gem in _gems)
        {
            gem.text = PrefsManager.GetGems().ToString();
        }

        for (var i = 0; i < _energys.Count; i++)
        {
            _energys[i].text = PrefsManager.GetEnergy().ToString() + "/50";
        }

        _lvlText.text = PrefsManager.GetPlayerLevel().ToString();
        _xpSlider.maxValue = PrefsManager.GetPlayerLevel() * 100;
        _xpSlider.value = PrefsManager.GetPlayerXP();
    }

    private void OpenBasePanels(GameObject panel)
    {
        _mainPanel.SetActive(false);
        _levelsPanel.SetActive(false);
        _shopPanel.SetActive(false);
        _noEnergyPanel.SetActive(false);
        _loadingPanel.SetActive(false);
        _upgradesPanel.SetActive(false);
        _tutorToBattle.SetActive(false);

        panel.SetActive(true);

        if (!_overlayEffects[0].gameObject.activeInHierarchy)
        {
            foreach (var effect in _overlayEffects)
            {
                effect.gameObject.SetActive(true);
            }
        }
        UpdateBalances();
    }

    public void OpenMain()
    {
        OpenBasePanels(_mainPanel);
    }
    public void OpenLevels()
    {
        OpenBasePanels(_levelsPanel);
    }
    public void OpenUpgrades()
    {
        OpenBasePanels(_upgradesPanel);
    }
    public void OpenShop()
    {
        OpenBasePanels(_shopPanel);
    }

    public void StartGame(int id)
    {
        if (PrefsManager.GetEnergy() >= 5)
        {
            OpenBasePanels(_loadingPanel);
            _energyManager.SpendEnergy();
            UpdateBalances();
            StartCoroutine(StartGameCor(id));
        }
        else
        {
            OpenEnergyPanel();
        }
    }

    private void OpenEnergyPanel()
    {
        _noEnergyPanel.SetActive(true);
        foreach (var effect in _overlayEffects)
        {
            effect.gameObject.SetActive(false);
        }
    }

    private IEnumerator StartGameCor(int id)
    {
        yield return new WaitForSeconds(0.1f);
        GameSceneManager.Instance.Game(id);
    }

    public void ChangeMusic()
    {
        int soundOn = PrefsManager.GetSound();


        if (soundOn == 0)
        {
            _audioManager.audioSource1.volume = 1;
            _audioManager.audioSource2.volume = 1;
            _musicCheck.GetChild(0).gameObject.SetActive(false);
            _musicCheck.GetChild(1).gameObject.SetActive(true);
            PrefsManager.ChangeSound(1);

        }
        else
        {
            _audioManager.audioSource1.volume = 0;
            _audioManager.audioSource2.volume = 0;
            _musicCheck.GetChild(1).gameObject.SetActive(false);
            _musicCheck.GetChild(0).gameObject.SetActive(true);
            PrefsManager.ChangeSound(0);
        }
    }

    public void GemsToGold(int gems)
    {
        var balance = PlayerPrefs.GetInt("gems", 0);
        if (balance>=gems)
        {
            PlayerPrefs.SetInt("gems", balance-gems);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins")+ gems * 100);
            UpdateBalances();
        }
    }

    public void ShowReward()
    {
        reward.SetActive(true);
    }

    public void DeleteSavings()
    {
        PlayerPrefs.DeleteAll();

        var filePath = Path.Combine(Application.persistentDataPath, "data.json");

        // Проверяем, существует ли файл перед удалением
        if (File.Exists(filePath))
        {
            try
            {
                // Удаляем файл
                File.Delete(filePath);
                Console.WriteLine("Файл успешно удален.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Файл не существует.");
        }
        PrefsManager.ChangeEnergy(50);
        UpdateBalances();
    }
    

}

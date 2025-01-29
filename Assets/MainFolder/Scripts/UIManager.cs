using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PUROPORO;
using UnityEngine.UI;
using System;
using System.Xml;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _enemiesRemain;
    [SerializeField] private GameObject _gameBlock;
    [SerializeField] private GameObject _chestBlock;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _killThemText;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _bossComeScreen;
    [SerializeField] private GameObject _updateScreen;
    [SerializeField] private GameObject _finLevelScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _unlockWeaponScreen;
    [SerializeField] private TMP_Text _startText;

    public GameObject _tutorGreetScreen;
    public GameObject _tutorMoveScreen;
    public GameObject _tutorKillScreen;
    public GameObject _tutorKillScreen2;
    public GameObject _tutorSkillScreen;
    public GameObject _tutorLevelUpScreen;

    [SerializeField] private Button _disactTutorMove;
    [SerializeField] private Button _disactTutorKill;
    [SerializeField] private Button _disactTutorLevel;

    [SerializeField] private GameObject _nextWaveObj;
    [SerializeField] private GameObject _finWaveObj;

    [SerializeField] private Button _healGems;
    [SerializeField] private Button _healVideo;

    [SerializeField] private GameObject _notHealButs;
    [SerializeField] private GameObject _healButs;

    [SerializeField] private Button _reviveGems;
    [SerializeField] private Button _reviveVideo;

    [SerializeField] private List<TMP_Text> _coins = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> _gems = new List<TMP_Text>();

    [SerializeField] private SOCard _coinsCard;
    [SerializeField] private SOCard _gemsCard;

    [SerializeField] private Button _backToMenuBut;
    [SerializeField] private Button _backToMenuBut2;
    [SerializeField] private Button _backToMenuButUnlockWeapon;

    [SerializeField] private Button _backToMenuButPauseMenu;
    // [SerializeField] private Button _playPauseMenu;
    [SerializeField] private Button _pauseBut;

    [SerializeField] private Button _nextWaveButton;
    [SerializeField] private Button _finLevelButton;

    [SerializeField] private Slider _healthSlider;

    [SerializeField] private List<GameObject> _waveTogglesParent = new List<GameObject>();
    [SerializeField] private List<Slider> _waveSliders = new List<Slider>();
    [SerializeField] private List<TMP_Text> _killTexts = new List<TMP_Text>();

    [SerializeField] private TMP_Text _finTime;
    [SerializeField] private TMP_Text _charter;

    [SerializeField] private GameObject _loseGems;
    [SerializeField] private GameObject _winGems;

    [SerializeField] private Image _unlockWeaponImage;
    [SerializeField] private TMP_Text _unlockWeaponName;
    [SerializeField] private TMP_Text _unlockWeaponDescr;

    private MatchManager _matchManager;
    private InstGemsAdd _instGemsAdd;
    private AudioManager _audioManager;

    [SerializeField] private Transform _musicCheck;

    public GameObject buyGems;
    public GameObject unlockCharter;

    public GameObject skillBut;

    private void Start()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
        var menuAudio = PlayerPrefs.GetInt("audio", 0);
        InitAudio();

        _matchManager = GetComponent<MatchManager>();
        PlayerMoveController.balance.AddListener(() => UpdateCoinsGems());
        EnemySpawner.kill.AddListener(() => UpdateKills());
        foreach (var coin in _coins)
        {
            coin.text = "0";
        }
        foreach (var gem in _gems)
        {
            gem.text = "0";
        }

        _nextWaveButton.onClick.AddListener(() => _matchManager.NewWave());

        _finLevelButton.onClick.AddListener(() => FinLevel());

        _backToMenuBut.onClick.AddListener(() => BackToMenu());
        _backToMenuBut2.onClick.AddListener(() => BackToMenu());
        _backToMenuButUnlockWeapon.onClick.AddListener(() => BackToMenu());

        _pauseBut.onClick.AddListener(() => MakePause());
        // _playPauseMenu.onClick.AddListener(() => UnmakePause());

        _backToMenuButPauseMenu.onClick.AddListener(() => { Time.timeScale = 1; GameSceneManager.Instance.Menu(); });

        _healGems.onClick.AddListener(() => HealGems());
        _reviveGems.onClick.AddListener(() => ReviveGems());

      //  _disactTutorMove.onClick.AddListener(() => DisactTutor(_tutorMoveScreen));
        _disactTutorKill.onClick.AddListener(() => DisactTutor(_tutorKillScreen));
        _disactTutorLevel.onClick.AddListener(() => DisactTutor(_tutorLevelUpScreen));


        _instGemsAdd = GetComponent<InstGemsAdd>();

        GameObject wavesScreen = null;

        foreach (var parent in _waveTogglesParent)
        {
            switch (_matchManager._levelData.waveData.Count)
            {
                case 1:
                    wavesScreen = parent.transform.GetChild(0).gameObject;
                    break;
                case 3:
                    wavesScreen = parent.transform.GetChild(1).gameObject;
                    break;
                case 5:
                    wavesScreen = parent.transform.GetChild(2).gameObject;
                    break;
                case 7:
                    wavesScreen = parent.transform.GetChild(3).gameObject;
                    break;
            }
            wavesScreen.SetActive(true);
        }
    }

    private void InitAudio()
    {

        var audio = PrefsManager.GetSound();
        if (audio == 1)
        {
            _audioManager.audioSource1.volume = 1;
            _audioManager.audioSource2.volume = 1;
            _musicCheck.GetChild(0).gameObject.SetActive(false);
            _musicCheck.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            _audioManager.audioSource1.volume = 0;
            _audioManager.audioSource2.volume = 0;
            _musicCheck.GetChild(1).gameObject.SetActive(false);
            _musicCheck.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ShowTutor(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void DisactTutor(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = panel == _tutorLevelUpScreen ? 0 : 1;
    }

    private void HealGems()
    {
        if ((int)PlayerMoveController.Instance._curHealth != (int)PlayerMoveController.Instance._maxHealth && _healGems.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (_instGemsAdd.SpendGems(10))
            {
                PlayerMoveController.Instance._curHealth = PlayerMoveController.Instance._maxHealth;
                PlayerMoveController.healthChange.Invoke();
                _notHealButs.gameObject.SetActive(false);
                _healButs.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateSliders(int maxWaves, int waves, float progress)
    {
        float value = waves * 100;
        value = value + (100 * progress);

        foreach (var sl in _waveSliders)
        {
            sl.maxValue = maxWaves * 100;
            sl.value = value;
        }
    }

    private void ReviveGems()
    {
        if (_instGemsAdd.SpendGems(10))
        {
            Time.timeScale = 1;
            _gameBlock.SetActive(true);
            _matchManager.Revive();
            _loseScreen.SetActive(false);
        }
    }

    public void MakePause()
    {
        Time.timeScale = 0;
        _pauseScreen.gameObject.SetActive(true);
    }

    public void UnmakePause()
    {
        Time.timeScale = 1;
        _pauseScreen.gameObject.SetActive(false);
    }

    public void NewWave()
    {
        _gameBlock.SetActive(true);
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
        _chestBlock.SetActive(false);
        _finWaveObj.SetActive(false);

        _timeText.transform.parent.gameObject.SetActive(true);
        _enemiesRemain.transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public void ShowTime(string time)
    {
        _timeText.text = time;
    }

    public void ShowEnemies()
    {
        _enemiesRemain.text = EnemySpawner.Instance.curEnemy.ToString();
    }

    public void ShowWin()
    {
        if (PlayerMoveController.Instance.gems == 0)
        {
            _loseGems.SetActive(false);
            _winGems.SetActive(false);
        }
        else
        {

            _loseGems.SetActive(true);
            _winGems.SetActive(true);
        }

        _notHealButs.gameObject.SetActive(true);
        _healButs.gameObject.SetActive(false);
        StartCoroutine(WinCor());
    }

    public void KillThemPart()
    {
        _timeText.transform.parent.gameObject.SetActive(false);
        _enemiesRemain.transform.parent.transform.parent.gameObject.SetActive(true);
        StartCoroutine(ShowText(_killThemText, 2));
    }

    public void ShowLose()
    {
        Debug.Log(8);
        if (PlayerMoveController.Instance.gems == 0)
        {
            _loseGems.SetActive(false);
            _winGems.SetActive(false);
        }
        else
        {

            _loseGems.SetActive(true);
            _winGems.SetActive(true);
        }
        Time.timeScale = 0.1f;
        Debug.Log(9);
        LoseEnd();
    }

    public void ShowBoss()
    {
        StartCoroutine(ShowText(_bossComeScreen, 2));
    }

    private IEnumerator ShowText(GameObject textScr, int sec)
    {
        textScr.SetActive(true);
        yield return new WaitForSeconds(sec);
        textScr.SetActive(false);
    }

    private IEnumerator WinCor()
    {
        yield return ShowText(_winText, 1);
        PlayerMoveController.Instance.MagnitAllXP();
        yield return new WaitForSeconds(1);

        while (_updateScreen.activeInHierarchy)
        {
            yield return null;
        }
        //ShowChest();
        AfterChest();
    }

    public void UpdateCoinsGems()
    {
        foreach (var coin in _coins)
        {
            coin.text = PlayerMoveController.Instance.coins.ToString("F0");
        }
        foreach (var gem in _gems)
        {
            gem.text = PlayerMoveController.Instance.gems.ToString();
        }
    }

    public IEnumerator StartGame()
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(0.5f);
        _startText.text = "WAVE " + (_matchManager._curWave + 1).ToString();
        _startText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _startText.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.5f);
        //_startText.text = "READY";
        //_startText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1);
        //_startText.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.5f);
        //_startText.text = "STEADY";
        //_startText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1);
        //_startText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        _startText.text = "SURVIVE!";
        _startText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _startText.gameObject.SetActive(false);

        _matchManager.MainStagePart();

        if (_matchManager.needTutor)
        {
            ShowTutor(_tutorKillScreen);
        }

        yield return new WaitForSeconds(3);
        if (_matchManager.needTutor)
        {
            ShowTutor(_tutorKillScreen2);
        }
    }

    public void ShowChest()
    {
        _gameBlock.SetActive(false);
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
        _chestBlock.SetActive(true);
    }

    public void AfterChest()
    {
        _chestBlock.SetActive(false);


        if (PlayerMoveController.Instance.gems == 0)
        {
            _loseGems.SetActive(false);
            _winGems.SetActive(false);
        }
        else
        {

            _loseGems.SetActive(true);
            _winGems.SetActive(true);
        }

        switch (_matchManager._end)
        {
            case MatchManager.End.Win:
                WinEnd();
                break;
            case MatchManager.End.Lose:
                LoseEnd();
                break;
        }
    }

    private void LoseEnd()
    {
        Debug.Log(11);
        _loseScreen.SetActive(true);
        //GameObject wavesScreen = null;

        //switch (_matchManager._levelData.waveData.Count)
        //{
        //    case 1:
        //        wavesScreen = _loseScreen.transform.GetChild(0).gameObject;
        //        break;
        //    case 3:
        //        wavesScreen = _loseScreen.transform.GetChild(1).gameObject;
        //        break;
        //    case 5:
        //        wavesScreen = _loseScreen.transform.GetChild(2).gameObject;
        //        break;
        //    case 7:
        //        wavesScreen = _loseScreen.transform.GetChild(3).gameObject;
        //        break;
        //}
        //wavesScreen.GetComponent<WinScreenFlow>().ShowWinWave(_matchManager._curWave + 1);
    }

    private void WinEnd()
    {
        Time.timeScale = 0;
        if (_matchManager.needTutor)
        {
            _finWaveObj.SetActive(true);
            _nextWaveObj.SetActive(false);
        }
        else
        {
            _finWaveObj.SetActive(_matchManager.finalWave);
            _nextWaveObj.SetActive(!_matchManager.finalWave);
        }
        _winScreen.SetActive(true);
        _healthSlider.maxValue = PlayerMoveController.Instance._maxHealth;
        _healthSlider.value = PlayerMoveController.Instance._curHealth;

        TimeSpan result = TimeSpan.FromSeconds(_matchManager._spentTimeTotal);
        _finTime.text = result.ToString("mm':'ss");

        _charter.text = "CHARTER 1." + (PrefsManager.GetUnlockedLevelCharter1()).ToString();


    }

    private void OpenFinScreen()
    {
        _finLevelScreen.SetActive(true);
        _winScreen.SetActive(false);
    }

    private void FinLevel()
    {
        if (_matchManager.weaponUnlock != null && !WeaponsManager.Instance._availWeapons.Contains(_matchManager.weaponUnlock))
        {
            UnlockWeapon();
        }
        else if (_matchManager.level >= 3)
        {
            unlockCharter.SetActive(true);
        }
        else
        {
            BackToMenu();
        }
    }

    private void UnlockWeapon()
    {
        var weapon = _matchManager.weaponUnlock;
        _finLevelScreen.SetActive(false);
        _unlockWeaponScreen.SetActive(true);

        _unlockWeaponImage.sprite = weapon.weaponsLevels[0].Image;
        _unlockWeaponName.text = weapon.weaponsLevels[0].Name;
        _unlockWeaponDescr.text = weapon.weaponsLevels[0].Descr;
        _unlockWeaponDescr.ForceMeshUpdate();

        WeaponsManager.Instance.UnlockWeapon(weapon);
    }

    private void BackToMenu()
    {
        Debug.Log("||||||||||||||||||||||||||||||||||" + " " + PrefsManager.GetUnlockedLevelCharter1() + PrefsManager.GetChosenLevelCharter1());

        Time.timeScale = 1;
        GameSceneManager.Instance.Menu();
    }

    private void UpdateKills()
    {
        foreach (var text in _killTexts)
        {
            text.text = EnemySpawner.Instance.killedEnemy.ToString();
        }
        UpdateCoinsGems();
    }

    public void ChangeMusic()
    {
        var musicOn = PrefsManager.GetSound();
        if (musicOn == 1)
        {
            _musicCheck.transform.GetChild(1).gameObject.SetActive(false);

            _musicCheck.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            _musicCheck.transform.GetChild(0).gameObject.SetActive(false);

            _musicCheck.transform.GetChild(1).gameObject.SetActive(true);
        }

        _audioManager.SwitchSound();

    }
}

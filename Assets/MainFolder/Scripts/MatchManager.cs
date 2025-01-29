using Facebook.MiniJSON;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class MatchManager : MonoBehaviour
{
    private UIManager _uiManager;

    public float _maxTime = 60;
    public float _elapsedTime;
    public float _spentTimeTotal;

    [SerializeField] private bool _bossLevel;
    private bool _bossFight;

    private bool _gameEnd;

    public CharterData _charterData;
    public LevelData _levelData;
    public WaveData _waveData;
    public int _maxEnemies;
    public int _curWave;

    public bool finalWave;
    public bool needTutor;

    public WeaponTypeParent weaponUnlock;

    [SerializeField] private List<Transform> heroSpawnPoints = new List<Transform>();

    public int charter;

    public enum Stage
    {
        Start,
        Main,
        BossFight,
        KillThem,
        Win
    }

    public enum End
    {
        Win,
        Lose
    }

    public Stage _stage;
    private Stage _prevStage;
    public End _end;

    public int level;

    private void Awake()
    {
        
        if (SceneManager.GetActiveScene().name == "BattleSceneForest 1")
        {
           level = PrefsManager.GetChosenLevelCharter1();
            charter = 0;
        }
        else if ( SceneManager.GetActiveScene().name == "BattleSceneIce 1")
        {
            level = PrefsManager.GetChosenLevelCharter2();
            charter = 1;
        }

        if (level >= _charterData.levelData.Count)
        {
            level = _charterData.levelData.Count - 1;
        }
        _levelData = _charterData.levelData[level];

        _waveData = _levelData.waveData[_curWave];

        _maxTime = _waveData.duration;
        _bossLevel = _waveData.bossWave;
        _maxEnemies = _waveData.maxEnemies;
        weaponUnlock = _levelData.weapUnlocked;

        if (PrefsManager.GetFirstPlay() == 2)
        {
            needTutor = true;
        }
        else
        {
            needTutor = false;
        }

        //YsoCorp.GameUtils.YCManager.instance.OnGameStarted(level);
    }

    private void Start()
    {
        _stage = Stage.Start;
        _uiManager = GetComponent<UIManager>();
        _uiManager.StartCoroutine("StartGame");
        PlayerMoveController.healthChange.AddListener(() => StartCoroutine(CheckLoseCor()));

        var spawnPoint = heroSpawnPoints[UnityEngine.Random.Range(0, heroSpawnPoints.Count - 1)];

        PlayerMoveController.Instance.transform.position = new Vector3(spawnPoint.transform.position.x, 6, spawnPoint.transform.position.z);
        BossController.defeatBoss.AddListener(() => Win());

       // AudioManager.Instance.StartBattle();
        EnemySpawner.Instance.maxEnemy = _maxEnemies;
        EnemySpawner.Instance.enemiesDamageMultiplier = _waveData.enemiesDamageMultiplier;
        EnemySpawner.Instance.enemiesHealthMultiplier = _waveData.enemiesHealthMultiplier;
        EnemySpawner.Instance.enemiesSpeedMultiplier = _waveData.enemiesSpeedMultiplier;

        EnemySpawner.Instance.enemyTypes.Clear();
        EnemySpawner.Instance.enemyTypes.AddRange(_waveData.enemyData);

        CalculateReward();

        finalWave = _curWave + 1 < _levelData.waveData.Count ? false : true;

        GAManager.instance.OnLevelStarted(level);


        //LevelStartEventData eventData = new LevelStartEventData() { level = PrefsManager.GetChosenLevelCharter1(), days_since_reg = PrefsManager.DaysFromReg() };
        //string json = JsonUtility.ToJson(eventData);
        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.level_start, json);

        //WaveStartEventData eventData2 = new WaveStartEventData() { level = PrefsManager.GetChosenLevelCharter1(), wave = _curWave, days_since_reg = PrefsManager.DaysFromReg() };
        //json = JsonUtility.ToJson(eventData2);
        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.wave_start, json);

        //AppMetrica.Instance.SendEventsBuffer();

      //  StartCoroutine(FirstTutor());
    }

    private IEnumerator FirstTutor()
    {
        yield return new WaitForSeconds(1);
        if (needTutor)
       {
            _uiManager.ShowTutor(_uiManager._tutorGreetScreen);

       }
    }

    private void CalculateReward()
    {
        var levelDuration = 0;

        foreach (WaveData waveData in _levelData.waveData) 
        {
            levelDuration += waveData.duration;
        }


        var enemiesPerLevel = levelDuration / EnemySpawner.Instance.pause;
        var goldPerEnemy = _levelData.maxReward / enemiesPerLevel;
        goldPerEnemy = goldPerEnemy <= 0 ? 1 : goldPerEnemy;
        EnemySpawner.Instance.goldPerEnemy = goldPerEnemy;
    }

    public void NewWave()
    {
        PlayerMoveController.Instance.allowRegen = true;
        _curWave++;
        EnemySpawner.Instance.wavenum = _curWave;
        _waveData = _levelData.waveData[_curWave];

        EnemySpawner.Instance.enemiesDamageMultiplier = _waveData.enemiesDamageMultiplier;
        EnemySpawner.Instance.enemiesHealthMultiplier = _waveData.enemiesHealthMultiplier;
        EnemySpawner.Instance.enemiesSpeedMultiplier = _waveData.enemiesSpeedMultiplier;

        _maxTime = _waveData.duration;
        _bossLevel = _waveData.bossWave;
        _maxEnemies = _waveData.maxEnemies;

        _stage = Stage.Start;
        _uiManager.NewWave();
        PlayerMoveController.Instance.NewWave();
        _uiManager.StartCoroutine("StartGame");
        EnemySpawner.Instance.NewWave(_maxEnemies);
        AudioManager.Instance.StartBattle();

        finalWave = _curWave+1 < _levelData.waveData.Count? false : true;

        WaveStartEventData eventData = new WaveStartEventData() { level = PrefsManager.GetChosenLevelCharter1(), wave = _curWave, days_since_reg = PrefsManager.DaysFromReg() };
        string json = JsonUtility.ToJson(eventData);

        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.wave_start, json);

        //AppMetrica.Instance.SendEventsBuffer();
    }

    public void Revive()
    {
        _gameEnd = false;
        if (PlayerMoveController.Instance.deadFromFall)
        {
            PlayerMoveController.Instance.transform.position = heroSpawnPoints[UnityEngine.Random.Range(0, heroSpawnPoints.Count - 1)].transform.position;
        }
        PlayerMoveController.Instance.Revive();
        _stage = _prevStage;
        WeaponsManager.Instance.gameObject.SetActive(true);

        switch (_stage)
        {
            case Stage.Main:
                EnemySpawner.Instance.AllowSpawn();
                break;

            case Stage.BossFight:
                EnemySpawner.Instance.StopSpawn();
                break;

            case Stage.KillThem:
                EnemySpawner.Instance.StopSpawn();
                break;

        }
    }

    private void OnEnable()
    {
        // Subscribe to the OnTick event when the script is enabled
        Ticker.OnTick += UpdateLogic;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnTick event when the script is disabled or destroyed
        Ticker.OnTick -= UpdateLogic;
    }

    private void UpdateLogic()
    {
        finalWave = _curWave + 1 < _levelData.waveData.Count ? false : true;


        switch (_stage)
        {
            case Stage.Main:
                MainStage();
                break;

            case Stage.BossFight:
                break;

            case Stage.KillThem:
                KillThemPart();
                break;

            case Stage.Win:
                break;
            case Stage.Start:
                break;
        }

        _spentTimeTotal+=Time.deltaTime;

        if (_stage == Stage.KillThem && EnemySpawner.Instance.curEnemy ==0)
        {
            Win();
        }

    }

    private void MainStage()
    {
        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            if (_bossLevel)
            {
                _stage = Stage.BossFight;
                BossPart();
            }
            else
            {
                _stage = Stage.KillThem;
                KillThemPartStart();
            }
        }

        else
        {
            var progress = EnemySpawner.Instance.progress = 1 - _elapsedTime / _maxTime;

            _uiManager.UpdateSliders(_levelData.waveData.Count,_curWave, progress);

            ConvertTime();
        }
    }

    private void ConvertTime()
    {
        TimeSpan result = TimeSpan.FromSeconds(_elapsedTime);
        string fromTimeString = result.ToString("mm':'ss");
        _uiManager.ShowTime(fromTimeString);
    }

    public void MainStagePart()
    {
        _elapsedTime = _maxTime;
        ConvertTime();
        EnemySpawner.Instance.gameObject.SetActive(true);
        EnemySpawner.Instance.AllowSpawn();
        _stage = Stage.Main;
    }

    public void Win()
    {
        GAManager.instance.OnLevelComplete(level);
        _end = End.Win;
        _gameEnd = true;
        PlayerMoveController.Instance.allowRegen = false;
        if (finalWave)
        {

            PlayerMoveController.Instance.coins += 0;
            PlayerMoveController.Instance.gems += 0;

            if (charter == 0)
            {
                if (PrefsManager.GetChosenLevelCharter1() == PrefsManager.GetUnlockedLevelCharter1())
                {
                    PrefsManager.ChangeUnlockedLevel1(1);
                    PrefsManager.ChangeChosenLevel1(PrefsManager.GetUnlockedLevelCharter1());
                }
                else
                {
                    PrefsManager.ChangeChosenLevel1(PrefsManager.GetChosenLevelCharter1() + 1);
                }
            }
            else
            {
                if (PrefsManager.GetChosenLevelCharter2() == PrefsManager.GetUnlockedLevelCharter2())
                {
                    PrefsManager.ChangeUnlockedLevel2(1);
                    PrefsManager.ChangeChosenLevel2(PrefsManager.GetUnlockedLevelCharter2());
                }
                else
                {
                    PrefsManager.ChangeChosenLevel2(PrefsManager.GetChosenLevelCharter2() + 1);
                }
            }
            PrefsManager.ChangePlayerXP(60);

            //LevelCompleteEventData eventData2 = new LevelCompleteEventData()
            //{
            //    level = PrefsManager.GetChosenLevelCharter1(),
            //    days_since_reg = PrefsManager.DaysFromReg(),
            //    days_since_reg = PrefsManager.DaysFromReg()
            //    heroLevel = (int)PlayerMoveController.Instance._level,
            //    time_spent = (int)_spentTimeTotal
            //};

            //string json2 = JsonUtility.ToJson(eventData2);
            //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.level_complete, json2);

            //AppMetrica.Instance.SendEventsBuffer();
        }

        PrefsManager.ChangeCoins((int)PlayerMoveController.Instance.coins);
        PrefsManager.ChangeGems(PlayerMoveController.Instance.gems);
        EnemySpawner.Instance.StopSpawn();
        StopAllCoroutines();
        _uiManager.ShowWin();
        AudioManager.Instance.OnWin();

        //WaveCompleteEventData eventData3 = new WaveCompleteEventData()
        //{
        //    level = PrefsManager.GetChosenLevelCharter1(),
        //    wave = _curWave,
        //    days_since_reg = PrefsManager.DaysFromReg(),
        //};

        //string json3 = JsonUtility.ToJson(eventData3);
        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.wave_complete, json3);

        //AppMetrica.Instance.SendEventsBuffer();
    }

    public void KillThemPartStart()
    {
        EnemySpawner.Instance.StopSpawn();
        _uiManager.KillThemPart();
    }

    public void KillThemPart()
    {
        _uiManager.ShowEnemies();
        if (EnemySpawner.Instance.curEnemy <=0)
        {
            _stage = Stage.Win;
            Win();
        }
    }

    public void CheckLose()
    {
        StartCoroutine(CheckLoseCor());
    }
    public IEnumerator CheckLoseCor()
    {
        if (PlayerMoveController.Instance._curHealth <= 0 )
        {
            AudioManager.Instance.OnLose();

            _gameEnd = true;
            _prevStage = _stage;
            _stage = Stage.Win;
            _end = End.Lose;
            EnemySpawner.Instance.StopSpawn();
            yield return new WaitForSeconds(3);
            Lose();
        }
    }

    private void Lose()
    {
        GAManager.instance.OnLevelLose(level);
        //LevelFailEventData eventData = new LevelFailEventData() 
        //{
        //    level = PrefsManager.GetChosenLevelCharter1(), 
        //    wave = _curWave, 
        //    days_since_reg = PrefsManager.DaysFromReg(), 
        //    heroLevel = (int)PlayerMoveController.Instance._level, 
        //    reason = PlayerMoveController.Instance.deadFromFall?"fall":"killed", 
        //    time_spent = (int)_spentTimeTotal 
        //};

        //string json = JsonUtility.ToJson(eventData);
        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.level_fail, json);

        //AppMetrica.Instance.SendEventsBuffer();

        //YsoCorp.GameUtils.YCManager.instance.OnGameFinished(false);
        _uiManager.ShowLose();
        PrefsManager.ChangeCoins((int)PlayerMoveController.Instance.coins);
        PrefsManager.ChangeGems(PlayerMoveController.Instance.gems);
        StopAllCoroutines();
    }

    private void BossPart()
    {
        _stage = Stage.BossFight;
        _gameEnd = false;
        _bossFight = true;
        EnemySpawner.Instance.StopSpawn();
        EnemySpawner.Instance.SpawnBoss();
        _uiManager.ShowBoss();

        AudioManager.Instance.BossFight();
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);

    }

}

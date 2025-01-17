using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{

    public MatchManager matchManager;
    public float maxEnemy = 10;
    public float curEnemy = 0;

    public float killedEnemy = 0;
    public static UnityEvent kill = new UnityEvent();

    [SerializeField] private Transform _poolParent;
    [SerializeField] private GameObject _enemyParent;

    public float pause;
    private float _xPlus;
    private float _zPlus;

    public List<GameObject> _enemies = new List<GameObject>();
    public List<SpawnPoint> _points = new List<SpawnPoint>();
    public ObjectPool<GameObject> _enemyPool;
    private GameObject _tempEnemy;
    public List<GameObject> _bosses = new List<GameObject>();
    public static EnemySpawner Instance;

    public List<EnemySO> enemyTypes;
    private int _numTypes;

    public float progress;

    public List<EnemyController> canBeShootList = new List<EnemyController>();

    private bool _allowSpawn = true;

    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _gem;
    [SerializeField] private GameObject _potion;

    public float enemiesHealthMultiplier;
    public float enemiesDamageMultiplier;
    public float enemiesSpeedMultiplier;

    public float goldPerEnemy;

    public Material whiteMat;

    public int wavenum;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _enemyPool = new ObjectPool<GameObject>(
         createFunc: () =>

             Instantiate(_enemyParent, _poolParent)
         ,
         actionOnGet: (obj) =>
         {

             obj.GetComponent<EnemyController>().enemyData = enemyTypes[SpawnRandomizer()];
             obj.SetActive(true);

         }
         ,
         actionOnRelease: (obj) => obj.SetActive(false),
         actionOnDestroy: (obj) => Destroy(obj),
         collectionCheck: false,
         defaultCapacity: 50,
         maxSize: 50
         );

        _numTypes = enemyTypes.Count;
        progress = 0;

        gameObject.SetActive(false);
        Debug.Log(matchManager);
        Debug.Log(matchManager._waveData);

        enemiesHealthMultiplier = matchManager._waveData.enemiesHealthMultiplier;
        enemiesDamageMultiplier = matchManager._waveData.enemiesDamageMultiplier;
     enemiesSpeedMultiplier = matchManager._waveData.enemiesSpeedMultiplier;

        for (int i = 0; i < 30; i++)
        {
            SpawnEnemy();
        }

        for (int i = 0; i < 30; i++)
        {
            DespawnEnemy(_poolParent.transform.GetChild(i).gameObject);
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
        pause += Time.deltaTime;

        if (curEnemy < maxEnemy && _points.Count > 0 && pause > ((1 - progress) / (1 + wavenum)) && _allowSpawn)
        {
            pause = 0;
            SpawnEnemy();
        }

        Transform _target = PlayerMoveController.Instance.transform;
        foreach (var e in _enemies)
        {
            if (Vector3.Distance(e.transform.position, _target.position) > 2)
            {

                var pos = e.transform.localPosition;
                var rot = e.transform.rotation.eulerAngles;

                e.transform.LookAt(_target.position);
                e.transform.rotation = Quaternion.Euler(rot.x, e.transform.rotation.eulerAngles.y, rot.z);
                //e.transform.localPosition = new Vector3(pos.x, pos.y + 0.05f, pos.z);
                e.transform.Translate(Vector3.forward / 10 );
                

            }
        }

        CheckQuant();

    }

    private void CheckQuant()
    {

        var temp = 0;

        for (var i = 0; i < _poolParent.childCount; i++)
        {
            if (_poolParent.GetChild(i).gameObject.activeInHierarchy)
            {
                temp++;
            }
        }

        curEnemy = temp;

    }

    private void SpawnEnemy()
    {
        var tempEnemy = _enemyPool.Get();
        SpawnPoint p = _points[Random.Range(0, _points.Count - 1)];

        tempEnemy.transform.position = p.transform.position;

        _enemies.Add(tempEnemy);
    }

    public void SpawnBoss()
    {
        GameObject boss = null;
        switch (wavenum)
        {
            case 2:
                boss = _bosses[0];
                break;
            case 4:
                boss = _bosses[1];
                break;
            case 6:
                boss = _bosses[2];
                break;
            default:
                boss = _bosses[0];
                break;
        }

        curEnemy++;
        boss = Instantiate(boss, _poolParent);
        //_boss.SetActive(true);
        //_boss.GetComponent<BossController>()._dead = false;
        //_boss.GetComponent<EnemyController>()._curHealth = _boss.GetComponent<EnemyController>()._maxHealth;
        SpawnPoint p = _points[Random.Range(0, _points.Count - 1)];

        boss.transform.position = p.transform.position;

        _enemies.Add(boss);
    }
    public void DespawnEnemy(GameObject enemy)
    {
        if (progress != 0)
        {
            PlayerMoveController.Instance.coins += goldPerEnemy;

            killedEnemy++;
            kill.Invoke();
        }
        _enemyPool.Release(enemy);
        _enemies.Remove(enemy);
        canBeShootList.Remove(enemy.GetComponent<EnemyController>());
    }

    private int SpawnRandomizer()
    {
        int num = Mathf.RoundToInt(progress * (enemyTypes.Count - 1));
        num = num >= (enemyTypes.Count - 1) ? (enemyTypes.Count - 1) : num;

        int rand = Random.Range(0, 100);

        if (rand < 50f)
            return num;
        else if (rand < 70 && num > 0)
            return num - 1;
        else if (rand < 85 && num > 1)
            return num - 2;
        else if (rand < 95f && num > 2)
            return num - 3;
        else if (rand <= 100f && num > 3)
            return num - 4;
        else
            return num;
    }

    public void StopSpawn()
    {
        _allowSpawn = false;
    }

    public void SpawnBonus()
    {


                if (!_gem.activeInHierarchy)
                {
                    _gem.SetActive(true);
                    SpawnPoint p = _points[Random.Range(0, _points.Count - 1)];
                    _gem.transform.position = new Vector3(p.transform.position.x, PlayerMoveController.Instance.transform.position.y, p.transform.position.z);
                }

                if (!_potion.activeInHierarchy)
                {
                    _potion.SetActive(true);
                    SpawnPoint p = _points[Random.Range(0, _points.Count - 1)];
                    _potion.transform.position = new Vector3(p.transform.position.x, PlayerMoveController.Instance.transform.position.y, p.transform.position.z);
                }

                if (!_coin.activeInHierarchy)
                {
                    _coin.SetActive(true);
                    SpawnPoint p = _points[Random.Range(0, _points.Count - 1)];
                    _coin.transform.position = new Vector3(p.transform.position.x, PlayerMoveController.Instance.transform.position.y, p.transform.position.z);
                }
            
        
    }

    IEnumerator DespawnItem(GameObject item)
    {
        yield return new WaitForSeconds(5);
        item.SetActive(false);
    }

    public void NewWave(int maxEnemies)
    {
        gameObject.SetActive(true);
        progress = 0;
        for (var i = 0; i < _poolParent.childCount; i++)
        {
            DespawnEnemy(_poolParent.GetChild(i).gameObject);
        }
        _enemyPool.Clear();
        _enemies.Clear();
        canBeShootList.Clear();

        maxEnemy = maxEnemies;

    }

    public void AllowSpawn()
    {
        _allowSpawn = true;
        InvokeRepeating("SpawnBonus", 5, 5);
        InvokeRepeating("CheckQuant", 0.2f, 0.2f);
    }
}
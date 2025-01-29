using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemySO enemyData;

    [HideInInspector]
    public Rigidbody _rb;
    [HideInInspector] public Vector3 _hitAway;
    [HideInInspector] public Transform _target;
    public bool _dead = false;
    public bool _fly = false;
    public bool _damagePause = false;
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public Vector3 _prevPosition;
    public float _damage = 5;
    [HideInInspector] public PlayerMoveController _player;
    [HideInInspector] public Animator _animator;
    public Material _whiteMaterial;
    public Material _tempMaterialMain;
    [HideInInspector] public Collider _collider;
    [HideInInspector] public bool _deadCorStarted = false;
    public GameObject _meshObj;
    public ParticleSystem _hitPS;
    public GameObject _deathPS;
    public TMP_Text _hitText;
    public Animator _textAnimator;

    public float maxSpeed = 0.5f;
    [HideInInspector]
    public float _moveSpeed = 0.5f;
    public float _maxHealth = 10;
    public float _curHealth = 10;

    public bool canBeShoot = false;

    public Slider _hpSlider;
    public DamageText damageText;
    private bool _getCrit;
    private DeathEffect _deathEffect;
    private HitEffect _hitEffect;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }
    public virtual void Start()
    {
        damageText = GetComponentInChildren<DamageText>();
        damageText.gameObject.SetActive(false);
        _player = PlayerMoveController.Instance;
        _target = _player.transform;

        LoadDataFromSO();

        _collider = GetComponentInChildren<Collider>();
        _whiteMaterial = EnemySpawner.Instance.whiteMat;
        _deathEffect = DeathEffect.Instance;
        _hitEffect = HitEffect.Instance;
    }

    public virtual void OnEnable()
    {
        // Subscribe to the OnTick event when the script is enabled
        Ticker.OnTick += UpdateLogic;
        if (_collider != null)
        {
            LoadDataFromSO();

        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnTick event when the script is disabled or destroyed
        EnemySpawner.Instance.canBeShootList.Remove(this);
        Ticker.OnTick -= UpdateLogic;
    }

    public virtual void UpdateLogic()
    {
        //if (!_dead && !_fly && _target != null)
        //    EnemyMove();
        if (_dead)
        {
            Death();
        }
    }

    public void EnemyMove()
    {
        if (Vector3.Distance(transform.position, _target.position) > 2)
        {


            //var pos = transform.localPosition;
            //var rot = transform.rotation.eulerAngles;

            //transform.LookAt(_target.position);
            //transform.rotation = Quaternion.Euler(rot.x, transform.rotation.eulerAngles.y, rot.z);
            //pos.Set(pos.x, pos.y, pos.z - 0.5f);

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.GetHit(collision.transform.position, transform.position, 3, _damage, _rb);
            //_rb.AddRelativeForce(Vector3.back*0.7, ForceMode.Impulse);
        }
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Despawn"))
        {
            EnemySpawner.Instance.canBeShootList.Remove(this);
            EnemySpawner.Instance.DespawnEnemy(gameObject);
            Clear();
        }
        else if (collider.gameObject.CompareTag("LevelCollider"))
        {
            _collider.isTrigger = false;
            _fly = false;
            // _animator.SetBool("Fly", false);
        }
    }
    public virtual void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("LevelCollider"))
        {
            _collider.isTrigger = false;
            _fly = false;
            //_animator.SetBool("Fly", false);
        }
    }

    public virtual void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.CompareTag("LevelCollider"))
        {
            _collider.isTrigger = true;
            _fly = true;
            // _animator.SetBool("Fly", true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Spawner"))
        {
            canBeShoot = true;
            EnemySpawner.Instance.canBeShootList.Add(this);
        }
    }

    public virtual void GetHit(float force, float upForce, float damage)
    {
        if (EnemySpawner.Instance.curEnemy > 1 || (EnemySpawner.Instance.curEnemy == 1 && WeaponsManager.Instance._matchManager._stage == MatchManager.Stage.BossFight))
        {
            if (!_damagePause && !_dead)
            {
                AudioManager.Instance.OnEnemyGetHit();
                // _damagePause = true;

                //New things Test block
                PlayerMoveController.Instance.Vamp(damage);

                _getCrit = false;
                float critChance = Random.Range(0.00f, 1.00f);
                if (critChance < PlayerMoveController.Instance._critChance)
                {
                    damage *= 2;
                    _getCrit = true;
                }

                _curHealth -= damage;
                _dead = _curHealth > 0 ? false : true;
                damageText.Show(damage, _getCrit);

                if (_dead)
                {
                    _hpSlider.gameObject.SetActive(false);
                    // _deathPS.SetActive(true);
                    _deathEffect.Play(this.transform);
                    _rb.AddRelativeForce((Vector3.back + Vector3.up) * 10, ForceMode.Impulse);
                    // _animator.SetBool("Die", true);
                    if (gameObject.activeInHierarchy)
                        StartCoroutine(DeathCor());
                    else
                        Death();
                }
                else
                {
                    _hpSlider.gameObject.SetActive(true);
                    _hpSlider.value = (_curHealth / _maxHealth) * 100;
                    _hitEffect.Play(this.transform);
                    //_hitPS.Play();
                    _rb.AddRelativeForce(Vector3.back * Random.Range(force / 1.2f, force / 1.2f) + Vector3.up * Random.Range(upForce / 1.2f, upForce / 1.2f), ForceMode.Impulse);
                    _animator.SetTrigger("GetHit");
                    // StartCoroutine(DamagePause());
                }
            }
        }
        else if (EnemySpawner.Instance.curEnemy == 1 && WeaponsManager.Instance._matchManager._stage == MatchManager.Stage.KillThem)
        {

            _curHealth -= damage;
            _dead = _curHealth > 0 ? false : true;

            if (_dead)
            {
                // _animator.SetBool("Die", true);
                if (gameObject.activeInHierarchy)
                    StartCoroutine(DeathCor());
                else
                    Death();
            }
        }
    }

    public virtual void GetHit(float damage)
    {
        //if (EnemySpawner.Instance.curEnemy > 0)
        //{
            if (!_damagePause )
            //&& !_dead
            
            {
                AudioManager.Instance.OnEnemyGetHit();
                // _damagePause = true;

                //New things Test block
                PlayerMoveController.Instance.Vamp(damage);

                _getCrit = false;
                float critChance = Random.Range(0.00f, 1.00f);
                if (critChance < PlayerMoveController.Instance._critChance)
                {
                    damage *= 2;
                    _getCrit = true;
                }
            
        _curHealth -= damage;
        _dead = _curHealth > 0 ? false : true;
        damageText.Show(damage, _getCrit);

        if (_dead)
        {
            _hpSlider.gameObject.SetActive(false);
            _deathEffect.Play(this.transform);
            // _deathPS.SetActive(true);
            // _rb.AddRelativeForce((Vector3.back + Vector3.up) * 10, ForceMode.Impulse);
            // _animator.SetBool("Die", true);
            if (gameObject.activeInHierarchy)
                StartCoroutine(DeathCor());
            else
                Death();
        }
        else
        {
            _hpSlider.gameObject.SetActive(true);
            _hpSlider.value = (_curHealth / _maxHealth) * 100;
            _hitEffect.Play(this.transform);
            //_hitPS.Play();
            //_rb.AddRelativeForce(Vector3.back * Random.Range(force / 1.2f, force / 1.2f) + Vector3.up * Random.Range(upForce / 1.2f, upForce / 1.2f), ForceMode.Impulse);
            _animator.SetTrigger("GetHit");
            //StartCoroutine(DamagePause());
        }
    }

                //else if (EnemySpawner.Instance.curEnemy == 1 && WeaponsManager.Instance._matchManager._stage == MatchManager.Stage.KillThem)
                //{

                //    _curHealth -= damage;
                //    _dead = _curHealth > 0 ? false : true;

                //    if (_dead)
                //    {
                //        // _animator.SetBool("Die", true);
                //        if (gameObject.activeInHierarchy)
                //            StartCoroutine(DeathCor());
                //        else
                //            Death();
                //    }
                //}
            
    }

    private IEnumerator DeathCor()
    {

        XPSpawner.Instance.SpawnXP(transform);




        _deadCorStarted = true;

        elapsedTime = 0;


        // _meshObj.GetComponent<Renderer>().material = _whiteMaterial;

        _collider.enabled = false;
        while (elapsedTime < 0.1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        _meshObj.SetActive(false);
        EnemySpawner.Instance.canBeShootList.Remove(this);
        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EnemySpawner.Instance.DespawnEnemy(gameObject);
        _hpSlider.gameObject.SetActive(false);
        Clear();

    }

    private void Death()
    {
        XPSpawner.Instance.SpawnXP(transform);

        elapsedTime = 0;



        _deadCorStarted = true;

        //  _meshObj.GetComponent<Renderer>().material = _whiteMaterial;
        _meshObj.SetActive(false);
        EnemySpawner.Instance.canBeShootList.Remove(this);
        EnemySpawner.Instance.DespawnEnemy(gameObject);
    
        _hpSlider.gameObject.SetActive(false);
        Clear();
    }

    public virtual void Clear()
    {
        _curHealth = _maxHealth;
        _fly = false;
        _dead = false;
        _rb.useGravity = true;
        _rb.drag = 0;
        _deadCorStarted = false;
        _damagePause = false;
        //_deathPS.SetActive(false);
        //_meshObj.SetActive(true);
        EnemySpawner.Instance.canBeShootList.Remove(this);
        canBeShoot = false;

    }

    private IEnumerator DamagePause()
    {
        elapsedTime = 0;
        // _meshObj.GetComponent<Renderer>().material = _whiteMaterial;
        yield return new WaitForSeconds(0.2f);
        // _meshObj.GetComponent<Renderer>().material = _tempMaterialMain;
        _meshObj.GetComponent<Renderer>().material = enemyData.mainMat;
        _damagePause = false;
    }

    public virtual void LoadDataFromSO()
    {
        _collider = GetComponentInChildren<Collider>();
        GetComponent<Rigidbody>().Sleep();
        StartCoroutine(ColliderEnable());
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        var x0 = transform;
        var x1 = x0.GetChild(0);
        var x3 = enemyData.enemyNumber;
        var x2 = x1.GetChild(x3);
        x2.gameObject.SetActive(true);
        var prevMesh = _meshObj;
        _meshObj = transform.GetChild(0).GetChild(enemyData.enemyNumber).GetChild(0).gameObject;
        //if (prevMesh != _meshObj)
        //{
        //    _mainMaterial = _meshObj.GetComponent<Renderer>().material;
        //}

        maxSpeed = enemyData.maxSpeed * EnemySpawner.Instance.enemiesSpeedMultiplier;
        _moveSpeed = maxSpeed;
        _maxHealth = enemyData.maxHealth * EnemySpawner.Instance.enemiesHealthMultiplier;

        _damage = enemyData.damage * EnemySpawner.Instance.enemiesDamageMultiplier;
        //Destroy(_tempMaterialMain);
        //_tempMaterialMain = new Material(enemyData.mainMat);
        _meshObj.GetComponent<Renderer>().material = enemyData.mainMat;
        //_tempMaterialMain.SetColor("_EmissionColor", Color.white * 1);

        _curHealth = _maxHealth;

        // _deathPS.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = enemyData.deathMat;



        _animator = transform.GetChild(0).GetChild(enemyData.enemyNumber).GetComponentInChildren<Animator>();
        _hpSlider.gameObject.SetActive(false);

        _damagePause = false;
        damageText.Stop();

        _meshObj.SetActive(true);
        
    }

    IEnumerator ColliderEnable()
    {
        yield return new WaitForSeconds(1);
        _collider.enabled = true;
        GetComponent<Rigidbody>().WakeUp();
    }
}

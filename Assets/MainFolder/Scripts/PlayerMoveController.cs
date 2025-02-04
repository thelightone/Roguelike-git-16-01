using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Cinemachine;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Security;
using UnityEngine.Experimental.GlobalIllumination;


public class PlayerMoveController : MonoBehaviour
{
    //INIT DATA BLOCK
    public static PlayerMoveController Instance;
    public HeroData _heroData;
    public ShopData _shopData;
    public UpgradePerLevel _upgradesData;
    public MatchManager _matchManager;

    // MOVE BLOCK
    [SerializeField] private FloatingJoystick _joystick;
    public float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private Vector3 _moveVector;
    private Animator _animatorController;
    private bool _grounded;

    // GET HIT BLOCK
    public bool damagePause;
    private Rigidbody _rb;
    private Vector3 _hitAway;
    public static UnityEvent healthChange = new UnityEvent();
    private float _elapsedTime;
    private Material _tempMaterial;
    [SerializeField] private GameObject _meshObj;
    private Collider _collider;
    public float _defence;

    // HEALTH BLOCK
    public float _curHealth;
    public float _maxHealth = 100;
    public float _regeneration;
    private float _regenTime;
    public bool allowRegen;
    public float _vamp;
    public HPSlider hpSlider;

    // XP BLOCK
    public float _curXP;
    public float _maxXP = 50;
    public float _level;
    public static UnityEvent xpChange = new UnityEvent();
    public static UnityEvent levelUpChange = new UnityEvent();
    public static UnityEvent getMaxLevel = new UnityEvent();
    [SerializeField] private ParticleSystem _getXPPS;
    public GameObject levelUpEffect;
    private float _xpCollectMultiplier;


    // ATTACK BLOCK
    private float _attackSpeed = 2;
    public float _critChance;
    public WeaponTypeParent _baseWeapon;
    public WeaponParent weaponParent;

    //CINEMACHINE
    [SerializeField] private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _noise;
    private bool zoomed;

    //COLLECT
    public float coins;
    public int gems;
    public static UnityEvent balance = new UnityEvent();
    [SerializeField] private ParticleSystem _getCoin;
    [SerializeField] private ParticleSystem _getGem;
    [SerializeField] private ParticleSystem _getPotion;
    private float _coinCollectMultiplier;
    public bool deadFromFall;
    public bool maxLevel;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponentInChildren<Rigidbody>();
        _animatorController = GetComponentInChildren<Animator>();

        _collider = GetComponent<Collider>();
        Material thisMaterial = _meshObj.GetComponent<Renderer>().material;
        _tempMaterial = new Material(thisMaterial);

        _meshObj.GetComponent<Renderer>().material = _tempMaterial;

        ParamsInit();

        xpChange.Invoke();
        healthChange.Invoke();

        _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        allowRegen = true;
    }

    private void ParamsInit()
    {
        _level = 1;
        _curXP = 0;
        _maxXP = 50;

        InitSpecificParam(ref _maxHealth, _heroData.heroMaxHealth, _upgradesData.MaxHealth, _shopData.heroMaxHealthLevel);
        _curHealth = _maxHealth;
        InitSpecificParam(ref _regeneration, _heroData.heroRegenerate, _upgradesData.Regenerate, _shopData.heroRegenerateLevel);
        InitSpecificParam(ref _moveSpeed, _heroData.heroSpeed, _upgradesData.Speed, _shopData.heroSpeedLevel);
        InitSpecificParam(ref _defence, _heroData.heroDefence, _upgradesData.Defence, _shopData.heroDefenceLevel);
        InitSpecificParam(ref _coinCollectMultiplier, _heroData.heroMultiplyCoinCollect, _upgradesData.MultiplyCoinCollect, _shopData.heroMultiplyCoinCollectLevel);
        InitSpecificParam(ref _xpCollectMultiplier, _heroData.heroMultiplyXpReceived, _upgradesData.MultiplyXpReceived, _shopData.heroMultiplyXpReceivedLevel);

        //New test parameters
        _vamp = 0;
        _critChance = 0.1f;
    }

    private void InitSpecificParam(ref float heroParam, float heroSOParam, float upgradePerLevel, float level)
    {
        if (level == 0)
        {
            heroParam = heroSOParam;
        }
        else if (level == 1)
        {
            heroParam = heroSOParam * _upgradesData.basicModifier;
        }
        else
        {
            heroParam = heroSOParam * (_upgradesData.basicModifier + (upgradePerLevel * (level - 1)));
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
        if (_curHealth > 0)
            Move();
        _regenTime += Time.deltaTime;
        if (_regenTime > 1)
        {
            Regenerate();
            _regenTime = 0;
        }
    }

    private void Regenerate()
    {
        if (_curHealth < _maxHealth && _curHealth > 0 && allowRegen)
        {
            _curHealth += _regeneration + _heroData.heroRegenerate;
            healthChange.Invoke();
        }
    }


    private void Move()
    {
        _moveVector = Vector3.zero;
        _moveVector.x = _joystick.Horizontal * _moveSpeed * Time.deltaTime;
        _moveVector.z = _joystick.Vertical * _moveSpeed * Time.deltaTime;

        var absHor = Math.Abs(_joystick.Horizontal);
        var absVer = Math.Abs(_joystick.Vertical);

        if (absHor > 0.1 || absVer > 0.1)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = transform.position + _moveVector;

            var animSpeed = absHor > absVer ? absHor : absVer;
            _animatorController.SetFloat("MoveSpeed", animSpeed * _moveSpeed);
            _animatorController.SetBool("Move", true);
            //if (!zoomed)
            //    _camera.m_Lens.FieldOfView = 44 + animSpeed;

            // if (!zoomed)
            // StartCoroutine(ZoomOut());
        }

        else if (absHor == 0 || absVer == 0)
        {
            _animatorController.SetBool("Move", false);
            if (!zoomed && _camera.m_Lens.FieldOfView != 44)
                StartCoroutine(ZoomIn());
        }
    }

    public void GetHit(Vector3 collision, Vector3 weapon, float force, float damage, Rigidbody enemy)
    {
        
        if (!damagePause)
        {

            Debug.Log("||||||||||||||||||||||||||||||||||" + " " + _curHealth);

            Debug.Log(_curHealth + " " + damage + " " + _defence);
            _curHealth -= (damage - (damage * _defence));
            allowRegen = false;
            damagePause = true;
            _hitAway = ((collision - weapon)).normalized;
          //  _rb.AddForce(_hitAway * force, ForceMode.Impulse);
            //_curHealth -= (damage - (damage * _defence));
            enemy.AddRelativeForce(Vector3.back * 2, ForceMode.Impulse);
            AudioManager.Instance.OnGetHit();
            //Handheld.Vibrate();

            if (_curHealth <= 0)
            {

                Death();
            }
            else
            {
                _animatorController.SetTrigger("GetHit");
               healthChange.Invoke();
               hpSlider.UpdateHealth();
                StartCoroutine(DamagePause());
            }
        }

        Debug.Log("||||||||||||||||||||||||||||||||||after" + " " + _curHealth);
    }

    public void GetHit(Vector3 collision, Vector3 weapon, float force, float damage)
    {
        if (!damagePause)
        {
            Debug.Log("||||||||||||||||||||||||||||||||||after" + " " + _curHealth);

            Debug.Log(_curHealth+" "+damage+" "+_defence);
            _curHealth -= (damage - (damage * _defence));

            allowRegen = false;
            damagePause = true;
            _hitAway = ((collision - weapon)).normalized;
          //  _rb.AddForce(_hitAway * force, ForceMode.Impulse);
            AudioManager.Instance.OnGetHit();
            //Handheld.Vibrate();

            if (_curHealth <= 0)
            {
                Debug.Log(1);
                Death();
            }
            else
            {
                _animatorController.SetTrigger("GetHit");
                healthChange.Invoke();
                hpSlider.UpdateHealth();
                StartCoroutine(DamagePause());
            }
        }

        Debug.Log("||||||||||||||||||||||||||||||||||" + " " + _curHealth);
    }
    private void Death()
    {
        WeaponsManager.Instance.gameObject.SetActive(false);
        var pos = transform.position;
        _joystick.gameObject.SetActive(false);
        _moveVector = new Vector3(0, 0, 0);
        _rb.linearDamping = 100;
        _rb.mass = 100;
        _collider.isTrigger = true;
        Debug.Log(3);
        _animatorController.SetBool("Death", true);
       // healthChange.Invoke();
       hpSlider.UpdateHealth();
        _matchManager.CheckLose();
        _elapsedTime = 0;

        //_tempMaterial.SetColor("_EmissionColor", Color.white * 100);
        //yield return new WaitForSeconds(0.1f);
        //_tempMaterial.SetColor("_EmissionColor", Color.white * 1);
        //_elapsedTime += Time.deltaTime;
        //yield return new WaitForSeconds(0.1f);



    }

    private IEnumerator DamagePause()
    {
        _elapsedTime = 0;


        _noise.m_AmplitudeGain = 3;

        //_tempMaterial.SetColor("_EmissionColor", Color.white * 100);
        //yield return new WaitForSeconds(0.1f);
        //_tempMaterial.SetColor("_EmissionColor", Color.white * 1);
        //_elapsedTime += Time.deltaTime;
        //yield return new WaitForSeconds(0.1f);


        _noise.m_AmplitudeGain = 0;



        _elapsedTime = 0;

        while (_elapsedTime < 0.4f)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        damagePause = false;
        allowRegen = true;
    }

    public void BaseAttack()
    {
        _animatorController.SetFloat("AttackBlend", (float)Math.Round(UnityEngine.Random.Range(0.6f, 3.4f), 0));
        _animatorController.SetTrigger("Attack");
    }

    public IEnumerator ZoomInOut()
    {
        zoomed = true;
        float zoomTime = 0;
        float cur = _camera.m_Lens.FieldOfView;

        while (zoomTime < 0.1f)
        {
            _camera.m_Lens.FieldOfView = Mathf.Lerp(cur, cur - 0.5f, zoomTime / 0.1f);
            zoomTime += Time.deltaTime;
            yield return null;
        }

        zoomTime = 0;
        cur = _camera.m_Lens.FieldOfView;
        while (zoomTime < 0.1f)
        {
            _camera.m_Lens.FieldOfView = Mathf.Lerp(cur, cur + 0.5f, zoomTime / 0.1f);
            zoomTime += Time.deltaTime;
            yield return null;
        }
        zoomed = false;
    }

    public IEnumerator ZoomIn()
    {
        float zoomTime = 0;
        float cur = _camera.m_Lens.FieldOfView;

        while (zoomTime < 1)
        {
            _camera.m_Lens.FieldOfView = Mathf.Lerp(cur, 44, zoomTime / 1);
            zoomTime += Time.deltaTime;
            yield return null;
        }
        zoomed = false;
    }

    public IEnumerator ZoomOut()
    {
        zoomed = true;
        float zoomTime = 0;
        float cur = _camera.m_Lens.FieldOfView;

        while (zoomTime < 2)
        {
            _camera.m_Lens.FieldOfView = Mathf.Lerp(cur, 45, zoomTime / 2);
            zoomTime += Time.deltaTime;
            yield return null;
        }
    }


    private IEnumerator LevelUp()
    {
        if (!maxLevel)
        {
            levelUpEffect.SetActive(true);
            AudioManager.Instance.OnLevelUp();
            _level++;
            _curXP = 0;
            _maxXP += _level * 25;
            levelUpChange.Invoke();

            WeaponsManager.Instance.CreateScreenArray(_level);

            yield return new WaitForSeconds(2f);
            levelUpEffect.SetActive(false);
        }
    }

    private void GetXP()
    {
        _curXP += 15 * _xpCollectMultiplier;

        AudioManager.Instance.OnTakeXP();
        xpChange.Invoke();
        
        if (_curXP >= _maxXP)
        {
            StartCoroutine(LevelUp());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("XP") || other.CompareTag("Coin") || other.CompareTag("Gem") || other.CompareTag("Potion"))
        {
            StartCoroutine(Magnit(other.gameObject));
        }
        else if (other.gameObject.CompareTag("Despawn"))
        {
            //_curHealth = 0;
            //deadFromFall = true;
            //StartCoroutine(Death());
        }
    }

    private IEnumerator Magnit(GameObject xp)
    {
        float elapsTime = 0;
        var pos = xp.transform.position;
        while (elapsTime < 0.5f)
        {
            xp.transform.position = Vector3.Lerp(pos, transform.position - new Vector3(0, 0.5f, 0), (elapsTime / 0.5f) * (elapsTime / 0.5f));
            elapsTime += Time.deltaTime;
            yield return null;
        }
        xp.transform.position = transform.position;

        switch (xp.tag)
        {
            case "XP":
                _getXPPS.Play();
                GetXP();
                XPSpawner.Instance.DespawnXP(xp);
                break;

            case "Coin":
                _getCoin.Play();
                coins += (int)(10 * _coinCollectMultiplier);
                balance.Invoke();
                AudioManager.Instance.OnTakeSpecial();
                xp.SetActive(false);
                break;

            case "Gem":
                _getGem.Play();
                gems += 1;
                balance.Invoke();
                xp.SetActive(false);
                AudioManager.Instance.OnTakeSpecial();
                break;

            case "Potion":
                _getPotion.Play();
                _curHealth = _curHealth + (_maxHealth * 0.5f) >= _maxHealth ? _maxHealth : _curHealth + (_maxHealth * 0.5f);
                healthChange.Invoke();
                AudioManager.Instance.OnTakeSpecial();
                xp.SetActive(false);
                break;
        }
    }

    public void MagnitAllXP()
    {
        foreach (var xp in XPSpawner.Instance.listXP)
        {
            StartCoroutine(Magnit(xp));
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("LevelCollider"))
        {
            _grounded = true;
            _animatorController.SetBool("Fly", false);
                }
    }

    private void OnCollisionExit(Collision other)
    {
        //if (other.gameObject.CompareTag("LevelCollider"))
        //{
        //    _grounded = false;
        //    StartCoroutine("FlyCheck");
        //}
    }

    private IEnumerator FlyCheck()
    {
        yield return new WaitForSeconds(2f);
        if (!_grounded)
        {
            _animatorController.SetBool("Fly", true);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("LevelCollider"))
        {
            _grounded = true;
            _animatorController.SetBool("Fly", false);
        }
    }

    public void ChangeMaxHealth(float change)
    {
        _maxHealth += change;
        healthChange.Invoke();
    }

    public void NewWave()
    {
    }

    public void Revive()
    {
        allowRegen = true;
        _curHealth = _maxHealth;
        _joystick.gameObject.SetActive(true);
        _moveVector = new Vector3(0, 0, 0);
        _rb.linearDamping= 1;
        _rb.mass = 5;
        _collider.isTrigger = false;
        AudioManager.Instance.StartBattle();
        _animatorController.SetTrigger("Revive");
        _animatorController.SetBool("Death", false);
        healthChange.Invoke();

        StartCoroutine(DamagePause());
        deadFromFall = false;
    }

    public void MaxLevel()
    {
        maxLevel = true;
        getMaxLevel.Invoke();
    }

    public void Vamp(float damage)
    {
        if (_curHealth < _maxHealth && _curHealth > 0)
        {
            _curHealth += damage * _vamp;
            healthChange.Invoke();
        }
    }

}





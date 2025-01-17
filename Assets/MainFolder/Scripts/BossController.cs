using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : EnemyController
{
    public static UnityEvent defeatBoss = new UnityEvent();
    private bool _doSkill;
    private float skillTime;

    [SerializeField] private GameObject _skill;

    public override void Start()
    {
        base.Start();
        _hpSlider.gameObject.SetActive(true);
        _hpSlider.value = _hpSlider.maxValue;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _hpSlider.value = _hpSlider.maxValue;
    }

    public override void UpdateLogic()
    {
        skillTime += Time.deltaTime;

        Debug.Log(skillTime);
        Debug.Log(canBeShoot);
        Debug.Log(_doSkill);
        if (skillTime>3 && canBeShoot && !_doSkill)
        {
            _doSkill = true;
            _animator.SetBool("Skill", true);
            _animator.ResetTrigger("GetHit");
            skillTime = 0;
            //StartSkill();
        }

        if (!_dead && !_fly && _target != null && !_doSkill)
        {
           // StopSkill();
          //  EnemyMove();
        }
    }

    public override void GetHit(float force, float upForce, float damage)
    {
        upForce = 0;
        force = 1;
        base.GetHit(force, upForce, damage);
    }

    public override void OnCollisionExit(Collision collider)
    {
        return;
    }

    public override void Clear()
    {
        base.Clear();
        defeatBoss.Invoke();
    }

    public override void OnTriggerEnter(Collider collider)
    {
        return;
    }

    public void StartSkill()
    {
        _skill.SetActive(true); 
    }

    public void StopSkill()
    {
        _animator.SetBool("Skill", false);
        _skill.SetActive(false);
        skillTime = 0;
        _doSkill = false;
    }

    public override void LoadDataFromSO()
    {
 
        maxSpeed = enemyData.maxSpeed * EnemySpawner.Instance.enemiesSpeedMultiplier;
        _moveSpeed = maxSpeed;
        _maxHealth = enemyData.maxHealth * EnemySpawner.Instance.enemiesHealthMultiplier;

        _damage = enemyData.damage * EnemySpawner.Instance.enemiesDamageMultiplier;
        //Destroy(_tempMaterialMain);
        //_tempMaterialMain = new Material(enemyData.mainMat);
        _meshObj.GetComponent<Renderer>().material = enemyData.mainMat;
        //_tempMaterialMain.SetColor("_EmissionColor", Color.white * 1);

        _curHealth = _maxHealth;

        _deathPS.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = enemyData.deathMat;

        _hpSlider.gameObject.SetActive(false);

        _damagePause = false;
        damageText.Stop();

        _meshObj.SetActive(true);
        _animator = GetComponentInChildren<Animator>();
    }
}

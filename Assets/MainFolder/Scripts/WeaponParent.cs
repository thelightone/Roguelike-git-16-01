using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class WeaponParent : MonoBehaviour
{
    public ActiveItemSO _activeItemSO;
    public float level;

    public EnemyController _enemy;
    public Transform _parent;

    public float _hitForce;
    public float _upForce;
    public float _damage;
    public AudioClip _attackAudio;

    public float elapsedTime;
    public float frequency;

    public HeroData heroData;
    public ShopData shopData;
    public UpgradePerLevel upgradesData;

    public float hitForceModif;
    public float weapHitForceUpModif;
    public float weapMultiplRotationModif;
    public float weapAttackSpeedModif;
    public float weapDamageModif;

    //SPECIFIC CHARACTERISTICS
    public float rotateSpeed;
    public float slowEffect;
    public float flySpeed;
    [HideInInspector]
    public PlayerMoveController player;
    [HideInInspector]
    public WeaponTypeParent typeParent;

    public virtual void Awake()
    {
        _parent = transform.parent;
        typeParent = GetComponentInParent<WeaponTypeParent>();
        player = PlayerMoveController.Instance;
        if (typeParent.isBaseWeapon)
        {
            AudioManager.Instance._baseAttack = _attackAudio;
            PlayerMoveController.Instance.weaponParent = this;
        }

        heroData = PlayerMoveController.Instance._heroData;
        shopData = PlayerMoveController.Instance._shopData;
        upgradesData = PlayerMoveController.Instance._upgradesData;

       InitSpecificParam(ref hitForceModif,heroData.weapHitForce,upgradesData.HitForce,shopData.weapHitForceLevel);
        weapHitForceUpModif = heroData.weapHitForceUp;
       InitSpecificParam(ref weapMultiplRotationModif,heroData.weapMultiplRotation,upgradesData.MultiplRotation, shopData.weapMultiplRotationLevel);
       InitSpecificParam(ref weapAttackSpeedModif, heroData.weapAttackSpeed ,upgradesData.AttackSpeed, shopData.weapAttackSpeedLevel);
       InitSpecificParam(ref weapDamageModif ,heroData.weapDamage ,upgradesData.Damage, shopData.weapDamageLevel);

        if (transform.parent.GetChild(0).GetComponent<WeaponParent>() == this)
        {
            level = 1;
        }
        if (transform.parent.GetChild(1).GetComponent<WeaponParent>() == this)
        {
            level = 2;
        }
        if (transform.parent.GetChild(2).GetComponent<WeaponParent>() == this)
        {
            level = 3;
        }

        InitChars((int)level - 1);

    }


    private void InitSpecificParam(ref float heroParam, float heroSOParam, float upgradePerLevel, float level)
    {
        if (level == 0)
        {
            heroParam = heroSOParam;
        }
        else if (level == 1)
        {
            heroParam = heroSOParam * upgradesData.basicModifier;
        }
        else
        {
            heroParam = heroSOParam * (upgradesData.basicModifier + (upgradePerLevel * (level-1)));
        }
    }

    private void InitChars(int level)
    {
        if (_activeItemSO != null)
        {
            _damage += _activeItemSO.activeItemData[level].damage;
            frequency += _activeItemSO.activeItemData[level].frequency;
            slowEffect += _activeItemSO.activeItemData[level].slowEffect;
            rotateSpeed += _activeItemSO.activeItemData[level].rotationSpeed;
            flySpeed += _activeItemSO.activeItemData[level].flySpeed;
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

    public virtual void UpdateLogic()
    {

    }

    public virtual void Shoot()
    {
        if (typeParent.isBaseWeapon && EnemySpawner.Instance.curEnemy > 0)
        {
            player.BaseAttack();
            AudioManager.Instance.BaseAttack();
        }
    }

    public virtual void OnTriggerEnter(Collider collision)
    {

        if ((collision.TryGetComponent(out EnemyController enemy)))
        {
            enemy.GetHit(
                _hitForce * Modifiers.multiplHitForce * hitForceModif,
                _upForce * Modifiers.multiplHitForceUp * weapHitForceUpModif,
                _damage * Modifiers.multiplDamage * weapDamageModif);
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.TryGetComponent(out EnemyController enemy)))
        {
            enemy.GetHit(
                _hitForce * Modifiers.multiplHitForce * hitForceModif,
                _upForce * Modifiers.multiplHitForceUp * weapHitForceUpModif,
                _damage * Modifiers.multiplDamage * weapDamageModif);
        }
    }
}

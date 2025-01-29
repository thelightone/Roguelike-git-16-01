using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class ChakrumWeapon: WeaponParent
{
    private List<GameObject> _enemys = new List<GameObject>();
    public List<BulletWeapon> _bullets = new List<BulletWeapon>();
    public List <EnemyController> _busyEnemies = new List<EnemyController>();

    public float reloadTime;
    public ChakrumBulletWeapon weapon;

    public override void Awake()
    {
        base.Awake();
        weapon = GetComponentInChildren<ChakrumBulletWeapon>();
    }

    public override void UpdateLogic()
    {
        transform.position = PlayerMoveController.Instance.transform.position;
        elapsedTime += Time.deltaTime;
        reloadTime = frequency* Modifiers.multiplAttackSpeed / weapAttackSpeedModif;

        

        if (elapsedTime > reloadTime)
        {
            Shoot();
            elapsedTime = 0;
        }         
    }

    public override void Shoot()
    {

        weapon.Shoot();



    }
}

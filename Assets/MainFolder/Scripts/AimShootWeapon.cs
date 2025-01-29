using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class AimShootWeapon : WeaponParent
{
    private List<GameObject> _enemys = new List<GameObject>();
    public List<BulletWeapon> _bullets = new List<BulletWeapon>();
    public List <EnemyController> _busyEnemies = new List<EnemyController>();


    public float reloadTime;

    public override void Awake()
    {
        base.Awake();
    }

    public override void UpdateLogic()
    {
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
        _busyEnemies.Clear();

        if (EnemySpawner.Instance.canBeShootList.Count > 0)
        {
            base.Shoot();

            foreach (BulletWeapon weapon in _bullets)
            {
                weapon.Shoot();
            }
        }
    }


}

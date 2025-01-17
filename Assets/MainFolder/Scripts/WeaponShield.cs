using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponShield : WeaponParent
{
    private Collider _mainCollider;

    public ParticleSystem _particle;


    public override void Awake()
    {
        base.Awake();
        _mainCollider = GetComponent<Collider>();
        _mainCollider.enabled = false;
        _particle.gameObject.SetActive(false);
    }

    public override void UpdateLogic()
    {
         if (elapsedTime > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif && _mainCollider.enabled ==false)
        {
            elapsedTime = 0;
            Shoot();
            player.damagePause = true;

        }

        elapsedTime += Time.deltaTime;
    }

    public override void Shoot()
    {
        _mainCollider.enabled = true;
        _particle.gameObject.SetActive(true);
        StartCoroutine(Stop());
    
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(flySpeed);

        _mainCollider.enabled = false;
        _particle.gameObject.SetActive(false);
        player.damagePause = false;
    }
}

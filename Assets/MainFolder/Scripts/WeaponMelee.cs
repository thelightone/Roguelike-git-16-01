using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponMelee : WeaponParent
{
    private float _activeTime = 0;
    private Collider _mainCollider;

    private List<ParticleSystem> _particles = new List<ParticleSystem>();


    public override void Awake()
    {
        base.Awake();
        _mainCollider = GetComponent<Collider>();
        _mainCollider.enabled = false;

        var children = transform.childCount;
        for (var i = 0; i < children; i++)
        {
            _particles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public override void UpdateLogic()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime-0.2f > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif)
        {

            _mainCollider.enabled = false;
            elapsedTime = 0;
        }
        else if (elapsedTime > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif && _mainCollider.enabled ==false)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        _mainCollider.enabled = true;
        base.Shoot();

        transform.GetChild(0).gameObject.SetActive(true);

        //foreach (var particle in _particles)
        //{
        //    particle.Play();

        //}

        StartCoroutine(Stop());
    
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.4f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponAxe : WeaponParent
{
    private float _activeTime = 0;
    private Collider _mainCollider;
    public GameObject axe;
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    public GameObject son;
    public override void Awake()
    {
        base.Awake();

        var children = transform.childCount;
        for (var i = 0; i < children; i++)
        {
           // _particles.Add(transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>());
        }

        son.SetActive(false);

        frequency = 6;

    }

    public override void UpdateLogic()
    {
        elapsedTime += Time.deltaTime;

 if (elapsedTime > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif )
        {
            Debug.Log("SHOOT!");
            elapsedTime = 0;
            Shoot();
        }
    }

    public override void Shoot()
    {
        StopAllCoroutines();
        son.transform.position = transform.position; 
        son.SetActive(true);
        //_mainCollider.enabled = true;




        //foreach (var particle in _particles)
        //{
        //    particle.Play();

        //}


        StartCoroutine(Stop());

    }

    private IEnumerator Stop()
    {
        //Debug.Log("sTARPrpAUESE!");
        yield return new WaitForSeconds(2);
        //transform.GetChild(1).gameObject.SetActive(false);
        //_mainCollider.enabled = false;
        //axe.SetActive(false);
        //Debug.Log("STIOrpAUESE!");

         son.SetActive(false);
    }

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }

}

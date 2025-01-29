using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations;

public class WeaponAOE : WeaponParent
{
    private float _activeTime = 0;
    private Collider _mainCollider;
    public float effectDuration;
    public int numStrikes;


    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private bool effectInProgress;


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

        GetComponentInParent<ParentConstraint>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void UpdateLogic()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime-0.2f > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif)
        {
            _mainCollider.enabled = false;
            elapsedTime = 0;
        }
        else if (elapsedTime > frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif && _mainCollider.enabled ==false && !effectInProgress)
        {
            effectInProgress = true;
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        var x = Random.Range(0, 2); 
        var z = Random.Range(0, 2);

        x = x==0? Random.Range(1, 10) : Random.Range(-1, -10);
        z = z == 0 ? Random.Range(1, 10) : Random.Range(-1, -10);

        transform.position = new Vector3 (player.transform.position.x + x, transform.position.y, player.transform.position.z + z);
        Debug.Log(transform.position);
        transform.GetChild(0).gameObject.SetActive(true);
        //foreach (var particle in _particles)
        //{
        //    particle.Play();

        //}

        StartCoroutine(Effect());
    
    }

    private IEnumerator Effect()
    {
        int strikes = 0;
        float pauseTime = (effectDuration * 0.75f) / (numStrikes*2);
        while (strikes<numStrikes)
        {
            yield return new WaitForSeconds(pauseTime);
            _mainCollider.enabled = true;
            yield return new WaitForSeconds(pauseTime);
            _mainCollider.enabled = false;
            strikes++;
        }
        yield return new WaitForSeconds(effectDuration * 0.25f);
        transform.GetChild(0).gameObject.SetActive(false);
        effectInProgress = false;
    }
}

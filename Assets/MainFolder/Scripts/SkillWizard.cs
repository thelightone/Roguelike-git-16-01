using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWizard : SkillParent 
{
    public float _hitForce;
    public float _upForce;
    public float _damage;
    public Collider _collider;
    public float prePause;


    public override IEnumerator SkillEffect()
    {
        var elapsedTime = prePause;

        while (elapsedTime > 0.01f)
        {
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        _collider.enabled = true;

        AudioManager.Instance.OnSkill();

        elapsedTime = 0.1f;

        while (elapsedTime > 0.01f)
        {
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        _collider.enabled = false;
    }

    public override void OnTriggerEnter(Collider collision)
    {

        if ((collision.TryGetComponent<EnemyController>(out EnemyController enemy)))
        {
            enemy.GetHit(_hitForce * Modifiers.multiplHitForce, _upForce * Modifiers.multiplHitForceUp, _damage * Modifiers.multiplDamage);
        }
    }

    public override void Action()
    {
        StartCoroutine(SkillEffect());
    }
}

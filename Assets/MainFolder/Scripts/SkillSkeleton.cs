using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSkeleton : SkillParent 
{
    public float duration;
    public float _damage;
    public Collider _collider;

    public override IEnumerator SkillEffect()
    {


        AudioManager.Instance.OnSkill();

        float elapsedTime = 0;
        _collider.enabled = true;

        while (elapsedTime < duration)
        {
            PlayerMoveController.Instance._curHealth+=(float)0.1;
            elapsedTime += Time.deltaTime;

            if(elapsedTime%1>0.5)
            {
                _collider.enabled = false;
            }
            else
            {
                _collider.enabled = true;
            }
            yield return null;
        }
        _collider.enabled = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
 
    }

    public override void OnTriggerEnter(Collider collision)
    {

        if ((collision.TryGetComponent<EnemyController>(out EnemyController enemy)))
        {
            enemy.GetHit(Modifiers.multiplHitForce, Modifiers.multiplHitForceUp, _damage * Modifiers.multiplDamage);
        }
    }

    public override void Action()
    {
        StartCoroutine(SkillEffect());
    }
}

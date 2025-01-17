using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAssasin : SkillParent 
{
    public float duration;


    public override IEnumerator SkillEffect()
    {


        AudioManager.Instance.OnSkill();

        float elapsedTime = 0;

        PlayerMoveController.Instance._moveSpeed += 6;
        PlayerMoveController.Instance.damagePause = true;

        while (elapsedTime < duration)
        {
            PlayerMoveController.Instance._curHealth+=(float)0.2;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

        PlayerMoveController.Instance._moveSpeed -= 6;
        PlayerMoveController.Instance.damagePause = false;
    }



    public override void Action()
    {
        StartCoroutine(SkillEffect());
    }
}

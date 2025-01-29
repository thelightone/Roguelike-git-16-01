using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarb : SkillParent 
{
    public float duration;


    public override IEnumerator SkillEffect()
    {


        AudioManager.Instance.OnSkill();

        float elapsedTime = 0;

        PlayerMoveController.Instance.transform.localScale *= 1.5f;
        var baseWeapon = PlayerMoveController.Instance.weaponParent;

        baseWeapon.frequency /= 3;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

        PlayerMoveController.Instance.transform.localScale /= 1.5f;

        var newBaseWeapon = PlayerMoveController.Instance.weaponParent;
        if (newBaseWeapon == baseWeapon)
        {
            baseWeapon.frequency *= 3;
        }
    }



    public override void Action()
    {
        StartCoroutine(SkillEffect());
    }
}

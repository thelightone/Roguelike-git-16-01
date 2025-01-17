using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParent : MonoBehaviour
{
    public virtual IEnumerator SkillEffect()
    {
        yield return null;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {

    }

    public virtual void Action()
    {
        StartCoroutine(SkillEffect());
    }
}

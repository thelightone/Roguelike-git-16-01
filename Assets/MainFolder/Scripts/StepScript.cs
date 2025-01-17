using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    public void Step()
    {
        AudioManager.Instance.OnStep();
        _effect.Play();
    }
}


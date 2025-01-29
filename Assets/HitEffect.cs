using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public static HitEffect Instance;
    public ParticleSystem particleSystem;

    private void Start()
    {
        Instance = this;
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void Play(Transform aim)
    {
        transform.position = aim.position;
        particleSystem.Play();
    }
}

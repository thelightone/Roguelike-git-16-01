using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public static DeathEffect Instance;
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

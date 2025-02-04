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

    public void PlayBoss()
    {
        particleSystem.transform.localScale = new Vector3(4, 4, 4);
    }

    public void Reset()
    {
        particleSystem.transform.localScale = new Vector3(1, 1, 1);
    }
}

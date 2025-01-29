using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Spawner") && !EnemySpawner.Instance._points.Contains(this))
        {
            Debug.Log(2);
            //EnemySpawner.Instance._points.Add(this);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Spawner") && EnemySpawner.Instance._points.Contains(this))
        {
           // EnemySpawner.Instance._points.Remove(this);
        }
    }
}

using UnityEngine;

public class InTerrain : MonoBehaviour
{
    public GameObject thisTerrain;
    public TerrainsManager terrainsManager;

    private void Start()
    {
        terrainsManager = GetComponentInParent<TerrainsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            terrainsManager.OnCenterEnter(thisTerrain);
        }
    }
}

using UnityEngine;

public class OnSide : MonoBehaviour
{
    public TerrainsManager terrainsManager;
    public GameObject thisTerrain;
    public int side;
    private void Start()
    {
        terrainsManager = GetComponentInParent<TerrainsManager>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // terrainsManager.TouchSide(side);
        }
    }
}

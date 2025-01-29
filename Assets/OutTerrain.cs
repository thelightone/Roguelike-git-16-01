using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class OutTerrain : MonoBehaviour
{
    public TerrainsManager terrainsManager;
    public GameObject thisTerrain;
    public int side;

    public List<GameObject> oppositeTerrains = new List<GameObject>();

    private void Start()
    {
       terrainsManager = GetComponentInParent<TerrainsManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            terrainsManager.OnCenterExit(side,thisTerrain.transform);
        }
    }
}

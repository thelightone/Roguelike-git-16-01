using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();   
    }

    private void OnTriggerEnter(Collider collision)
    {
        collider.isTrigger = false;
    }
}

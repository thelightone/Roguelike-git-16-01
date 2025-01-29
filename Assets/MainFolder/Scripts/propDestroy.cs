using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestroy : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Despawn"))
        {
            gameObject.SetActive(false);
        }
    }
}

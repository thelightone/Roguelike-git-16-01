using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    public WeaponParent wp;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        wp.OnTriggerEnter(other);
    }

    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Collision");
        wp.OnCollisionEnter(collision);
    }
}

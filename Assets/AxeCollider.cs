using UnityEngine;

public class AxeCollider : MonoBehaviour
{
    public WeaponParent weaponParent;

    private void Start()
    {
        weaponParent = GetComponentInParent<WeaponParent>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collider>(out var other2)) 
        weaponParent.OnTriggerEnter(other2);
    }
}

using UnityEngine;

public class WeaponRotateSwords : WeaponParent
{

    public override void UpdateLogic()
    {
        Rotate();
    }

    private void Rotate()
    {
        _parent.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rotateSpeed*Modifiers.multiplRotation *weapMultiplRotationModif, transform.rotation.eulerAngles.z);
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if ((collision.TryGetComponent(out EnemyController enemy)))
        {
            enemy.GetHit(_damage * Modifiers.multiplDamage * weapDamageModif);
        }
    }
}
using UnityEngine;

public class WeaponRotate : WeaponParent
{
    

    public override void UpdateLogic()
    {
        Rotate();
    }

    private void Rotate()
    {
        _parent.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rotateSpeed*Modifiers.multiplRotation *weapMultiplRotationModif, transform.rotation.eulerAngles.z);
    }
}
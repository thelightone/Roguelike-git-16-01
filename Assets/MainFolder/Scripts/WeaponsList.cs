using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponsList", menuName = "Weapons List", order = 51)]
public class WeaponsList : ScriptableObject
{
    public List<WeaponTypeParent> allWeapons;
    public List<WeaponTypeParent> availableWeapons;

}

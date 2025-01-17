using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PassiveItemData", menuName = "Custom/PassiveItemData", order = 51)]
[System.Serializable]
public class PassiveItemSO : ScriptableObject
{
    [SerializeField] public List<PassiveChar> passiveItemData = new List<PassiveChar>();
}
[System.Serializable]
public class PassiveChar
{
    public float addedHealth = 0;
    public float addedRegenerate = 0;
    public float multiplDamage = 0;
    public float multiplSpeed = 0;
    public float multiplHitForce = 0;
    public float multiplHitForceUp = 0;
    public float multiplRotation = 0;
    public float addedDefence = 0.0f;
    public float multiplyAttackSpeed = 0.0f;
    public float addedVamp = 0.0f;
    public float addedCrit = 0.0f;
}

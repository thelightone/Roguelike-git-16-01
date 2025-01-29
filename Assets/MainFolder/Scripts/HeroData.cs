using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HeroData", menuName = "Custom/HeroData")]

public class HeroData:ScriptableObject
{
    public float heroMaxHealth = 100; //number
    public float heroRegenerate = 0; //number
    public float heroSpeed = 1; //number
    public float heroDefence = 0.1f; //number, decimal
    public float heroMultiplyCoinCollect = 1; //multiplier
    public float heroMultiplyXpReceived = 1; //multiplier

    public float weapHitForce = 1; //multiplier
    public float weapHitForceUp = 1; //multiplier
    public float weapMultiplRotation = 1; //multiplier
    public float weapAttackSpeed = 1; //multiplier
    public float weapDamage = 1; //multiplier    
}

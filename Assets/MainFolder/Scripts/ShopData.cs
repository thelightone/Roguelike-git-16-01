using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopData", menuName = "Custom/ShopData")]

public class ShopData : ScriptableObject
{
    public int heroMaxHealthLevel; 
    public int heroRegenerateLevel; 
    public int heroSpeedLevel; 
    public int heroDefenceLevel; 
    public int heroMultiplyCoinCollectLevel; 
    public int heroMultiplyXpReceivedLevel; 

    public int weapHitForceLevel; 
    public int weapMultiplRotationLevel; 
    public int weapAttackSpeedLevel; 
    public int weapDamageLevel;

    public int priceModif;
}

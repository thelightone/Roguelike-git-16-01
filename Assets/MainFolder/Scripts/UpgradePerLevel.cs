using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "Custom/UpgradesData")]

[System.Serializable]
public class UpgradePerLevel:ScriptableObject
    {
        public float MaxHealth;
        public float Regenerate;
        public float Speed;
        public float Defence;
        public float MultiplyCoinCollect;
        public float MultiplyXpReceived;

        public float HitForce;
        public float MultiplRotation;
        public float AttackSpeed;
        public float Damage;

        public float basicModifier;
    }



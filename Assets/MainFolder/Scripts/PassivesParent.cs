using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivesParent : WeaponParent
{
    public PassiveItemSO passivesItemSO;
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

    public override void Awake()
    {
        base.Awake();

        if (transform.parent.GetChild(0).GetComponent<PassivesParent>() == this)
        {
            level = 1;
        }
        if (transform.parent.GetChild(1).GetComponent<PassivesParent>() == this)
        {
            level = 2;
        }
        if (transform.parent.GetChild(2).GetComponent<PassivesParent>() == this)
        {
            level = 3;
        }

        InitChars((int)level - 1);

    }

    private void InitChars(int level)
    {
        addedHealth += passivesItemSO.passiveItemData[level].addedHealth;
        addedRegenerate += passivesItemSO.passiveItemData[level].addedRegenerate;
        multiplDamage += passivesItemSO.passiveItemData[level].multiplDamage;
        multiplSpeed += passivesItemSO.passiveItemData[level].multiplSpeed;
        multiplHitForce += passivesItemSO.passiveItemData[level].multiplHitForce;
        multiplRotation += passivesItemSO.passiveItemData[level].multiplRotation;
        addedDefence += passivesItemSO.passiveItemData[level].addedDefence;
        multiplyAttackSpeed += passivesItemSO.passiveItemData[level].multiplyAttackSpeed;
        addedVamp += passivesItemSO.passiveItemData[level].addedVamp;
        addedCrit += passivesItemSO.passiveItemData[level].addedCrit;
    }

    public override void Shoot()
    {
        return;
    }

    public override void OnTriggerEnter(Collider collision)
    {
        return;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        return;
    }

    private void OnEnable()
    {
        player = PlayerMoveController.Instance;
        ChangeParams();
    }

    private void ChangeParams()
    {
        player.ChangeMaxHealth(addedHealth);
        player._regeneration += addedRegenerate;
        player._moveSpeed += player._moveSpeed * multiplSpeed / 100;
        player._defence += addedDefence / 100;
        player._vamp += addedVamp / 100;
        player._critChance += addedCrit / 100;

        Modifiers.multiplDamage += multiplDamage / 100;
        Modifiers.multiplHitForce += multiplHitForce / 100;
        Modifiers.multiplHitForceUp += multiplHitForceUp / 100;
        Modifiers.multiplRotation += multiplRotation / 100;
        Modifiers.multiplAttackSpeed -= multiplyAttackSpeed / 100;

        //foreach (var weapon in WeaponsManager.Instance._copiesWeapons)
        //{
        //    weapon.transform.GetChild(0).GetComponent<WeaponParent>()._frequency /= 1 + multiplyAttackSpeed / 100;
        //    weapon.transform.GetChild(1).GetComponent<WeaponParent>()._frequency /= 1 + multiplyAttackSpeed / 100;
        //    weapon.transform.GetChild(2).GetComponent<WeaponParent>()._frequency /= 1 + multiplyAttackSpeed / 100;
        //}

    }
}

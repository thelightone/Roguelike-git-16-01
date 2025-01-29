using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private EnergyRecoverData _energyRecoverData;
    private MenuManager _menuManager;

    private void Start()
    {
        _menuManager = GetComponent<MenuManager>();
        InvokeRepeating("CheckEnergy", 0, 1);
    }

    private void CheckEnergy()
    {
        var ticks = new DateTime(_energyRecoverData.energySpentTime);
        float elapsedTime = (float)(DateTime.Now - ticks).TotalSeconds;
        if (elapsedTime > 60) 
        {
            var addedEnergy = Convert.ToInt32(elapsedTime / 60);
            PrefsManager.ChangeEnergy(addedEnergy);
            _energyRecoverData.energySpentTime = DateTime.Now.Ticks;
            _menuManager.UpdateBalances();
        }
    }

    public void SpendEnergy()
    {
        _energyRecoverData.energySpentTime = DateTime.Now.Ticks;
        PrefsManager.ChangeEnergy(-5);
    }
}

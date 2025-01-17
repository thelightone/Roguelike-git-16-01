using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstGemsAdd : MonoBehaviour
{
    [SerializeField] private GameObject addGemsScreen;

    public bool SpendGems(int price)
    {
        if (PrefsManager.GetGems() >= price)
        {
            PrefsManager.ChangeGems(-price);
            return true;
        }
        else
        {
            addGemsScreen.SetActive(true);
            return false;
        }

    }

}

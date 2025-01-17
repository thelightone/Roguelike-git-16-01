
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class RewardsManager : MonoBehaviour
{
    public GameObject rewardScreen;
    public List<RewardCell> rewardCell = new List<RewardCell>();
    public bool shownToday;
    public Button claim;
    public DateTime lastReward;
    int days;

    
    private void Start()
    {
  
    }

    private void OnEnable()
    {
        string tempTime = PlayerPrefs.GetString("rewardDate", "0");

        if (tempTime != "0")
        {
            lastReward = DateTime.Parse(tempTime);
        }

        DateTime now = DateTime.Now;
        if ((now - lastReward).Days> 0)
        {
            shownToday = false;
        }

        else
        {
            if (PlayerPrefs.GetInt("streak", 0) > 0)
            {
                shownToday = true;
            }
            else
            {
                shownToday = false;
            }
        }

        if (!shownToday)
        {
           days = PlayerPrefs.GetInt("streak", 0);
           
            rewardScreen.SetActive(true);

            if(days>5)
            {
                days = 0;
            }

            Clear();

            for (var i = 0; i < days; i++)
            {
                rewardCell[i].Choosed();
            }
            rewardCell[(int)days].ActiveCell();

            shownToday = true;

            PlayerPrefs.SetString("rewardDate", DateTime.Now.ToString());
        }
    }

   

    public void Claim()
    {
        var cell = rewardCell[days];

        if(cell.priceType == PriceType.Gems)
        {
            PlayerPrefs.SetInt("gems", PlayerPrefs.GetInt("gems", 0) + cell.rewardNum);
        }
        else
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + cell.rewardNum);
        }

        rewardScreen.SetActive(false);
        GetComponent<MenuManager>().UpdateBalances();

        days++;
        PlayerPrefs.SetInt("streak", days);
    }
    public void Clear()
    {
        foreach (var r in rewardCell)
        {
            r.UnChoosed();
            r.DisActiveCell();
        }


    }


}

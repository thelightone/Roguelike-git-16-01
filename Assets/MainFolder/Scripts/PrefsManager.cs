using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsManager
{
    public const string coins = "coins";
    public const string gems = "gems";
    public const string energy = "energy";

    public const string charter = "charter";
    public const string charter1UnlockedLevel = "charter1UnlockedLevel";
    public const string charter1ChosenLevel = "charter1ChosenLevel";
    public const string charter2UnlockedLevel = "charter2UnlockedLevel";
    public const string charter2ChosenLevel = "charter2ChosenLevel";
    public const string soundOn = "soundOn";
    public const string firstPlay = "firstPlay";
    public const string playerLevel = "playerLevel";
    public const string playerXP = "playerXP";

    public const string regDate = "regDate";


    public static void Init(GameData gameData)
    {
        if (!PlayerPrefs.HasKey(coins))
        {
            PlayerPrefs.SetInt(energy, 50);
            PlayerPrefs.SetInt(soundOn, 1);
        }

        PlayerPrefs.SetInt(coins, gameData.coins);
        PlayerPrefs.SetInt(gems, gameData.gems);
        PlayerPrefs.SetInt(charter, gameData.charter);
        PlayerPrefs.SetInt(charter1UnlockedLevel, gameData.charter1UnlockedLevel);
        PlayerPrefs.SetInt(charter1ChosenLevel, gameData.charter1UnlockedLevel);
        PlayerPrefs.SetInt(charter2UnlockedLevel, gameData.charter2UnlockedLevel);
        PlayerPrefs.SetInt(charter2ChosenLevel, gameData.charter2UnlockedLevel);
        // 0 - regular game, 1 - first launch, need start game, 2 - after first launch, need show tutor in menu
        PlayerPrefs.SetInt(firstPlay, gameData.firstPlay);
        PlayerPrefs.SetInt(firstPlay, gameData.firstPlay);
        PlayerPrefs.SetString(regDate, gameData.regDate);

    }

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(coins,0);
    }

    public static int GetFirstPlay()
    {
        return PlayerPrefs.GetInt(firstPlay);
    }

    public static int GetGems()
    {
        return PlayerPrefs.GetInt(gems,0);
    }

    public static int GetPlayerLevel()
    {
        return PlayerPrefs.GetInt(playerLevel);
    }

    public static int GetPlayerXP()
    {
        return PlayerPrefs.GetInt(playerXP);
    }

    public static int GetEnergy()
    {
        return PlayerPrefs.GetInt(energy);
    }

    public static int GetCharter()
    {
        return PlayerPrefs.GetInt(charter);
    }

    public static int GetUnlockedLevelCharter1()
    {
        return PlayerPrefs.GetInt(charter1UnlockedLevel);
    }

    public static int GetUnlockedLevelCharter2()
    {
        return PlayerPrefs.GetInt(charter2UnlockedLevel);
    }


    public static int GetChosenLevelCharter1()
    {
        return PlayerPrefs.GetInt(charter1ChosenLevel);
    }
    public static int GetChosenLevelCharter2()
    {
        return PlayerPrefs.GetInt(charter2ChosenLevel);
    }

    public static int GetSound()
    {
        return PlayerPrefs.GetInt(soundOn);
    }

    public static DateTime GetRegDate()
    {
        return DateTime.Parse(PlayerPrefs.GetString(regDate));
    }

    public static int DaysFromReg()
    {
        return (GetRegDate() - DateTime.UtcNow).Days;
    }

    public static void ChangeCoins(int change)
    {
        var newValue = PlayerPrefs.GetInt(coins) + change >= 0 ? PlayerPrefs.GetInt(coins) + change : 0;
        PlayerPrefs.SetInt(coins, newValue);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.coins, newValue);
    }

    public static void ChangePlayerLevelIncrease()
    {
        var newValue = PlayerPrefs.GetInt(playerLevel) + 1;
        PlayerPrefs.SetInt(playerLevel, newValue);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.playerLevel, newValue);
    }

    public static void ChangePlayerXP(int increase)
    {   
        var updatedXP = PlayerPrefs.GetInt(playerXP)+increase;

        if (updatedXP>GetPlayerLevel()*100) 
        {
            increase = (PlayerPrefs.GetInt(playerXP) + increase) - (GetPlayerLevel() * 100);
            ChangePlayerLevelIncrease();
        }

        PlayerPrefs.SetInt(playerXP, increase);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.playerXP, increase);
    }

    public static void ChangeGems(int change)
    {
        var newValue = PlayerPrefs.GetInt(gems) + change >= 0 ? PlayerPrefs.GetInt(gems) + change : 0;
        PlayerPrefs.SetInt(gems, newValue);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.gems, newValue);
    }

    public static void ChangeEnergy(int change)
    {
        var newValue = PlayerPrefs.GetInt(energy) + change >= 0 ? PlayerPrefs.GetInt(energy) + change : 0;
        newValue = newValue > 50 ? 50 : newValue;
        PlayerPrefs.SetInt(energy, newValue);
    }

    public static void ChangeUnlockedLevel1(int change)
    {
        var newValue = PlayerPrefs.GetInt(charter1UnlockedLevel) + change >= 4 ?4: PlayerPrefs.GetInt(charter1UnlockedLevel) + change;
        PlayerPrefs.SetInt(charter1UnlockedLevel, newValue);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.charter1UnlockedLevel, newValue);
    }

    public static void ChangeUnlockedLevel2(int change)
    {
        var newValue = PlayerPrefs.GetInt(charter2UnlockedLevel) + change >= 4 ? 4 : PlayerPrefs.GetInt(charter2UnlockedLevel) + change;
        PlayerPrefs.SetInt(charter1UnlockedLevel, newValue);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.charter2UnlockedLevel, newValue);
    }

    public static void ChangeChosenLevel1(int change)
    {
        var newValue = PlayerPrefs.GetInt(charter1UnlockedLevel) > change ? change : PlayerPrefs.GetInt(charter1UnlockedLevel);
        PlayerPrefs.SetInt(charter1ChosenLevel, newValue);
    }

    public static void ChangeChosenLevel2(int change)
    {
        var newValue = PlayerPrefs.GetInt(charter2UnlockedLevel) > change ? change : PlayerPrefs.GetInt(charter2UnlockedLevel);
        PlayerPrefs.SetInt(charter2ChosenLevel, newValue);
    }

    public static void ChangeCharter(int change)
    {
        PlayerPrefs.SetInt(charter, PlayerPrefs.GetInt(charter) + change);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.charter, PlayerPrefs.GetInt(charter) + change);
    }

    public static void ChangeFirstPlay(int change)
    {
        PlayerPrefs.SetInt(firstPlay, change);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.firstPlay, change);
    }

    public static void ChangeSound(int change)
    {
        PlayerPrefs.SetInt(soundOn, change);
    }
}

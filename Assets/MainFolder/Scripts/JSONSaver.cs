using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using UnityEngine;

public static class JSONSaver
{
    private static string fileName = "data.json";
    private static string filePath;
    public static GameData gameData;

    public enum DataTypes
    {
        coins,
        gems,
        charter,
        charter1UnlockedLevel,
        charter2UnlockedLevel,
        firstPlay,
        playerLevel,
        playerXP,

        heroMaxHealthLevel,
        heroRegenerateLevel,
        heroSpeedLevel,
        heroDefenceLevel,
        heroMultiplyCoinCollectLevel,
        heroMultiplyXpReceivedLevel,
        weapHitForceLevel,
        weapMultiplRotationLevel,
        weapAttackSpeedLevel,
        weapDamageLevel,

        regDate
    }

    public static void CheckFile()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            LoadJsonFromFile();
        }
        else
        {
            CreateJsonFile();
        }

        PrefsManager.Init(gameData);

    }

  public  static GameData LoadJsonFromFile()
    {
        try
        {
            if (filePath != null)
            {
                string jsonData = File.ReadAllText(filePath);
                gameData = JsonUtility.FromJson<GameData>(jsonData);
                return gameData;
                // Deserialize JSON data if necessary
                // Example: YourDataClass data = JsonUtility.FromJson&lt;YourDataClass&gt;(jsonData);
            }else
            {
                return null;
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading file: " + e.Message);
            return null;
        }
    }

    static void CreateJsonFile()
    {
        try
        {
            // Define default data
            gameData = new GameData
            {
                coins = 1000,
                gems = 50,
                charter = 0,
                charter1UnlockedLevel = 0,
                charter2UnlockedLevel = 0,

                firstPlay = 1,
                playerLevel = 1,
                playerXP = 0,

                heroMaxHealthLevel = 0,
                heroRegenerateLevel = 0,
                heroSpeedLevel = 0,
                heroDefenceLevel = 0,
                heroMultiplyCoinCollectLevel = 0,
                heroMultiplyXpReceivedLevel = 0,
                weapHitForceLevel = 0,
                weapMultiplRotationLevel = 0,
                weapAttackSpeedLevel = 0,
                weapDamageLevel = 0,
                regDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // Serialize data to JSON
            string jsonData = JsonUtility.ToJson(gameData, true);

            // Write JSON data to file
            File.WriteAllText(filePath, jsonData);

        }
        catch (IOException e)
        {
            Debug.LogError("Error creating file: " + e.Message);
        }
    }

    public static void UpdateJsonFile(DataTypes dataType, int newValue)
    {
        GameData gameDataToSave = LoadJsonFromFile();

        if (newValue == null) newValue = 0;

        switch (dataType)
        {
            case DataTypes.coins:
                gameDataToSave.coins = newValue;
                break;
            case DataTypes.gems:
                gameDataToSave.gems = newValue;
                break;
            case DataTypes.charter:
                gameDataToSave.charter = newValue;
                break;
            case DataTypes.charter1UnlockedLevel:
                gameDataToSave.charter1UnlockedLevel = newValue;
                break;
            case DataTypes.charter2UnlockedLevel:
                gameDataToSave.charter2UnlockedLevel = newValue;
                break;
            case DataTypes.firstPlay:
                gameDataToSave.firstPlay = newValue;
                break;
            case DataTypes.heroMaxHealthLevel:
                gameDataToSave.heroMaxHealthLevel = newValue;
                break;
            case DataTypes.heroRegenerateLevel:
                gameDataToSave.heroRegenerateLevel = newValue;
                break;
            case DataTypes.heroSpeedLevel:
                gameDataToSave.heroSpeedLevel = newValue;
                break;
            case DataTypes.heroDefenceLevel:
                gameDataToSave.heroDefenceLevel = newValue;
                break;
            case DataTypes.heroMultiplyCoinCollectLevel:
                gameDataToSave.heroMultiplyCoinCollectLevel = newValue;
                break;
            case DataTypes.heroMultiplyXpReceivedLevel:
                gameDataToSave.heroMultiplyXpReceivedLevel = newValue;
                break;
            case DataTypes.weapHitForceLevel:
                gameDataToSave.weapHitForceLevel = newValue;
                break;
            case DataTypes.weapMultiplRotationLevel:
                gameDataToSave.weapMultiplRotationLevel = newValue;
                break;
            case DataTypes.weapAttackSpeedLevel:
                gameDataToSave.weapAttackSpeedLevel = newValue;
                break;
            case DataTypes.weapDamageLevel:
                gameDataToSave.weapDamageLevel = newValue;
                break;
            case DataTypes.playerLevel:
                gameDataToSave.playerLevel = newValue;
                break;
            case DataTypes.playerXP:
                gameDataToSave.playerXP = newValue;
                break;
        }

        try
        {
            // Serialize data to JSON
            string jsonData = JsonUtility.ToJson(gameDataToSave, true);

            // Write JSON data to file
            File.WriteAllText(filePath, jsonData);

        }
        catch (IOException e)
        {
            Debug.LogError("Error creating file: " + e.Message);
        }
    }
}

// Example data class to be serialized to JSON
[System.Serializable]
public class GameData
{
    public int coins;
    public int gems;
    public int charter;
    public int charter1UnlockedLevel;
    public int charter2UnlockedLevel;
    public int firstPlay;
    public int playerLevel;
    public int playerXP;

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

    public string regDate;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppMetricaEventsTypes
{
    public static string level_start = "level_start";
    public static string level_complete = "level_complete";
    public static string level_fail = "level_fail";

    public static string wave_start = "wave_start";
    public static string wave_complete = "wave_complete";

    public static string buy_upgrade = "buy_upgrade";
}

public class LevelStartEventData
{
    public int level;
    //[JsonProperty("days since reg")]
    public int days_since_reg;
}

public class LevelCompleteEventData
{
    public int level;
    public int heroLevel;
    public int time_spent;
    //[JsonProperty("days since reg")]
    public int days_since_reg;
}

public class LevelFailEventData
{
    public int level;
    public int heroLevel;
    public int wave;
    public string reason;
    public int time_spent;
    //[JsonProperty("days since reg")]
    public int days_since_reg;
}

public class WaveStartEventData
{
    public int level;
    public int wave;
    //[JsonProperty("days since reg")]
    public int days_since_reg;
}

public class WaveCompleteEventData
{
    public int level;
    public int wave;
    //[JsonProperty("days since reg")]
    public int days_since_reg;
}

public class BuyUpgradeEventData
{
    public string name;
    public int level;
    ////[JsonProperty("days since reg")]
    public int days_since_reg;
}
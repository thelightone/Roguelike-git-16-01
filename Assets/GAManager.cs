using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAManager : MonoBehaviour
{
    public static GAManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
    }
    
    public void OnLevelComplete(int _level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level Finish"+_level);
        Debug.Log("Level: " + _level);
    }
    public void OnLevelLose(int _level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level Lose" + _level);
        Debug.Log("Level: " + _level);
    }
    public void OnLevelStarted(int _level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level Started" + _level);
        Debug.Log("Level: " + _level);
    }

    public void OnGameStarted()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Game Started");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public delegate void TickDelegate();
    public static event TickDelegate OnTick;

    void FixedUpdate()
    {
        OnTick?.Invoke();
    }
}

//{
//    void OnEnable()
//    {
//        // Subscribe to the OnTick event when the script is enabled
//        TickManager.OnTick += UpdateLogic;
//    }

//    void OnDisable()
//    {
//        // Unsubscribe from the OnTick event when the script is disabled or destroyed
//        TickManager.OnTick -= UpdateLogic;
//    }

//    void UpdateLogic()
//    {
//        // Your update logic goes here
//    }
//}
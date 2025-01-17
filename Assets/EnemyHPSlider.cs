using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPSlider : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Vector3 _rotation;

    private void OnEnable()
    {
        // Subscribe to the OnTick event when the script is enabled
        Ticker.OnTick += UpdateLogic;

    }

    private void OnDisable()
    {
        // Unsubscribe from the OnTick event when the script is disabled or destroyed
        Ticker.OnTick -= UpdateLogic;
    }

    private void UpdateLogic()
    {
        _canvas.transform.rotation = Quaternion.Euler(_rotation);
    }
}

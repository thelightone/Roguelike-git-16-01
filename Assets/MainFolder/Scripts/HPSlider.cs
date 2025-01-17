using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    private float _maxHealth;
    private float _curHealth;
    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();
        PlayerMoveController.healthChange.AddListener(() => UpdateHealth());
        _slider.maxValue = _maxHealth = PlayerMoveController.Instance._maxHealth;
        _slider.value = _curHealth = PlayerMoveController.Instance._curHealth;
    }

    public void UpdateHealth()
    {
        _slider.maxValue = _maxHealth = PlayerMoveController.Instance._maxHealth;
        _slider.value = _curHealth = PlayerMoveController.Instance._curHealth;
    }
}
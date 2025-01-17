using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPSlider : MonoBehaviour
{
    private float _maxXP;
    private float _curXP;
    private Slider _slider;
    [SerializeField] private TMP_Text _level;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _level = GetComponentInChildren<TMP_Text>();
        PlayerMoveController.xpChange.AddListener(() => UpdateXP());
        PlayerMoveController.levelUpChange.AddListener(() => LevelUp());
        PlayerMoveController.getMaxLevel.AddListener(() => GetMaxLevel());
        _slider.maxValue = _maxXP = 50;
        _slider.value = _curXP = 0;
        _level.text = "1";
    }

    private void UpdateXP()
    {
        _slider.value = _curXP = PlayerMoveController.Instance._curXP;

    }

    private void LevelUp()
    {
        _maxXP += PlayerMoveController.Instance._level*25;
        _slider.maxValue = _maxXP;
        _slider.value = _curXP = 0;
        _level.text = PlayerMoveController.Instance._level.ToString();
    }

    private void GetMaxLevel()
    {
        _level.text = "MAX";
    }
}

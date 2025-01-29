using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] public Slider charter1Slider;
    [SerializeField] public Slider charter2Slider;
    [SerializeField] private GameObject parent;
    private int _curLevel;
    [SerializeField] private TMP_Text _levelText;

    // OLD
    private void Awake()
    {
        UpdateLevelView();
        ChooseLevel();
    }

    private void UpdateLevelView()
    {
        _curLevel = PrefsManager.GetUnlockedLevelCharter1();
                                                         
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
            }

            parent.transform.GetChild(_curLevel).gameObject.SetActive(true);
            parent.transform.GetChild(parent.transform.childCount-1).gameObject.SetActive(true);       
    }

    public void ChooseLevel()
    {
        var chosenLogosParent = parent.transform.GetChild(parent.transform.childCount - 1);
        for (var i = 0; i < chosenLogosParent.transform.childCount; i++)
        {
            chosenLogosParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        if (PrefsManager.GetChosenLevelCharter1() == 4)
        {
            chosenLogosParent.GetChild(3).GetChild(0).gameObject.SetActive(true);
            _levelText.text = "Charter 1-4"+  "\n" + "Lava Cave";
        }
        else
        {
            chosenLogosParent.GetChild(PrefsManager.GetChosenLevelCharter1()).GetChild(0).gameObject.SetActive(true);
            _levelText.text = "Charter 1-" + (PrefsManager.GetChosenLevelCharter1() + 1)+ "\n" + "Lava Cave";
        }

    }
    //NEW

    private void OnEnable()
    {
        charter1Slider.value = PrefsManager.GetUnlockedLevelCharter1();
        charter2Slider.value = PrefsManager.GetUnlockedLevelCharter2();
    }
}

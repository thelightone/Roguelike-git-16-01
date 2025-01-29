using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoughtCell : MonoBehaviour
{
    public Image image;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    public void ShowLevel (int level)
    {
        image.gameObject.SetActive(true);
        _star2.SetActive(false);
        _star3.SetActive(false);

        _star1.SetActive (true);
        if (level>1)
        {
            _star2.SetActive (true);
        }
        if (level > 2)
        {
            _star3.SetActive(true);
        }
    }

    public void Reset ()
    {
        _star1.SetActive(false);
        _star2.SetActive(false);
        _star3.SetActive(false);
        image.gameObject.SetActive(false);
    }

}

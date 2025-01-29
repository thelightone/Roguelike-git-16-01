using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _descr;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _newLabel;

    public void UpdateCard(WeaponInfo info)
    {
        _image.sprite = info.Image;
        _name.text = info.Name;
        _descr.text = info.Descr.Replace("___","\n");

        _star1.SetActive(false); 
        _star2.SetActive(false);

        if (info.Level-1>0)
        {
            _star1.SetActive(true);
            _newLabel.SetActive(false);
        }
        if (info.Level - 1 > 1)
        {
            _star2.SetActive(true);
            _newLabel.SetActive(false);
        }
        if (info.Level - 1 == 0)
        {
            _newLabel.SetActive(true);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShopData : MonoBehaviour
{
    public Sprite sprite;
    public string name;
    public int price;
    public PriceType priceType;
    public int id;

    public GameObject chooseBG;
    public GameObject unchooseBG;
    public GameObject priceOnCard;

    public Sprite skillSprite;
    public Sprite weaponSprite;
    public string skillName;
    public string weaponName;

    public WeaponTypeParent weaponParent;

    public void Awake()
    {
        chooseBG = transform.parent.GetChild(1).gameObject;

        unchooseBG = transform.parent.GetChild(0).gameObject;
    }

    public void Init()
    {

        if (PlayerPrefs.GetInt("hero", 0) == id)
        {
            chooseBG.SetActive(true);
            unchooseBG.SetActive(false);
        }
        else
        {
            chooseBG.SetActive(false);
            unchooseBG.SetActive(true);
        }

        if (PlayerPrefs.GetInt("hero" + id, 0) == 1)
        {
            RemovePrice();
        }
    }

    public void Choose()
    {
        chooseBG.SetActive(true);
        unchooseBG.SetActive(false);
    }

    public void UnChoose()
    {
        unchooseBG.SetActive(true);
        chooseBG.SetActive(false);
    }

    public void RemovePrice()
    {
        priceOnCard.SetActive(false);
    }
}
public enum PriceType
{
    Gems,
    Coins
}


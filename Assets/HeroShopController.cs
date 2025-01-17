using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroShopController : MonoBehaviour
{
    public Image mainImage;
    public TMP_Text mainName;
    public int mainId;
    public TMP_Text priceText;
    public PriceType priceType;
    public Transform priceTypeBut;
    public List<Button> heroesButtons = new List<Button>();
    public Transform buyButtonParent;
    public Image mainSkillImage;
    public TMP_Text mainSkillName;
    public Image mainWeapImage;
    public TMP_Text mainWeapName;

    public Button select;
    public Button buy;

    public HeroShopData choosenCard;

    public TMP_Text coins;
    public TMP_Text gems;

    public GameObject addGemsPanel;
    public WeaponsList wpl;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        select.onClick.AddListener(() => Select());
        //buy.onClick.AddListener(() => Buy());


        foreach (var button in heroesButtons)
        {
            button.onClick.AddListener(() => ChooseCard(button.GetComponent<HeroShopData>().id));
            button.GetComponent<HeroShopData>().Init();
        }

        PlayerPrefs.SetInt("hero0", 1);
    }

    public void OnEnable()
    {
        Init();

        coins.text = PlayerPrefs.GetInt("coins", 0).ToString();
        gems.text = PlayerPrefs.GetInt("gems", 0).ToString();
    }

    public void ChooseCard(int id)
    {
        mainId = id;
        choosenCard = heroesButtons[id].GetComponent<HeroShopData>();
        mainImage.sprite = choosenCard.sprite;
        mainName.text = choosenCard.name;
        priceText.text = choosenCard.price.ToString();
        priceType = choosenCard.priceType;
        mainSkillImage.sprite = choosenCard.skillSprite;
        mainWeapImage.sprite = choosenCard.weaponSprite;
        mainSkillName.text = choosenCard.skillName;
        mainWeapName.text = choosenCard.weaponName;

        priceTypeBut.GetChild(0).gameObject.SetActive(false);
        priceTypeBut.GetChild(1).gameObject.SetActive(false);

        if (priceType == PriceType.Gems)
        {
            priceTypeBut.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            priceTypeBut.GetChild(0).gameObject.SetActive(true);
        }

        foreach (var button in heroesButtons)
        {
           button.GetComponent<HeroShopData>().Init();
        }

        buyButtonParent.GetChild(0).gameObject.SetActive(false);
        buyButtonParent.GetChild(1).gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("hero"+choosenCard.id,0)==1)
        {
           // choosenCard.Choose();
            buyButtonParent.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            buyButtonParent.GetChild(0).gameObject.SetActive(true);
        }

    }

    public void Select()
    {
        foreach (var button in heroesButtons)
        {
            button.GetComponent<HeroShopData>().UnChoose();
        }
        choosenCard.Choose();
        PlayerPrefs.SetInt("hero", mainId);
    }

    public void Buy()
    {
        Debug.Log("SSSS");   
            var g = PlayerPrefs.GetInt("gems", 0);
            var c = PlayerPrefs.GetInt("coins", 0);

            if (choosenCard.priceType == PriceType.Gems)
            {
                if (choosenCard.price <= g)
                {
                    PlayerPrefs.SetInt("gems", g - choosenCard.price);
                    buyButtonParent.GetChild(0).gameObject.SetActive(false);
                    buyButtonParent.GetChild(1).gameObject.SetActive(true);
                    PlayerPrefs.SetInt("hero" + choosenCard.id.ToString(), 1);
                    choosenCard.RemovePrice();
                    if (!wpl.availableWeapons.Contains(choosenCard.weaponParent))
                        wpl.availableWeapons.Add(choosenCard.weaponParent);
                }
                else
                {
                    addGemsPanel.SetActive(true);
                }
            }
            else
            {
                if (choosenCard.price <= c)
                {
                    PlayerPrefs.SetInt("coins", c - choosenCard.price);
                    buyButtonParent.GetChild(0).gameObject.SetActive(false);
                    buyButtonParent.GetChild(1).gameObject.SetActive(true);
                    PlayerPrefs.SetInt("hero" + choosenCard.id.ToString(), 1);
                    choosenCard.RemovePrice();
                    if (!wpl.availableWeapons.Contains(choosenCard.weaponParent))
                        wpl.availableWeapons.Add(choosenCard.weaponParent);
                }
            }

            coins.text = PlayerPrefs.GetInt("coins", 0).ToString();
            gems.text = PlayerPrefs.GetInt("gems", 0).ToString();
        
    }
}

using Facebook.MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TMP_Text _upgradeName;
    [SerializeField] private TMP_Text _upgradeDescr;
    [SerializeField] private TMP_Text _upgradeCost;
    [SerializeField] private Image _chosenImage;

    [SerializeField] private ShopData _shopData;
    [SerializeField] private HeroData _heroData;

    [SerializeField] private MenuManager _menuManager;

    [SerializeField] private Button _hp;

    [SerializeField] private Button _regen;

    [SerializeField] private Button _speed;

    [SerializeField] private Button _armor;

    [SerializeField] private Button _damage;

    [SerializeField] private Button _attackSpeed;

    [SerializeField] private Button _rotate;

    [SerializeField] private Button _hitForce;

    [SerializeField] private Button _xp;

    [SerializeField] private Button _coins;

    [SerializeField] private Button _select;

    [SerializeField] private GameObject _canBuyBut;
    [SerializeField] private GameObject _noCanBuyBut;

    private int maxLvlHP = 30;
    private int maxLvlReg = 10;
    private int maxLvlArm = 30;
    private int maxLvlDam = 30;
    private int maxLvlAttSpeed = 30;
    private int maxLvlRotate = 30;
    private int maxLvlHitForce = 10;
    private int maxLvlXP = 30;
    private int maxLvlCoins = 30;

    private List<Button> _buttons = new List<Button>();

    public Button _chosenButton;
    private float _chosenPrice;

    private void Start()
    {
        _hp.onClick.AddListener(() => ChooseUpgrade(_hp));
        _regen.onClick.AddListener(() => ChooseUpgrade(_regen));
        _speed.onClick.AddListener(() => ChooseUpgrade(_speed));
        _armor.onClick.AddListener(() => ChooseUpgrade(_armor));
        _damage.onClick.AddListener(() => ChooseUpgrade(_damage));
        _attackSpeed.onClick.AddListener(() => ChooseUpgrade(_attackSpeed));
        _rotate.onClick.AddListener(() => ChooseUpgrade(_rotate));
        _hitForce.onClick.AddListener(() => ChooseUpgrade(_hitForce));
        _xp.onClick.AddListener(() => ChooseUpgrade(_xp));
        _coins.onClick.AddListener(() => ChooseUpgrade(_coins));

        _select.onClick.AddListener(() => SelectUpgrade());

        _buttons.Add(_hp);
        _buttons.Add(_regen);
        _buttons.Add(_speed);
        _buttons.Add(_armor);
        _buttons.Add(_damage);
        _buttons.Add(_attackSpeed);
        _buttons.Add(_rotate);
        _buttons.Add(_hitForce);
        _buttons.Add(_xp);
        _buttons.Add(_coins);

        InitLevels();

        ChooseUpgrade(_hp);
    }

    private void InitLevels()
    {
        var gameData = JSONSaver.LoadJsonFromFile();

        
        //ShowLevel(_hp, gameData.heroMaxHealthLevel);
        //ShowLevel(_regen, gameData.heroRegenerateLevel);
        //ShowLevel(_xp, gameData.heroMultiplyXpReceivedLevel);
        //ShowLevel(_coins, gameData.coins);
        //ShowLevel(_rotate, gameData.weapMultiplRotationLevel);
        //ShowLevel(_hitForce, gameData.weapHitForceLevel);
        //ShowLevel(_speed, gameData.heroSpeedLevel);
        //ShowLevel(_attackSpeed, gameData.weapAttackSpeedLevel);
        //ShowLevel(_armor, gameData.heroDefenceLevel);
        //ShowLevel(_damage, gameData.weapDamageLevel);

        _shopData.heroMaxHealthLevel = gameData.heroMaxHealthLevel;
        _shopData.heroRegenerateLevel = gameData.heroRegenerateLevel;
        _shopData.heroMultiplyXpReceivedLevel = gameData.heroMultiplyXpReceivedLevel;
        _shopData.heroMultiplyCoinCollectLevel = gameData.heroMultiplyCoinCollectLevel;
        _shopData.weapMultiplRotationLevel = gameData.weapMultiplRotationLevel;
        _shopData.weapHitForceLevel = gameData.weapHitForceLevel;
       _shopData.heroSpeedLevel= gameData.heroSpeedLevel;
        _shopData.weapAttackSpeedLevel= gameData.weapAttackSpeedLevel;
         _shopData.heroDefenceLevel= gameData.heroDefenceLevel;
         _shopData.weapDamageLevel= gameData.weapDamageLevel;

        ShowLevel(_hp, _shopData.heroMaxHealthLevel);
        ShowLevel(_regen, _shopData.heroRegenerateLevel);
        ShowLevel(_xp, _shopData.heroMultiplyXpReceivedLevel);
        ShowLevel(_coins, _shopData.heroMultiplyCoinCollectLevel);
        ShowLevel(_rotate, _shopData.weapMultiplRotationLevel);
        ShowLevel(_hitForce, _shopData.weapHitForceLevel);
        ShowLevel(_speed, _shopData.heroSpeedLevel);
        ShowLevel(_attackSpeed, _shopData.weapAttackSpeedLevel);
        ShowLevel(_armor, _shopData.heroDefenceLevel);
        ShowLevel(_damage, _shopData.weapDamageLevel);
    }

    private void ShowLevel(Button but, float level)
    {
        var levelsIconsParent = but.transform.GetChild(5);

        if (level>0)
        {
            levelsIconsParent.gameObject.SetActive(true);
            levelsIconsParent.GetComponent<TMP_Text>().text = "Lvl." + level;
        }

    }

    public void ChooseUpgrade(Button but)
    {

        HighlightBut(but);

        if (but == _hp)
        {
            DrawInfoOnChoose("HEALTH", "Increase health by 20 per level", _shopData.heroMaxHealthLevel,maxLvlHP, but.transform.GetChild(3));
        }
        else if (but == _regen)
        {
            DrawInfoOnChoose("REGENERATION", "Increase regeneration by 0.3 HP/second per level", _shopData.heroRegenerateLevel,maxLvlReg, but.transform.GetChild(3));
        }
        else if (but == _speed)
        {
            DrawInfoOnChoose("MOVE SPEED", "Increase move speed by 10% per level", _shopData.heroSpeedLevel,3, but.transform.GetChild(3));
        }
        else if (but == _armor)
        {
            DrawInfoOnChoose("ARMOR", "Increase armor by 10% per level", _shopData.heroDefenceLevel,maxLvlArm, but.transform.GetChild(3));
        }
        else if (but == _damage)
        {
            DrawInfoOnChoose("DAMAGE", "Increase damage by 10% per level", _shopData.weapDamageLevel,maxLvlDam, but.transform.GetChild(3));
        }
        else if (but == _attackSpeed)
        {
            DrawInfoOnChoose("ATTACK SPEED", "Increase attack speed by 10% per level", _shopData.weapAttackSpeedLevel,maxLvlAttSpeed, but.transform.GetChild(3));
        }
        else if (but == _rotate)
        {
            DrawInfoOnChoose("ROTATION", "Increase speed of rotating weapons by 10% per level", _shopData.weapMultiplRotationLevel,maxLvlRotate, but.transform.GetChild(3));
        }
        else if (but == _hitForce)
        {
            DrawInfoOnChoose("HIT FORCE", "Increase force of pushing enemies by 10% per level", _shopData.weapHitForceLevel,maxLvlHitForce, but.transform.GetChild(3));
        }
        else if (but == _xp)
        {
            DrawInfoOnChoose("EXPERIENCE", "Increase amount of collected XP by 10% per level", _shopData.heroMultiplyXpReceivedLevel,maxLvlXP, but.transform.GetChild(3));
        }
        else if (but == _coins)
        {
            DrawInfoOnChoose("COINS", "Increase amount of collected coins by 10% per level", _shopData.heroMultiplyCoinCollectLevel, maxLvlCoins, but.transform.GetChild(3));
        }

        _chosenButton = but;
    }

    private void DrawInfoOnChoose(string name, string descr, int level,int maxLevel, Transform icon)
    {
        _upgradeName.text = name;
        _upgradeDescr.text = descr;
        _chosenImage.sprite = icon.GetComponent<Image>().sprite;

        //_chosenPrice = (1 + _shopData.priceModif * 0.1f) * 100 * (level + 1);
        switch (level)
        {
            case 0:
                _chosenPrice = 20;
                break;
            case 1:
                _chosenPrice = 50;
                break;
            case 2:
                _chosenPrice = 150;
                break;
            case 3:
                _chosenPrice = 300;
                break;
            case 4:
                _chosenPrice = 900;
                break;
            case 5:
                _chosenPrice = 1500;
                break;
            case 6:
                _chosenPrice = 5000;
                break;
            default:
                _chosenPrice = 5000 * 2.5f * (level - 6);
                break;
        }

        _upgradeCost.text = _chosenPrice.ToString();

        if (_chosenPrice <= PrefsManager.GetCoins())
        {
            _canBuyBut.SetActive(true);
            _noCanBuyBut.SetActive(false);
        }

        if (level >= maxLevel || _chosenPrice > PrefsManager.GetCoins())
        {
            _canBuyBut.SetActive(false);
            _noCanBuyBut.SetActive(true);

        }


    }

    private void HighlightBut(Button but)
    {
        foreach (var item in _buttons)
        {
            item.transform.GetChild(4).gameObject.SetActive(false);
        }
        but.transform.GetChild(4).gameObject.SetActive(true);
    }

    public void SelectUpgrade()
    {
        if (_chosenButton == _hp)
        {
            UpgradeField(ref _shopData.heroMaxHealthLevel,maxLvlHP, _hp, ref _heroData.heroMaxHealth, 120, 140, 160,JSONSaver.DataTypes.heroMaxHealthLevel);
        }
        else if (_chosenButton == _regen)
        {
            UpgradeField(ref _shopData.heroRegenerateLevel,maxLvlReg, _regen, ref _heroData.heroRegenerate, 0.6f, 0.9f, 1.2f, JSONSaver.DataTypes.heroRegenerateLevel);
        }
        else if (_chosenButton == _speed)
        {
            UpgradeField(ref _shopData.heroSpeedLevel,3, _speed, ref _heroData.heroSpeed, 8, 9, 10, JSONSaver.DataTypes.heroSpeedLevel);
        }
        else if (_chosenButton == _armor)
        {
            UpgradeField(ref _shopData.heroDefenceLevel,maxLvlArm, _armor, ref _heroData.heroDefence, 0.2f, 0.3f, 0.4f, JSONSaver.DataTypes.heroDefenceLevel);
        }
        else if (_chosenButton == _damage)
        {
            UpgradeField(ref _shopData.weapDamageLevel,maxLvlDam, _damage, ref _heroData.weapDamage, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.weapDamageLevel);
        }
        else if (_chosenButton == _attackSpeed)
        {
            UpgradeField(ref _shopData.weapAttackSpeedLevel,maxLvlAttSpeed, _attackSpeed, ref _heroData.weapAttackSpeed, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.weapAttackSpeedLevel);
        }
        else if (_chosenButton == _rotate)
        {
            UpgradeField(ref _shopData.weapMultiplRotationLevel,maxLvlRotate, _rotate, ref _heroData.weapMultiplRotation, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.weapMultiplRotationLevel);
        }
        else if (_chosenButton == _hitForce)
        {
            UpgradeField(ref _shopData.weapHitForceLevel,maxLvlHitForce, _hitForce, ref _heroData.weapHitForce, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.weapHitForceLevel);
        }
        else if (_chosenButton == _xp)
        {
            UpgradeField(ref _shopData.heroMultiplyXpReceivedLevel,maxLvlXP, _xp, ref _heroData.heroMultiplyXpReceived, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.heroMultiplyXpReceivedLevel);
        }
        else if (_chosenButton == _coins)
        {
            UpgradeField(ref _shopData.heroMultiplyCoinCollectLevel,maxLvlCoins, _coins, ref _heroData.heroMultiplyCoinCollect, 1.1f, 1.2f, 1.3f, JSONSaver.DataTypes.heroMultiplyCoinCollectLevel);
        }
    }

   public void UpgradeField(ref int curlevel, int maxLevel, Button button, ref float dataOnLevel, float level1, float level2, float level3, JSONSaver.DataTypes dataType)
    {
        var curLevel = curlevel;

        if (curLevel <= maxLevel-1 && _chosenPrice <= PrefsManager.GetCoins())
        {

            StartCoroutine(ShowEffect(button.transform.GetChild(2).gameObject));

            var nextLevel = curLevel + 1;
            button.transform.GetChild(5).gameObject.SetActive(true);
            button.transform.GetChild(5).GetComponent<TMP_Text>().text = "Lvl."+nextLevel;

        curlevel = nextLevel;

            _shopData.priceModif++;

            PrefsManager.ChangeCoins(-(int)_chosenPrice);
            
            _menuManager.UpdateBalances();

            ChooseUpgrade(_chosenButton);

            JSONSaver.UpdateJsonFile(dataType, nextLevel);


            BuyUpgradeEventData eventData = new BuyUpgradeEventData()
            {
                level = curLevel,
                name = _upgradeName.text,
                days_since_reg = PrefsManager.DaysFromReg(),
            };

            string json = JsonUtility.ToJson(eventData);
            //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.buy_upgrade, json);

            //AppMetrica.Instance.SendEventsBuffer();
        }

        

    }

    private IEnumerator ShowEffect(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(1);
        go.SetActive(false);
    }
    public void BuyTutor(Button button)
    {
        StartCoroutine(ShowEffect(button.transform.GetChild(2).gameObject));
        var curLevel = 0;
        var nextLevel = curLevel + 1;
        button.transform.GetChild(5).gameObject.SetActive(true);
        button.transform.GetChild(5).GetComponent<TMP_Text>().text = "Lvl." + nextLevel;

        curLevel = nextLevel;

        _shopData.priceModif++;

        PrefsManager.ChangeCoins(-(int)_chosenPrice);
        _menuManager.UpdateBalances();

        ChooseUpgrade(_chosenButton);

        JSONSaver.UpdateJsonFile(JSONSaver.DataTypes.heroMaxHealthLevel, nextLevel);


        BuyUpgradeEventData eventData = new BuyUpgradeEventData()
        {
            level = curLevel,
            name = _upgradeName.text,
            days_since_reg = PrefsManager.DaysFromReg(),
        };

        string json = JsonUtility.ToJson(eventData);
        //AppMetrica.Instance.ReportEvent(AppMetricaEventsTypes.buy_upgrade, json);

        //AppMetrica.Instance.SendEventsBuffer();
    }
}

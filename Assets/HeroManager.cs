using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public float heroId;
    public Transform heroParent;
    public WeaponsList weapons;
    public PlayerMoveController hero;

    public List<WeaponTypeParent> heroWeapons = new List<WeaponTypeParent>();

    void Awake()
    {
        heroId = PlayerPrefs.GetInt("hero", 0);

        ChooseSkin();

        ChooseWeapon();

        //skill choose in skills button controller

    }

    private void ChooseWeapon()
    {
        var num = 0;
        foreach (var weapon in weapons.availableWeapons)
        {
            if (weapon == heroWeapons[(int)heroId])
                num++;
        }

        if (num == 0)
        {
            weapons.availableWeapons.Add(heroWeapons[(int)heroId]);
        }

        hero._baseWeapon = heroWeapons[(int)heroId];
    }

    private void ChooseSkin()
    {

        for (var i = 0; i < 4; i++)
        {
            heroParent.GetChild(i).gameObject.SetActive(false);
        }


        heroParent.GetChild(PlayerPrefs.GetInt("hero", 0)).gameObject.SetActive(true);
    }
}

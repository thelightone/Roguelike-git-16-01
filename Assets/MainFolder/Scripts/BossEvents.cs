using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvents : MonoBehaviour
{
    private BossController _boss;

    private void Start()
    {
        _boss = GetComponentInParent<BossController>();
    }

    public void StartSkill()
    {
        _boss.StartSkill();
    }

    public void StopSkill()
    {
        _boss.StopSkill();
    }
}

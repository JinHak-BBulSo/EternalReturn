using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : SkillUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateInteractable(int playerLevel, int weaponMasteryLevel)
    {
        base.UpdateInteractable(playerLevel, weaponMasteryLevel);
        button.interactable = (skillInfo.CurrentLevel < (playerLevel - 1) / 5) ? true : false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillUI : SkillUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override bool IsInteractable(int totalSkillLevel, int playerLevel, int weaponMasteryLevel)
    {
        if (!base.IsInteractable(totalSkillLevel, playerLevel, weaponMasteryLevel))
            return false;

        if (skillInfo.CurrentLevel >= (weaponMasteryLevel - 1) / 7)
            return false;

        return true;
    }
}

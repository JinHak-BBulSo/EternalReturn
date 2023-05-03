using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillUI : SkillUI
{
    private Image skillLevelUpUI;

    protected override void Awake()
    {
        base.Awake();
        skillLevelUpUI = transform.GetChild(1).GetComponent<Image>();
        skillLevelUpUI.color = Color.cyan;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateInteractable(int playerLevel, int weaponMasteryLevel)
    {
        base.UpdateInteractable(playerLevel, weaponMasteryLevel);
        button.interactable = (skillInfo.CurrentLevel < (weaponMasteryLevel - 1) / 7) ? true : false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillUI : SkillUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        skillInfo.AddLevel();
        skillLevelBgRect.GetChild(skillInfo.CurrentLevel - 1).GetComponent<Image>().color = Color.white;
    }

    public override void UpdateInteractable(int playerLevel, int weaponMasteryLevel)
    {
        base.UpdateInteractable(playerLevel, weaponMasteryLevel);
    }
}

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
        image.color = Color.white;
        skillLevelBgRect.GetChild(skillInfo.CurrentLevel - 1).GetComponent<Image>().color = Color.white;
    }

    protected override void Update()
    {
        /* Do nothing */
    }

    public override void UpdateInteractable(int playerLevel, int weaponMasteryLevel)
    {
        base.UpdateInteractable(playerLevel, weaponMasteryLevel);
    }
}

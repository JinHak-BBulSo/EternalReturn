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

    protected override bool IsInteractable(int totalSkillLevel, int playerLevel, int weaponMasteryLevel)
    {
        if (!base.IsInteractable(totalSkillLevel, playerLevel, weaponMasteryLevel))
            return false;

        if (skillInfo.CurrentLevel * 4 >= playerLevel)
            return false;

        return true;
    }
}

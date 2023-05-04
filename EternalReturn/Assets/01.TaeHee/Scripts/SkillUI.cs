using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private const string INGAME_PREFABS_PATH = "09.InGameUI/Prefabs/";
    private const string INGAME_CHARACTER_PROFILES_PATH = "09.InGameUI/Sprite/CharacterProfiles/";
    private const string INGAME_SKILL_ICONS_PATH = "09.InGameUI/Sprite/SKillIcons/";
    private const string SKILL_LEVEL_PREFAB_NAME = "Skill Level";

    private static PlayerSkillSystem playerSkillSystem;
    private static PlayerBase playerBase;


    protected Button button;
    protected SkillInfo skillInfo;
    protected RectTransform skillLevelBgRect;


    protected virtual void Awake()
    {
        button = transform.GetChild(1).GetComponent<Button>();
        skillLevelBgRect = transform.GetChild(0).GetComponent<RectTransform>();

        if (playerSkillSystem == null)
        {
            playerSkillSystem = PlayerManager.Instance.Player.GetComponent<PlayerSkillSystem>();
            playerBase = playerSkillSystem.GetComponent<PlayerBase>();
        }
        skillInfo = playerSkillSystem.skillInfos[transform.GetSiblingIndex()];

        for (int i = 0; i < skillInfo.MaxLevel; i++)
        {
            GameObject skillLevelInst = Instantiate(InGameUIResources.skillLevel);
            skillLevelInst.transform.SetParent(skillLevelBgRect, false);

            RectTransform instRect = skillLevelInst.GetComponent<RectTransform>();
            instRect.sizeDelta = new Vector2((skillLevelBgRect.rect.width - 2) / skillInfo.MaxLevel, skillLevelBgRect.rect.height / 2);
            instRect.anchoredPosition = new Vector2((-0.5f * (skillInfo.MaxLevel - 1) + i) * instRect.rect.width, 0);
        }
    }

    protected virtual void Start()
    {

    }

    public virtual void UpdateInteractable(int playerLevel, int weaponMasteryLevel)
    {
        button.gameObject.SetActive(true);
        button.interactable = (skillInfo.CurrentLevel < skillInfo.MaxLevel) ? true : false;
    }

    public void HideLevelUpUI()
    {
        button.gameObject.SetActive(false);
    }

    public virtual void OnClickLevelUp()
    {
        if (skillInfo.CurrentLevel >= skillInfo.MaxLevel)
            return;

        skillInfo.AddLevel();

        skillLevelBgRect.GetChild(skillInfo.CurrentLevel - 1).GetComponent<Image>().color = Color.white;
        PlayerUI.Instance.UpdateSkillLevelUpUI(playerSkillSystem.GetTotalSkillLevel(), playerSkillSystem.GetWeaponSkillLevel(), playerBase.playerStat.playerExp.level, playerBase.playerStat.weaponExp.level);

        Debug.Log($"SkillLv: {skillInfo.CurrentLevel}");
    }
}

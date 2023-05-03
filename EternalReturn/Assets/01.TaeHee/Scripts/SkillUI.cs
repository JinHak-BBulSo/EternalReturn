using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private static GameObject skillLevel;
    private static PlayerSkillSystem playerSkillSystem;

    private const string INGAME_PREFABS_PATH = "09.InGameUI/Prefabs/";
    private const string SKILL_LEVEL_PREFAB_NAME = "Skill Level";

    protected Button button;
    protected SkillInfo skillInfo;
    protected RectTransform skillLevelBgRect;

    protected virtual void Awake()
    {
        if (skillLevel == null)
        {
            skillLevel = Resources.Load($"{ INGAME_PREFABS_PATH}{SKILL_LEVEL_PREFAB_NAME}") as GameObject;
        }

        button = transform.GetChild(1).GetComponent<Button>();
        skillLevelBgRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        if (playerSkillSystem == null)
        {
            playerSkillSystem = PlayerManager.Instance.Player.GetComponent<PlayerSkillSystem>();
        }
        skillInfo = playerSkillSystem.skillInfos[transform.GetSiblingIndex()];

        for (int i = 0; i < skillInfo.MaxLevel; i++)
        {
            GameObject skillLevelInst = Instantiate(skillLevel);
            skillLevelInst.transform.SetParent(skillLevelBgRect, false);

            RectTransform instRect = skillLevelInst.GetComponent<RectTransform>();
            instRect.sizeDelta = new Vector2((skillLevelBgRect.rect.width - 2) / skillInfo.MaxLevel, skillLevelBgRect.rect.height / 2);
            instRect.anchoredPosition = new Vector2((-0.5f * (skillInfo.MaxLevel - 1) + i) * instRect.rect.width, 0);
        }
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
        PlayerUI.Instance.UpdateSkillLevelUpUI(playerSkillSystem.GetTotalSkillLevel(), playerSkillSystem.GetWeaponSkillLevel(), 5, 1);

        Debug.Log($"SkillLv: {skillInfo.CurrentLevel}");
    }
}

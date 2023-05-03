using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private static GameObject skillLevel;

    protected Button button;
    protected SkillInfo skillInfo;
    protected RectTransform skillLevelBgRect;

    //
    private static PlayerSkillSystem playerSkillSystem;

    protected virtual void Awake()
    {
        button = transform.GetChild(1).GetComponent<Button>();

        if (skillLevel == null)
        {
            skillLevel = Resources.Load("09.UI/Prefabs/Skill Level") as GameObject;
        }
        skillLevelBgRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        if (playerSkillSystem == null)
        {
            //player = PlayerManager.Instance.Player;
            playerSkillSystem = GameObject.Find("Test Player").GetComponent<PlayerSkillSystem>();
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

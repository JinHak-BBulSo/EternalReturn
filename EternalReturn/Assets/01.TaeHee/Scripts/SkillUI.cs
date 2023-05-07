using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private static PlayerSkillSystem playerSkillSystem;
    private static PlayerBase playerBase;

    protected Image image;
    protected Button button;
    protected SkillInfo skillInfo;
    protected RectTransform skillLevelBgRect;

    private int index;
    private Image coolDownImage;
    private Text coolDownText;

    protected virtual void Awake()
    {
        button = transform.GetChild(1).GetComponent<Button>();
        skillLevelBgRect = transform.GetChild(0).GetComponent<RectTransform>();

        index = transform.GetSiblingIndex();
        image = GetComponent<Image>();
        coolDownImage = transform.GetChild(2).GetComponent<Image>();
        coolDownText = coolDownImage.transform.GetChild(0).GetComponent<Text>();

        if (playerSkillSystem == null)
        {
            playerSkillSystem = PlayerManager.Instance.Player.GetComponent<PlayerSkillSystem>();
            playerBase = playerSkillSystem.GetComponent<PlayerBase>();
        }

        skillInfo = playerSkillSystem.skillInfos[index];
        image.color = Color.gray;

        for (int i = 0; i < skillInfo.MaxLevel; i++)
        {
            GameObject skillLevelInst = Instantiate(UIResources.skillLevel);
            skillLevelInst.transform.SetParent(skillLevelBgRect, false);

            RectTransform instRect = skillLevelInst.GetComponent<RectTransform>();
            instRect.sizeDelta = new Vector2((skillLevelBgRect.rect.width - 2) / skillInfo.MaxLevel, skillLevelBgRect.rect.height / 2);
            instRect.anchoredPosition = new Vector2((-0.5f * (skillInfo.MaxLevel - 1) + i) * instRect.rect.width, 0);
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (playerBase.skillCooltimes[index])
        {
            coolDownImage.gameObject.SetActive(true);
            coolDownImage.fillAmount = playerBase.skillSystem.skillInfos[index].currentCooltime /
                (playerBase.skillSystem.skillInfos[index].cooltime * ((100 - playerBase.playerTotalStat.coolDown) / 100));
            coolDownText.text = $"{(int)playerBase.skillSystem.skillInfos[index].currentCooltime}";
        }
        else
        {
            coolDownImage.gameObject.SetActive(false);
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

        image.color = Color.white;
        skillLevelBgRect.GetChild(skillInfo.CurrentLevel - 1).GetComponent<Image>().color = Color.white;
        PlayerUI.Instance.UpdateSkillLevelUpUI(playerSkillSystem.GetTotalSkillLevel(), playerSkillSystem.GetWeaponSkillLevel(), playerBase.playerStat.playerExp.level, playerBase.playerStat.weaponExp.level);
        playerBase.SkillPoint[index] = playerBase.skillSystem.skillInfos[index].CurrentLevel;
        playerBase.ItemChang();


        Debug.Log($"SkillLv: {skillInfo.CurrentLevel}");
    }
}

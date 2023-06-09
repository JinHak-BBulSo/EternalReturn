using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterType
{
    Aya = 0,
    Jackie = 1
}

public class PlayerUI : SingleTonBase<PlayerUI>
{
    [SerializeField] private Image profileImage;

    [SerializeField] private GameObject skillBg;
    private List<SkillUI> skillUIs = new List<SkillUI>();

    [SerializeField] private Text levelUI;
    [SerializeField] private Image hpGauge;
    private RectTransform hpGaugeRect;
    private Text hpText;
    private RectTransform hpPulseRect;
    [SerializeField] private Image spGauge;
    private Text spText;
    [SerializeField] private Image expGauge;

    protected override void Awake()
    {
        base.Awake();

        hpText = hpGauge.transform.GetChild(0).GetComponent<Text>();
        hpGaugeRect = hpGauge.GetComponent<RectTransform>();
        hpPulseRect = hpGauge.transform.GetChild(1).GetComponent<RectTransform>();
        spText = spGauge.transform.GetChild(0).GetComponent<Text>();

        for (int i = 0; i < skillBg.transform.childCount; i++)
        {
            skillUIs.Add(skillBg.transform.GetChild(i).GetComponent<SkillUI>());
        }
    }

    public void InitializeCharacterUI()
    {
        profileImage.sprite = UIResources.GetProfileSprite((CharacterType)PlayerManager.Instance.characterNum);
        for (int i = 0; i < skillUIs.Count; i++)
        {
            skillUIs[i].gameObject.SetActive(true);
            skillUIs[i].GetComponent<Image>().sprite = UIResources.GetSkillIconSprite((CharacterType)PlayerManager.Instance.characterNum, (SkillInfoType)i);
        }

        GameObject player = PlayerManager.Instance.Player;
        PlayerSkillSystem playerSkillSystem = player.GetComponent<PlayerSkillSystem>();
        PlayerBase playerBase = player.GetComponent<PlayerBase>();
        UpdateSkillLevelUpUI(playerSkillSystem.GetTotalSkillLevel(), playerSkillSystem.GetWeaponSkillLevel(),
        playerBase.playerStat.playerExp.level, playerBase.playerStat.weaponExp.level);
    }

    public void UpdatePlayerLevelUI(int playerLevel)
    {
        levelUI.text = playerLevel.ToString();
    }

    public void UpdateHpUI(float currentHp, float maxHp)
    {
        currentHp = Mathf.Ceil(currentHp);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        maxHp = Mathf.Ceil(maxHp);

        hpGauge.fillAmount = currentHp / maxHp;
        hpPulseRect.offsetMax = new Vector2(-hpGaugeRect.rect.width * (1f - hpGauge.fillAmount), hpPulseRect.offsetMax.y);
        hpText.text = $"{currentHp} / {maxHp}";
    }

    public void UpdateSpUI(float currentSp, float maxSp)
    {
        currentSp = Mathf.Ceil(currentSp);
        currentSp = Mathf.Clamp(currentSp, 0, maxSp);
        maxSp = Mathf.Ceil(maxSp);

        spGauge.fillAmount = currentSp / maxSp;
        spText.text = $"{currentSp} / {maxSp}";
    }

    public void UpdateExpUI(float currentExp, float maxExp)
    {
        expGauge.fillAmount = currentExp / maxExp;
    }

    public void UpdateSkillLevelUpUI(int totalSkillLevel, int weaponSkillLevel, int playerLevel, int weaponMasteryLevel)
    {
        if (totalSkillLevel >= playerLevel && weaponSkillLevel >= (weaponMasteryLevel - 1) / 7)
        {
            foreach (var skillUI in skillUIs)
            {
                skillUI.HideLevelUpUI();
            }
            return;
        }

        foreach (var skillUI in skillUIs)
        {
            skillUI.UpdateInteractable(totalSkillLevel, playerLevel, weaponMasteryLevel);
        }
    }
}
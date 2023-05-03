using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : SingleTonBase<PlayerUI>
{
    [SerializeField] private GameObject skillBg;
    [SerializeField] private List<SkillUI> skillUIs = new List<SkillUI>();

    [SerializeField] private Text levelUI;
    [SerializeField] private Image hpGauge;
    private RectTransform hpGaugeRect;
    private Text hpText;
    private RectTransform hpPulseRect;
    [SerializeField] private Image spGauge;
    private Text spText;
    [SerializeField] private Image expGauge;

    float chp = 10000;
    float mhp = 10000;
    float csp = 3000;
    float msp = 3000;
    float cexp = 0;
    float mexp = 1000;

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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        UpdateSkillLevelUpUI(1, 1, 3, 3);

        //chp -= Time.deltaTime * 1000;
        //csp -= Time.deltaTime * 500;
        //cexp += Time.deltaTime * 200;

        //UpdateHpUI(chp, mhp);
        //UpdateSpUI(csp, msp);
        //UpdateExpUI(cexp % mexp, mexp);
        //UpdatePlayerLevelUI((int)cexp);
    }

    public void InitializeCharacterUI()
    {
        foreach (var skillUI in skillUIs)
        {
            skillUI.gameObject.SetActive(true);
        }
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
        Debug.Log($"total {totalSkillLevel}");
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
            skillUI.UpdateInteractable(playerLevel, weaponMasteryLevel);
        }
    }
}

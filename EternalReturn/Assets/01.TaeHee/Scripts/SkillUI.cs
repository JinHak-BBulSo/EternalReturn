using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    protected int currentSkillLevel;
    protected int maxSkillLevel = 5;

    private Button button;

    [SerializeField] protected RectTransform skillLevelBgRect;
    private GameObject skillLevel;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
    }

    protected virtual void Start()
    {
        for (int i = 0; i < maxSkillLevel; i++)
        {
            GameObject skillLevelInst = Instantiate(skillLevel);
            skillLevelInst.transform.SetParent(skillLevelBgRect, false);

            RectTransform instRect = skillLevelInst.GetComponent<RectTransform>();
            instRect.sizeDelta = new Vector2((skillLevelBgRect.rect.width - 2) / maxSkillLevel, skillLevelBgRect.rect.height / 2);

        }
    }

    public virtual void SetInteractable()
    {
        button.interactable = (currentSkillLevel < maxSkillLevel) ? true : false;
    }
}

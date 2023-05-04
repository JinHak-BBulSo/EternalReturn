using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
    public SkillInfo[] skillInfos { get; private set; } = new SkillInfo[6];

    private int[] skillMaxLevels = { 5, 5, 5, 3, 2, 3 };

    private void Awake()
    {
        for (int i = 0; i < skillInfos.Length; i++)
        {
            skillInfos[i] = new SkillInfo(skillMaxLevels[i]);
        }
    }

    public int GetTotalSkillLevel()
    {
        int totalLevel = 0;
        foreach (var skillInfo in skillInfos)
        {
            totalLevel += skillInfo.CurrentLevel;
        }
        totalLevel -= skillInfos[(int)SkillInfoType.Weapon].CurrentLevel + 1;// 무기 스킬 레벨, 패시브 기본 1레벨 제외

        return totalLevel;
    }

    public int GetWeaponSkillLevel()
    {
        return skillInfos[(int)SkillInfoType.Weapon].CurrentLevel;
    }
}

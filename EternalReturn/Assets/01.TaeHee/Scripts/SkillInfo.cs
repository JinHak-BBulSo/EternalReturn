using UnityEngine;

public enum SkillInfoType
{
    Normal1 = 0,
    Normal2 = 1,
    Normal3 = 2,
    Ultimate = 3,
    Weapon = 4,
    Passive = 5
}

public class SkillInfo
{
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }
    public float cooltime;
    public float reduceCooltime;
    public float currentCooltime;
    public SkillInfo(int maxLevel_)
    {
        MaxLevel = maxLevel_;
    }

    public void AddLevel()
    {
        if (CurrentLevel >= MaxLevel)
            return;

        ++CurrentLevel;
        cooltime = cooltime - (reduceCooltime * (CurrentLevel - 1));
    }
}
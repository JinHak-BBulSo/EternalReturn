using UnityEngine;

public enum SkillInfoType
{
    Normal = 0,
    Ultimate = 3,
    Weapon = 4,
    Passive = 5
}

public class SkillInfo
{
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }

    public SkillInfo(int maxLevel_)
    {
        MaxLevel = maxLevel_;
    }

    public void AddLevel()
    {
        ++CurrentLevel;
    }
}
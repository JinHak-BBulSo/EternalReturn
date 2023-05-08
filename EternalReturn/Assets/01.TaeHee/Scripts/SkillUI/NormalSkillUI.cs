public class NormalSkillUI : SkillUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override bool IsInteractable(int totalSkillLevel, int playerLevel, int weaponMasteryLevel)
    {
        if (!base.IsInteractable(totalSkillLevel, playerLevel, weaponMasteryLevel))
            return false;

        if (skillInfo.CurrentLevel * 2 >= playerLevel)
            return false;

        return true;
    }
}
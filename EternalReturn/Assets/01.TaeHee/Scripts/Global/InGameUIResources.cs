using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InGameUIResources
{
    private const string INGAME_PREFABS_PATH = "09.InGameUI/Prefabs/";
    private const string INGAME_CHARACTER_PROFILES_PATH = "09.InGameUI/Sprite/CharacterProfiles/";
    private const string INGAME_SKILL_ICONS_PATH = "09.InGameUI/Sprite/SKillIcons/";
    private const string SKILL_LEVEL_PREFAB_NAME = "Skill Level";

    public static GameObject skillLevel = Resources.Load($"{INGAME_PREFABS_PATH}{SKILL_LEVEL_PREFAB_NAME}") as GameObject;
}

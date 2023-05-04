using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public static class InGameUIResources
{
    private const string INGAME_PREFABS_PATH = "09.InGameUI/Prefabs/";
    private const string INGAME_CHARACTER_PROFILES_PATH = "09.InGameUI/Sprite/CharacterProfiles/";
    private const string INGAME_SKILL_ICONS_PATH = "09.InGameUI/Sprite/SkillIcons/";

    private const string SKILL_LEVEL_PREFAB_NAME = "Skill Level";
    private const string PROFILE_AYA_NAME = "CharProfile_Aya_S002";
    private const string PROFILE_JACKIE_NAME = "CharProfile_Jackie_S003";

    public static readonly GameObject skillLevel;

    public static Dictionary<CharacterType, Sprite> characterProfiles;
    public static Dictionary<CharacterType, Sprite[]> characterSkillIcons;

    static InGameUIResources()
    {
        skillLevel = Resources.Load<GameObject>($"{INGAME_PREFABS_PATH}{SKILL_LEVEL_PREFAB_NAME}");

        characterProfiles = new Dictionary<CharacterType, Sprite>
        {
            { CharacterType.Aya, Resources.Load<Sprite>($"{INGAME_CHARACTER_PROFILES_PATH}{PROFILE_AYA_NAME}") },
            { CharacterType.Jackie, Resources.Load<Sprite>($"{INGAME_CHARACTER_PROFILES_PATH}{PROFILE_JACKIE_NAME}") }
        };

        characterSkillIcons = new Dictionary<CharacterType, Sprite[]>();
        Debug.Log("Enum Length" + Enum.GetValues(typeof(CharacterType)).Length);
        for (int i = 0; i < Enum.GetValues(typeof(CharacterType)).Length; i++)
        {
            Sprite[] skillIcons = new Sprite[6];
            for (int j = 0; j < skillIcons.Length; j++)
            {
                skillIcons[j] = Resources.Load<Sprite>($"{INGAME_SKILL_ICONS_PATH}{GetSkillIconID((CharacterType)i, (SkillInfoType)j)}");
            }

            characterSkillIcons.Add((CharacterType)i, skillIcons);
        }
    }

    public static Sprite GetProfileSprite(CharacterType characterType)
    {
        if (!characterProfiles.ContainsKey(characterType))
            return null;

        return characterProfiles[characterType];
    }

    public static Sprite GetSkillIconSprite(CharacterType characterType, SkillInfoType skillInfoType)
    {
        if (!characterSkillIcons.ContainsKey(characterType))
            return null;

        return characterSkillIcons[characterType][(int)skillInfoType];
    }

    private static string GetSkillIconID(CharacterType characterType, SkillInfoType skillInfoType)
    {
        return $"SkillIcon_1{((int)characterType).ToString().PadLeft(3, '0')}{((int)skillInfoType).ToString().PadLeft(3, '0')}";
    }
}

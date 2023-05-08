using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIResources
{
    private const int ITEM_BG_IMAGE_COUNT = 4;
    private const string PATH_ITEM_BG_IMAGE = "03.Item/Ico_ItemGradebg_";//+ [01, 04]

    private const string INGAME_PREFABS_PATH = "09.InGameUI/Prefabs/";
    private const string INGAME_SPRITE_PATH = "09.InGameUI/Sprite/";
    private const string INGAME_CHARACTER_PROFILES_PATH = "09.InGameUI/Sprite/CharacterProfiles/";
    private const string INGAME_SKILL_ICONS_PATH = "09.InGameUI/Sprite/SkillIcons/";

    private const string SKILL_LEVEL_PREFAB_NAME = "Skill Level";
    private const string PROFILE_AYA_NAME = "CharProfile_Aya_S002";
    private const string PROFILE_JACKIE_NAME = "CharProfile_Jackie_S003";
    private const string SPRITE_CAMERA_ROOTING_ON_NAME = "CameraRootingOn";
    private const string SPRITE_CAMERA_ROOTING_OFF_NAME = "CameraRootingOff";

    public static readonly GameObject skillLevel;

    public static Dictionary<CharacterType, Sprite> characterProfiles;
    public static Dictionary<CharacterType, Sprite[]> characterSkillIcons;

    public static ReadOnlyCollection<Sprite> ItemBgSpritesRO { get; private set; }
    private static List<Sprite> itemBgSprites = new List<Sprite>();

    public static Sprite CameraRootingOnSprite;
    public static Sprite CameraRootingOffSprite;

    static UIResources()
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

        string itemBgPath;
        for (int i = 1; i <= ITEM_BG_IMAGE_COUNT; i++)
        {
            itemBgPath = $"{PATH_ITEM_BG_IMAGE}{(i).ToString().PadLeft(2, '0')}";
            itemBgSprites.Add(Resources.Load<Sprite>(itemBgPath));
        }

        ItemBgSpritesRO = new ReadOnlyCollection<Sprite>(itemBgSprites);

        CameraRootingOnSprite = Resources.Load<Sprite>($"{INGAME_SPRITE_PATH}{SPRITE_CAMERA_ROOTING_ON_NAME}");
        CameraRootingOffSprite = Resources.Load<Sprite>($"{INGAME_SPRITE_PATH}{SPRITE_CAMERA_ROOTING_OFF_NAME}");
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

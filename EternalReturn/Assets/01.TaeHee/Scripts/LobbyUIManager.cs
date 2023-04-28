using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : SingleTonBase<LobbyUIManager>
{
    private const string NAME_WISH_LIST_SETTING_WINDOW = "WishListSetting Window";

    private Canvas canvas;
    private WishListSetting WishListSetting;

    protected override void Awake()
    {
        base.Awake();
    }
}

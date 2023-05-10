using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListSettingButton : MonoBehaviour
{
    [SerializeField] GameObject wishListSettingWindow;

    public void OnClickButton()
    {
        wishListSettingWindow.SetActive(true);
    }
}
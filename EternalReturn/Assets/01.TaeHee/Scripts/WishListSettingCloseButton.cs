using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListSettingCloseButton : MonoBehaviour
{
    [SerializeField] GameObject wishListSettingWindow;

    public void OnClickButton()
    {
        wishListSettingWindow.SetActive(false);
    }
}

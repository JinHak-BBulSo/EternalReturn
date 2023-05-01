using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListSettingCloseButton : MonoBehaviour
{
    [SerializeField] GameObject wishListSettingWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            wishListSettingWindow.SetActive(false);
        }
    }

    public void OnClickButton()
    {
        wishListSettingWindow.SetActive(false);
    }
}
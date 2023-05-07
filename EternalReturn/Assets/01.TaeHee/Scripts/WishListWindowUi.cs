using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListWindowUi : MonoBehaviour
{
    [SerializeField]
    private GameObject wishListWindow = default;
    [SerializeField]
    private ItemWishList itemWishList = default;
    void Start()
    {
        wishListWindow = transform.GetChild(0).gameObject;
        itemWishList.SetWishList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!wishListWindow.activeSelf)
            {
                wishListWindow.SetActive(true);
            }
            else
            {
                wishListWindow.SetActive(false);
            }
        }
    }
}

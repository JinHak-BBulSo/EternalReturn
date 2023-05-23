using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListWindowUi : MonoBehaviour
{
    [SerializeField]
    private GameObject wishListWindow = default;
    [SerializeField]
    private ItemWishList itemWishList = default;

    private void Awake()
    {
        wishListWindow = transform.GetChild(0).gameObject;
        wishListWindow.SetActive(true);
    }

    void Start()
    {
        itemWishList.SetWishList();
        wishListWindow.SetActive(false);
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

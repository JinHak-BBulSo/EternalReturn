using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemStat item;

    public bool isInventory = false;
    public bool isEquipment = false;
    public bool isItemWishList = false;
    public GameObject nameBg;
    public GameObject wishListImg;
    private void OnEnable()
    {
        ItemBgOn();
    }
    private void Update()
    {
        itemWishListChk();
    }

    public void itemWishListChk()
    {
        if (!isItemWishList)
        {
            wishListImg.SetActive(isItemWishList);
        }

    }
    public void ItemBgOn()
    {
        nameBg = transform.GetChild(0).gameObject;
        wishListImg = transform.GetChild(0).GetChild(0).gameObject;
        if (isInventory)
        {
            nameBg.SetActive(false);
        }
        else if (isEquipment)
        {
            nameBg.SetActive(false);
        }
        else
        {
            nameBg.SetActive(true);
            List<Item> itemInferiorList = new List<Item>();
            itemInferiorList = ItemManager.Instance.itemInferiorList;

            for (int i = 0; i < itemInferiorList.Count; i++)
            {
                if (item.id == itemInferiorList[i].item.id)
                {
                    isItemWishList = true;
                }
            }

            wishListImg.SetActive(isItemWishList);
        }
    }





}


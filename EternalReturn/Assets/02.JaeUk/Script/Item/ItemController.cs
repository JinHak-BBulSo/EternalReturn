using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemController : MonoBehaviour
{

    public ItemStat item;


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
        if (!item.isItemWishList)
        {
            wishListImg.SetActive(item.isItemWishList);
        }

    }
    public void ItemBgOn()
    {
        nameBg = transform.GetChild(0).gameObject;
        wishListImg = transform.GetChild(0).GetChild(0).gameObject;
        if (item.isInventory)
        {
            nameBg.SetActive(false);
        }
        else if (item.isEquipment)
        {
            nameBg.SetActive(false);
        }
        else
        {
            nameBg.SetActive(true);
            List<ItemStat> itemInferiorList = new List<ItemStat>();
            itemInferiorList = ItemManager.Instance.itemInferiorList;

            for (int i = 0; i < itemInferiorList.Count; i++)
            {
                if (item.id == itemInferiorList[i].id)
                {
                    item.isItemWishList = true;
                }
            }

            wishListImg.SetActive(item.isItemWishList);
        }
    }





}


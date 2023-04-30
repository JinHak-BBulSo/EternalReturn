using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemWishList : MonoBehaviour
{
    [SerializeField] private FocusedItem focusedItem;
    [SerializeField] private ItemSlot[] itemWishListSlots = new ItemSlot[6];
    private ItemStat currentFocusedItem;

    public void AddItemWishList()
    {
        currentFocusedItem = focusedItem.CurrentFocusedItem;

        if (currentFocusedItem == null || currentFocusedItem.rare < 1 || 
            currentFocusedItem.type > 18 || currentFocusedItem.type < 1)
            return;

        for (int i = 0; i < ItemManager.Instance.itemWishList.Count; i++)
        {
            if (ItemManager.Instance.itemWishList[i].type == currentFocusedItem.type)
            {
                ItemManager.Instance.itemWishList[i] = currentFocusedItem;
                UpdateItemWishListUI(currentFocusedItem);
                return;
            }
        }

        ItemManager.Instance.itemWishList.Add(currentFocusedItem);
        UpdateItemWishListUI(currentFocusedItem);
    }

    private void UpdateItemWishListUI(ItemStat item)
    {
        switch (item.type)
        {
            case (int)ItemType.Chest:
                itemWishListSlots[1].UpdateNewItem(item);
                break;
            case (int)ItemType.Head:
                itemWishListSlots[2].UpdateNewItem(item);
                break;
            case (int)ItemType.Arm:
                itemWishListSlots[3].UpdateNewItem(item);
                break;
            case (int)ItemType.Leg:
                itemWishListSlots[4].UpdateNewItem(item);
                break;
            case (int)ItemType.Accessory:
                itemWishListSlots[5].UpdateNewItem(item);
                break;
            default:
                itemWishListSlots[0].UpdateNewItem(item);
                break;
        }
    }
}

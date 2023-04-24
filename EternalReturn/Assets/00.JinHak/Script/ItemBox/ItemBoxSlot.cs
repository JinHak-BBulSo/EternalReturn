using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using static UnityEditor.Progress;

public class ItemBoxSlot : Slot
{   
    public ItemBoxSlotList slotList = default;
    public ItemStat slotItem = new ItemStat();
    public ItemStat cloneItem = new ItemStat();
    
    public void OnClick()
    {
        GetItem();
    }
    
    public void GetItem()
    {
        if (slotItem == default) return;

        List<ItemStat> inventory_ = ItemManager.Instance.inventory;
        ItemManager.Instance.GetItem(slotItem);

        slotItem.count--;

        if (slotItem.count == 0)
        {
            slotList.nowOpenItemBox.boxItems.Remove(slotItem);
            slotList.nowOpenItemBox.ResetSlot();
            slotList.nowOpenItemBox.SetSlot();
        }

        /*if (inventory_. < 10)
        {
            //ItemManager.Instance.AddItemToList(slotItem, inventory_);
            ItemManager.Instance.GetItem(slotItem);
            slotItem.count--;

            if (slotItem.count == 0)
            {
                slotList.nowOpenItemBox.boxItems.Remove(slotItem);
                slotList.nowOpenItemBox.ResetSlot();
                slotList.nowOpenItemBox.SetSlot();
            }
        }
        else
        {
            slotList.nowOpenItemBox.fullInvenTxt.SetActive(true);
        }*/

        /*bool isInInventory_ = false;
        ItemStat targetItem_ = new ItemStat();

        foreach (ItemStat inventoryItem_ in ItemManager.Instance.inventory)
        {
            if (inventoryItem_.id == slotItem.id)
            {
                isInInventory_ = true;
                targetItem_ = inventoryItem_;
            }
            else
            {
                continue;
            }
        }

        if (isInInventory_)
        {
            if (targetItem_.count < targetItem_.maxCount)
            {
                targetItem_.count++;
            }
            else
            {
                AddItemToInventory();
            }
        }
        else
        {
            AddItemToInventory();
        }*/
    }

    private void AddItemToInventory()
    {
        List<ItemStat> inventory_ = ItemManager.Instance.inventory;

        if (inventory_.Count < 10)
        {
            //ItemManager.Instance.AddItemToList(slotItem, inventory_);
            ItemManager.Instance.GetItem(slotItem);
            slotItem.count--;

            if(slotItem.count == 0)
            {
                slotList.nowOpenItemBox.boxItems.Remove(slotItem);
                slotList.nowOpenItemBox.ResetSlot();
                slotList.nowOpenItemBox.SetSlot();
            }
        }
        else
        {
            slotList.nowOpenItemBox.fullInvenTxt.SetActive(true);
        }
    }
}

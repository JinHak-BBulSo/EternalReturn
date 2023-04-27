using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxSlot : Slot
{   
    public ItemBoxSlotList slotList = default;
    public ItemStat slotItem = new ItemStat();
    public ItemStat cloneItem = new ItemStat();
    public GameObject fullInvenTxt = default;

    public void OnClick()
    {
        GetItem();
    }
    
    public void GetItem()
    {
        if (slotItem == default) return;

        fullInvenTxt.SetActive(false);
        ItemStat item = new ItemStat(slotItem);
        ItemManager.Instance.GetItem(item);

        if (!ItemManager.Instance.isInventoryFull)
        {
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
            fullInvenTxt.SetActive(true);
        }
    }
}

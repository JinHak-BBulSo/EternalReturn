using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxSlot : MonoBehaviourPun
{   
    public ItemBoxSlotList slotList = default;
    public ItemStat slotItem = new ItemStat();
    public ItemStat cloneItem = new ItemStat();
    public GameObject fullInvenTxt = default;
    public AllItemBox allItemBox = default;

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
                photonView.RPC("ItemRemove", RpcTarget.All, slotList.nowOpenItemBox.itemBoxIndex, slotItem.id);
                slotList.nowOpenItemBox.ResetSlot();
                slotList.nowOpenItemBox.SetSlot();
                //ItemRemove();
            }
            //ItemRemove();
        }
        else
        {
            fullInvenTxt.SetActive(true);
        }
    }

    [PunRPC]
    public void ItemRemove(int itemBoxIndex_, int index_)
    {
        allItemBox.allItemBoxes[itemBoxIndex_].boxItems.Remove(slotItem);
    }
}

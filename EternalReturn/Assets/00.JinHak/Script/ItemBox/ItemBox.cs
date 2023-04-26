using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    private Outline outline = default;
    private GameObject itemBoxUi = default;

    private ItemBoxSlotList slotList = default;

    public List<GameObject> itemPrefabs = new List<GameObject>();
    public List<ItemStat> boxItems = new List<ItemStat>();

    public List<ItemBoxSlotList> playerItemBoxSlotList = new List<ItemBoxSlotList>();
    public List<PlayerBase> contactPlayer = new List<PlayerBase>();

    protected virtual void Awake()
    {
        /*itemBoxUi = GameObject.Find("TestUi").transform.GetChild(1).gameObject;
        slotList = itemBoxUi.transform.GetChild(0).GetChild(4).GetComponent<ItemBoxSlotList>();*/
        outline = GetComponent<Outline>();
        SetItems();
    }

    private void SetItems()
    {
        foreach (var item in itemPrefabs)
        {
            ItemStat item_ = new ItemStat(item.GetComponent<ItemController>().item);
            ItemStat boxItem_ = item_;
            boxItems.Add(boxItem_);
        }
    }

    public void SetSlot()
    {
        int slotIndex_ = 0;
        Image slotItemImage = default;
        Text slotItemCountTxt = default;

        foreach (var slotList in playerItemBoxSlotList)
        {
            foreach (var item in boxItems)
            {
                slotItemImage = slotList.boxSlotList[slotIndex_].transform.GetChild(0).GetComponent<Image>();
                slotItemCountTxt = slotList.boxSlotList[slotIndex_].transform.GetChild(1).GetComponent<Text>();

                slotItemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;
                slotItemImage.gameObject.SetActive(true);

                slotItemCountTxt.text = item.count.ToString();
                slotItemCountTxt.gameObject.SetActive(true);

                slotList.boxSlotList[slotIndex_].slotItem = item;

                slotIndex_++;
            }

            for (int i = slotIndex_; i < slotList.boxSlotList.Count; i++)
            {
                slotList.boxSlotList[i].transform.GetChild(0).gameObject.SetActive(false);
                slotList.boxSlotList[i].transform.GetChild(1).gameObject.SetActive(false);
                slotList.boxSlotList[i].slotItem = default;
            }
        }
    }

    public void ResetSlot()
    {
        foreach (var slotList in playerItemBoxSlotList)
        {
            foreach (var slot in slotList.boxSlotList)
            {
                slot.transform.GetChild(0).gameObject.SetActive(false);
                slot.transform.GetChild(1).gameObject.SetActive(false);
                slot.slotItem = default;
            }
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerBase>().clickTarget == this.gameObject)
            {
                SetSlot();
                slotList.nowOpenItemBox = this;
            }
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            if (nowContactPlayer.clickTarget == this.gameObject)
            {
                if (!contactPlayer.Contains(nowContactPlayer))
                {
                    contactPlayer.Add(other.GetComponent<PlayerBase>());
                    playerItemBoxSlotList.Add(nowContactPlayer.itemBoxSlotList);
                    
                    SetSlot();
                    nowContactPlayer.itemBoxSlotList.nowOpenItemBox = this;
                    nowContactPlayer.itemBoxSlotList.nowOpenItemBox = this;
                    nowContactPlayer.itemBoxUi.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            if (contactPlayer.Contains(nowContactPlayer))
            {
                contactPlayer.Remove(nowContactPlayer);
                ResetSlot();

                nowContactPlayer.itemBoxSlotList.nowOpenItemBox = default;
                //Full Inven Txt 끄기
                nowContactPlayer.itemBoxUi.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                nowContactPlayer.itemBoxUi.SetActive(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if (!outline.isClick)
        {
            outline.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    private Outline outline = default;
    public int itemBoxIndex = -1;

    public List<GameObject> itemPrefabs = new List<GameObject>();
    public List<ItemStat> boxItems = new List<ItemStat>();

    public List<PlayerBase> contactedPlayer = new List<PlayerBase>();
    public ItemBoxSlotList slotList = default;

    public AudioSource itemBoxAudio = default;
    public GameObject notOpenItemBoxImg = default;

    protected virtual void Awake()
    {
        /*itemBoxUi = GameObject.Find("TestUi").transform.GetChild(1).gameObject;
        slotList = itemBoxUi.transform.GetChild(0).GetChild(4).GetComponent<ItemBoxSlotList>();*/
        outline = GetComponent<Outline>();
        itemBoxAudio = transform.parent.GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance.Player != default &&
            PlayerManager.Instance.Player.GetComponent<PlayerBase>().itemBoxUi.activeSelf)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerBase>().itemBoxUi.SetActive(false);
        }
    }

    public void SetItems()
    {
        foreach (var item in itemPrefabs)
        {
            ItemStat item_ = new ItemStat(item.GetComponent<ItemController>().item);
            boxItems.Add(item_);
        }
    }

    public void SetItems(int index_)
    {
        ItemStat item_ = new ItemStat(itemPrefabs[index_].GetComponent<ItemController>().item);
        boxItems.Add(item_);
    }

    public void AddItem(int itemIndex_)
    {
        ItemStat item_ = new ItemStat(ItemManager.Instance.itemList[itemIndex_ - 1]);
        boxItems.Add(item_);
    }

    public void SetSlot()
    {
        int slotIndex_ = 0;
        Image slotItemImage = default;
        Text slotItemCountTxt = default;
        slotList = PlayerManager.Instance.Player.GetComponent<PlayerBase>().itemBoxSlotList;

        foreach (var item in boxItems)
        {
            slotItemImage = slotList.boxSlotList[slotIndex_].slotItemImage;
            slotItemCountTxt = slotList.boxSlotList[slotIndex_].slotItemCount;

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

    public void ResetSlot()
    {
        slotList = PlayerManager.Instance.Player.GetComponent<PlayerBase>().itemBoxSlotList;
        foreach (var slot in slotList.boxSlotList)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
            slot.transform.GetChild(1).gameObject.SetActive(false);
            slot.slotItem = default;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PlayerManager.Instance.Player == other.gameObject && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            if (nowContactPlayer.clickTarget == this.gameObject)
            {
                nowContactPlayer.itemBoxSlotList.nowOpenItemBox = this;
                nowContactPlayer.itemBoxUi.SetActive(true);
                SetSlot();

                if (!contactedPlayer.Contains(nowContactPlayer))
                {
                    contactedPlayer.Add(nowContactPlayer);
                    if (notOpenItemBoxImg != default)
                    {
                        notOpenItemBoxImg.SetActive(false);
                    }
                    nowContactPlayer.GetExp(20, PlayerStat.PlayerExpType.SEARCH);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PlayerManager.Instance.Player == other.gameObject && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            ResetSlot();

            nowContactPlayer.itemBoxSlotList.nowOpenItemBox = default;
            //Full Inven Txt 끄기
            nowContactPlayer.itemBoxUi.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            nowContactPlayer.itemBoxUi.SetActive(false);
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

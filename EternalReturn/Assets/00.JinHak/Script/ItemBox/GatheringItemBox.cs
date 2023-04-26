using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatheringItemBox : MonoBehaviour
{
    private Outline outline = default;

    public GameObject itemPrefab = default;
    public ItemStat gatheringItem = new ItemStat();

    void Start()
    {
        outline = GetComponent<Outline>();
        gatheringItem = itemPrefab.GetComponent<ItemController>().item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerStay(Collider other)
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
                    slotList.nowOpenItemBox = this;
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
    IEnumerator GatheringItem()
    {

    }*/
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

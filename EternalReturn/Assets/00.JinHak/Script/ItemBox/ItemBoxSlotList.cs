using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxSlotList : MonoBehaviour
{
    public List<ItemBoxSlot> boxSlotList = new List<ItemBoxSlot>();
    public ItemBox nowOpenItemBox = default;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            boxSlotList.Add(transform.GetChild(i).GetComponent<ItemBoxSlot>());
            boxSlotList[i].slotList = this;
        }

        transform.parent.parent.gameObject.SetActive(false);
    }
}

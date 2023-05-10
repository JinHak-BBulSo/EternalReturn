using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    public bool chk = false;
    public Vector3 pos = default;
    public int myNum;
    public override void UseItem(int i)
    {
        Debug.Log("!!");
        if (ItemManager.Instance.equipmentInven[i].id == 0)
        {

        }
        else
        {

            ItemStat itemDefault = new ItemStat();
            ItemManager.Instance.PickItem(ItemManager.Instance.equipmentInven[i]);
            ItemManager.Instance.equipmentInven[i] = itemDefault;
            ItemManager.Instance.isItemPick = true;


        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!chk)
        {
            UseItem(myNum);
        }

    }
    public void OnDrag(PointerEventData eventData)
    {

        chk = true;
        transform.GetChild(1).position = eventData.position;



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        UseItem(myNum);
        transform.GetChild(1).position = transform.position;
        chk = false;


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        pos = transform.GetChild(1).position;
    }
}


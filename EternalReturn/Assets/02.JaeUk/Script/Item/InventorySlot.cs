using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{

    public bool chk = false;
    public Vector3 pos = default;
    public int myNum;
    public override void Start()
    {
        base.Start();
    }
    // Start is called before the first frame update
    public override void UseItem(int i)
    {
        if (!chk)
        {
            if (ItemManager.Instance.inventory.Count < i + 1)
            {

            }
            else
            {
                if (ItemManager.Instance.inventory[i].type == ItemManager.Instance.Player.weaponType)
                {
                    Switching(i, 0);
                }
                else
                {
                    switch (ItemManager.Instance.inventory[i].type)
                    {

                        case 14:
                            Switching(i, 2);
                            break;
                        case 15:
                            Switching(i, 1);
                            break;
                        case 16:
                            Switching(i, 3);
                            break;
                        case 17:
                            Switching(i, 4);
                            break;
                        case 18:
                            Switching(i, 5);
                            break;
                        case 19:
                            break;
                        case 20:
                            break;
                        default:
                            break;
                    }
                }
                chk = false;
                ItemManager.Instance.isEquipmentChang = true;
            }

        }
        else
        {
            Debug.Log("Active");

        }



    }
    public void Switching(int order, int idx)
    {
        if (ItemManager.Instance.equipmentInven[idx].id != 0)
        {
            (ItemManager.Instance.inventory[order], ItemManager.Instance.equipmentInven[idx]) =
                           (ItemManager.Instance.equipmentInven[idx], ItemManager.Instance.inventory[order]);
        }
        else
        {
            ItemManager.Instance.equipmentInven[idx] = ItemManager.Instance.inventory[order];
            ItemManager.Instance.inventory[order] = new ItemStat();
        }

        ItemManager.Instance.SetequipmentTotalState();
        ItemManager.Instance.isItemPick = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(chk);
        if (!chk)
        {
            UseItem(myNum);
        }

    }
    public void OnDrag(PointerEventData eventData)
    {

        chk = true;
        transform.GetChild(0).position = eventData.position;



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector3.Distance(eventData.position, pos) >= 70f)
        {
            ItemManager.Instance.DropItem(ItemManager.Instance.itemList[ItemManager.Instance.inventory[myNum].id - 1], ItemManager.Instance.Player.gameObject, ItemManager.Instance.ItemCanvas);
            if (ItemManager.Instance.inventory[myNum].count > 1)
            {
                ItemManager.Instance.inventory[myNum].count--;
            }
            else if (ItemManager.Instance.inventory[myNum].count == 1)
            {
                ItemManager.Instance.inventory[myNum] = new ItemStat();
            }
            ItemManager.Instance.InventoryChange();
        }

        transform.GetChild(0).position = transform.position;
        chk = false;


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        pos = transform.GetChild(0).position;
    }
}

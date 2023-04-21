using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : Slot
{

    public override void Start()
    {
        base.Start();
        playerWeaponType = 1;
    }
    // Start is called before the first frame update
    public override void Onclick(int i)
    {
        Debug.Log("!!");
        if (ItemManager.Instance.inventory.Count < i + 1)
        {

        }
        else
        {
            if (ItemManager.Instance.inventory[i].type == playerWeaponType)
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
            ItemManager.Instance.inventory.RemoveAt(order);
        }

        ItemManager.Instance.SetequipmentTotalState();
        ItemManager.Instance.isItemPick = true;
    }
}

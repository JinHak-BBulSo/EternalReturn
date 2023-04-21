using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : Slot
{
    // Start is called before the first frame update
    public override void Onclick(int i)
    {
        Debug.Log("!!");
        if (ItemManager.Instance.equipmentInven[i].id == 0)
        {

        }
        else
        {
            if (ItemManager.Instance.inventory.Count < 10)
            {
                ItemStat itemDefault = new ItemStat();
                ItemManager.Instance.inventory.Add(ItemManager.Instance.equipmentInven[i]);
                ItemManager.Instance.equipmentInven[i] = itemDefault;
                ItemManager.Instance.SetequipmentTotalState();
                ItemManager.Instance.isItemPick = true;
            }

        }

    }
}


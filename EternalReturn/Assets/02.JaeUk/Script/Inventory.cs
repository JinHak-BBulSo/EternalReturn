using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SlotList
{
    // Start is called before the first frame update
    public GameObject[] ItemSlot;
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void Addslot(int id)
    {
        if (ItemManager.Instance.isItemPick)
        {
            ItemManager.Instance.isItemPick = false;
            inventory = ItemManager.Instance.inventory;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].item.id == id && inventory[i].item.maxCount > inventory[i].item.count)
                {
                    inventory[i].item.count++;
                }
                else if (inventory.Count == 10)
                {

                }
                else
                {
                    inventory.Add(itemListClone[id]);
                }
            }
        }

    }
}

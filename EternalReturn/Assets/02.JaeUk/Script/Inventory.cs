using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Addslot();
    }
    public override void Addslot()
    {
        if (ItemManager.Instance.isItemPick)
        {
            ItemManager.Instance.isItemPick = false;
            inventory = ItemManager.Instance.inventory;
            for (int i = 0; i < ItemSlot.Length; i++)
            {
                ItemSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = default;
                ItemSlot[i].transform.GetChild(0).gameObject.SetActive(false);
                ItemSlot[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            for (int i = 0; i < inventory.Count; i++)
            {

                ItemSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.itemListObj[inventory[i].id].GetComponent<SpriteRenderer>().sprite;
                ItemSlot[i].transform.GetChild(1).GetComponent<Text>().text = inventory[i].count.ToString();
                ItemSlot[i].transform.GetChild(0).gameObject.SetActive(true);
                ItemSlot[i].transform.GetChild(1).gameObject.SetActive(true);

            }

        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WishList : SlotList
{
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
            List<ItemStat> wishList = ItemManager.Instance.itemWishList;
            for (int i = 0; i < ItemSlot.Length; i++)
            {
                ItemSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = default;
                ItemSlot[i].transform.GetChild(1).gameObject.SetActive(false);
                ItemSlot[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int i = 0; i < wishList.Count; i++)
            {
                ItemSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = ItemManager.Instance.itemListObj[wishList[i].id].GetComponent<SpriteRenderer>().sprite;
                ItemSlot[i].transform.GetChild(1).gameObject.SetActive(true);
                switch (itemListClone[wishList[i].id].rare)
                {
                    case 1:
                        ItemSlot[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.3f, 0.43f, 0.29f, 1f);
                        break;
                    case 2:
                        ItemSlot[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.17f, 0.22f, 0.36f, 1f);
                        break;
                    case 3:
                        ItemSlot[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.38f, 0.27f, 0.49f, 1f);
                        break;
                }
                ItemSlot[i].transform.GetChild(0).gameObject.SetActive(true);


            }

        }

    }
}

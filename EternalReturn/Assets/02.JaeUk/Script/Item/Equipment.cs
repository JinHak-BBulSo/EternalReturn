using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Equipment : SlotList
{
    public GameObject[] ItemSlot;
    public Sprite[] defaultImg;
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
            List<ItemStat> equipment = ItemManager.Instance.equipmentInven;
            for (int i = 0; i < ItemSlot.Length; i++)
            {
                ItemSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = defaultImg[i];
                ItemSlot[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int i = 0; i < equipment.Count; i++)
            {
                if (equipment[i] != default && equipment[i].id != 0)
                {
                    ItemSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = ItemManager.Instance.itemListObj[equipment[i].id].GetComponent<SpriteRenderer>().sprite;
                    ItemSlot[i].transform.GetChild(1).gameObject.SetActive(true);
                    switch (itemListClone[equipment[i].id].rare)
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
                else
                {
                    ItemSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = defaultImg[i];
                }



            }

        }

    }
}

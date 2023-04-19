using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCombine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                AddAbleCombineList(ItemManager.Instance.inventory[i], ItemManager.Instance.itemCombineDictionary);
            }
            for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
            {
                AddAbleCombineList(ItemManager.Instance.equipmentInven[i], ItemManager.Instance.itemCombineDictionary);

            }




        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (ItemManager.Instance.combineAbleList.Count != 0)
            {
                CombineItem(ItemManager.Instance.combineAbleList[0], ItemManager.Instance.itemCombineDictionary);
                StartCoroutine(Delay());


            }

        }
    }
    IEnumerator Delay()
    {
        DeleteInferiorList(ItemManager.Instance.combineAbleList[0]);
        yield return new WaitForSeconds(1f);
        ItemManager.Instance.combineAbleList.RemoveAt(0);
    }
    public void DeleteInferiorList(Item item)
    {
        int count = 0;
        switch (item.item.rare)
        {
            case 0:

                break;
            case 1:
                for (int i = 0; i < ItemManager.Instance.ItemInferiorUncommon.Count; i++)
                {
                    Item tier1 = ItemManager.Instance.ItemInferiorUncommon[i];
                    if (tier1.item.id == item.item.id)
                    {
                        Debug.Log("!!");
                        ItemManager.Instance.DeleteInferiorList(item.item.id);
                    }
                }
                break;
            case 2:
                foreach (Item tier2 in ItemManager.Instance.ItemInferiorRare)
                {

                    if (tier2.item.id == item.item.id)
                    {
                        ItemManager.Instance.DeleteInferiorList(item.item.id);
                    }
                    count++;
                }
                break;
            case 3:
                foreach (Item tier3 in ItemManager.Instance.itemWishList)
                {

                    if (tier3.item.id == item.item.id)
                    {
                        ItemManager.Instance.itemWishList.RemoveAt(count);
                        ItemManager.Instance.DeleteInferiorList(item.item.id);
                    }
                    count++;
                }
                break;
        }
    }
    public void CombineItem(Item item, Dictionary<ItemDefine, int> dic)
    {
        int[] needItem = ItemManager.Instance.ADDCombineItemToInven(item.item.id);
        bool idchk1 = false;
        bool idchk2 = false;
        List<int> itemSlot1 = new List<int>();
        List<int> itemSlot2 = new List<int>();
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {
            if (ItemManager.Instance.equipmentInven[i].item.id == needItem[0])
            {
                itemSlot1.Add(i);
                idchk1 = true;
            }
            else if (ItemManager.Instance.equipmentInven[i].item.id == needItem[1])
            {
                itemSlot1.Add(i);
                idchk2 = true;
            }
        }


        if (!idchk1 || !idchk2)
        {

            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                if (ItemManager.Instance.inventory[i].item.id == needItem[0])
                {
                    itemSlot2.Add(i);
                    idchk1 = true;
                }
                else if (ItemManager.Instance.inventory[i].item.id == needItem[1])
                {
                    itemSlot2.Add(i);
                    idchk2 = true;
                }
            }

        }
        if (idchk1 && idchk2)
        {
            for (int i = itemSlot1.Count - 1; i >= 0; i--)
            {
                if (ItemManager.Instance.inventory[itemSlot1[i]].item.count <= 1)
                {
                    ItemManager.Instance.inventory.RemoveAt(itemSlot1[i]);
                }
                else
                {
                    ItemManager.Instance.inventory[itemSlot1[i]].item.count--;
                }

            }
            for (int i = itemSlot2.Count - 1; i >= 0; i--)
            {
                if (ItemManager.Instance.equipmentInven[itemSlot2[i]].item.count <= 1)
                {
                    ItemManager.Instance.equipmentInven.RemoveAt(itemSlot2[i]);
                }
                else
                {
                    ItemManager.Instance.equipmentInven[itemSlot2[i]].item.count--;
                }
            }


            ItemManager.Instance.inventory.Add(ItemManager.Instance.itemList[item.item.id - 1]);
        }



    }
    public void AddAbleCombineList(Item item, Dictionary<ItemDefine, int> dic)
    {
        bool isEquipment = false;
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {
            ItemDefine define = new ItemDefine(item.item.id, ItemManager.Instance.equipmentInven[i].GetComponent<Item>().item.id);

            if (define.itemId_2 >= define.itemId_1)
            {
                if (define.IsitemCombineValue(dic))
                {
                    if (define.GetitemCombineValue(dic) == null)
                    {

                    }
                    else
                    {
                        if (!ItemManager.Instance.combineAbleList.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                        {
                            ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                            isEquipment = true;
                        }
                    }


                }

            }
            else
            {
                (define.itemId_1, define.itemId_2) = (define.itemId_2, define.itemId_1);
                if (define.IsitemCombineValue(dic))
                {
                    if (define.GetitemCombineValue(dic) == null)
                    {

                    }
                    else
                    {
                        if (!ItemManager.Instance.combineAbleList.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                        {
                            ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                            isEquipment = true;
                        }

                    }


                }
            }
        }
        if (isEquipment)
        {
            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                ItemDefine define = new ItemDefine(item.item.id, ItemManager.Instance.inventory[i].GetComponent<Item>().item.id);

                if (define.itemId_2 >= define.itemId_1)
                {
                    if (define.IsitemCombineValue(dic))
                    {
                        if (define.GetitemCombineValue(dic) == null)
                        {

                        }
                        else
                        {
                            if (!ItemManager.Instance.combineAbleList.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                            {
                                ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                            }
                        }


                    }

                }
                else
                {
                    (define.itemId_1, define.itemId_2) = (define.itemId_2, define.itemId_1);
                    if (define.IsitemCombineValue(dic))
                    {
                        if (define.GetitemCombineValue(dic) == null)
                        {

                        }
                        else
                        {
                            if (!ItemManager.Instance.combineAbleList.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                            {
                                ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                            }

                        }


                    }
                }
            }
        }


    }
}

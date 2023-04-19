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

            // AddAbleCombineList(ItemManager.Instance.itemList[111], ItemManager.Instance.itemCombineDictionary);
            AddAbleCombineList(ItemManager.Instance.itemList[111], ItemManager.Instance.itemCombineDictionary);

        }
    }
    public void CombineItem(Item item, Dictionary<ItemDefine, int> dic)
    {


        for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
        {
            ItemDefine define = new ItemDefine(item.item.id, ItemManager.Instance.inventory[i].GetComponent<Item>().item.id);

            if (define.itemId_2 >= define.itemId_1)
            {
                if (define.IsitemCombineValue(dic))
                {
                    Debug.Log(define.itemId_1);
                    Debug.Log(define.itemId_2);
                    if (ItemManager.Instance.inventory.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                    {
                        ItemManager.Instance.inventory[ItemManager.Instance.inventory.IndexOf(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1])].item.count++;
                    }
                    else
                    {

                    }
                    ItemManager.Instance.inventory.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                }

            }
            else
            {
                (define.itemId_1, define.itemId_2) = (define.itemId_2, define.itemId_1);
                if (define.IsitemCombineValue(dic))
                {
                    if (ItemManager.Instance.inventory.Contains(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]))
                    {
                        ItemManager.Instance.inventory[ItemManager.Instance.inventory.IndexOf(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1])].item.count++;
                    }
                    else
                    {

                    }
                    ItemManager.Instance.inventory.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                }
            }
        }

    }
    public void AddAbleCombineList(Item item, Dictionary<ItemDefine, int> dic)
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
                        ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
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
                        ItemManager.Instance.combineAbleList.Add(ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1]);
                    }


                }
            }
        }

    }
}

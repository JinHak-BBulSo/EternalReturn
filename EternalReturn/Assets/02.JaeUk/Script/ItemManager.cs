using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTonBase<ItemManager>
{
    private ItemDefine itemDefine = new ItemDefine();
    public List<Item> itemList = new List<Item>();
    [SerializeField]
    public Dictionary<ItemDefine, int> itemCombineDictionary = new Dictionary<ItemDefine, int>();
    public List<ItemStat> itemWishList = new List<ItemStat>();
    public List<ItemStat> itemInferiorList = new List<ItemStat>();
    public List<Item> combineAbleList = new List<Item>();
    public List<Item> inventory = new List<Item>();


    protected override void Awake()
    {
        base.Awake();
    }

    public int[] AddInferiorList(int ItemId)
    {
        int[] forReturn = new int[2];
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_2;
        if (itemList[id_1 - 1].GetComponent<Item>().item.rare == 0 && itemList[id_2 - 1].GetComponent<Item>().item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_1 - 1].item))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1].item)].count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_1 - 1].item);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1].item)].count++;
            }
            if (itemInferiorList.Contains(itemList[id_2 - 1].item))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1].item)].count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_2 - 1].item);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1].item)].count++;
            }
            return forReturn;
        }
        else if (itemList[id_1 - 1].GetComponent<Item>().item.rare >= 1 && itemList[id_2 - 1].GetComponent<Item>().item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_2 - 1].item))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1].item)].count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_2 - 1].item);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1].item)].count++;
            }
            return AddInferiorList(itemList[id_1 - 1].GetComponent<Item>().item.id);
        }
        else if (itemList[id_2 - 1].GetComponent<Item>().item.rare >= 1 && itemList[id_1 - 1].GetComponent<Item>().item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_1 - 1].item))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1].item)].count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_1 - 1].item);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1].item)].count++;
            }
            return AddInferiorList(itemList[id_2 - 1].GetComponent<Item>().item.id);
        }
        else
        {
            AddInferiorList(itemList[id_2 - 1].GetComponent<Item>().item.id);
            AddInferiorList(itemList[id_1 - 1].GetComponent<Item>().item.id);
            return forReturn;
        }

    }

}

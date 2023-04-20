using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : SingleTonBase<ItemManager>
{
    public ItemDefine itemDefine = new ItemDefine();
    public Dictionary<int, GameObject> itemListObj = new Dictionary<int, GameObject>();
    public List<Item> itemList = new List<Item>();
    public Dictionary<ItemDefine, int> itemCombineDictionary = new Dictionary<ItemDefine, int>();
    public List<Item> itemWishList = new List<Item>();
    public List<Item> itemInferiorList = new List<Item>();
    public List<Item> combineAbleList = new List<Item>();
    public List<Item> inventory = new List<Item>();
    public List<Item> equipmentInven = new List<Item>();
    public bool isItemDrop = true;
    public bool isItemPick = true;


    protected override void Awake()
    {
        AddPrefabs();
        base.Awake();

    }

    public int[] AddInferiorList(int ItemId)
    {
        int[] forReturn = new int[2];
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_2;
        if (itemList[id_1 - 1].item.rare == 0 && itemList[id_2 - 1].item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_1 - 1]))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1])].item.count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_1 - 1]);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1])].item.count++;
            }
            if (itemInferiorList.Contains(itemList[id_2 - 1]))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1])].item.count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_2 - 1]);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1])].item.count++;
            }
            return forReturn;
        }
        else if (itemList[id_1 - 1].item.rare >= 1 && itemList[id_2 - 1].item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_2 - 1]))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1])].item.count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_2 - 1]);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_2 - 1])].item.count++;
            }
            return AddInferiorList(itemList[id_1 - 1].item.id);
        }
        else if (itemList[id_2 - 1].item.rare >= 1 && itemList[id_1 - 1].item.rare == 0)
        {
            if (itemInferiorList.Contains(itemList[id_1 - 1]))
            {
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1])].item.count++;
            }
            else
            {
                itemInferiorList.Add(itemList[id_1 - 1]);
                itemInferiorList[itemInferiorList.IndexOf(itemList[id_1 - 1])].item.count++;
            }
            return AddInferiorList(itemList[id_2 - 1].item.id);
        }
        else
        {
            AddInferiorList(itemList[id_2 - 1].item.id);
            AddInferiorList(itemList[id_1 - 1].item.id);
            return forReturn;
        }

    }
    public void AddPrefabs()
    {
        List<GameObject> item = Resources.LoadAll<GameObject>("03.Item/Prefabs").ToList();

        int itemKey = -1;

        for (int i = 0; i < item.Count; i++)
        {
            itemKey = int.Parse(item[i].name.Split(".")[0]);
            itemListObj.Add(itemKey, item[i]);
        }


        for (int i = 1; i < itemListObj.Count + 1; i++)
        {
            if (itemListObj[i] == null)
            {

            }
            else
            {
                itemList.Add(itemListObj[i].GetComponent<Item>());
            }

        }
    }



}

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
    public List<Item> ItemInferiorRare = new List<Item>();
    public List<Item> ItemInferiorUncommon = new List<Item>();
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
    public int[] ADDCombineItemToInven(int id)
    {
        int[] retrunId = new int[2];
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, id).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, id).itemId_2;
        retrunId[0] = id_1;
        retrunId[1] = id_2;
        return retrunId;
    }

    public int[] DeleteInferiorList(int ItemId)
    {
        int[] forReturn = new int[2];
        Item item_base = ItemManager.Instance.itemList[ItemId - 1];
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_2;
        Item item_1 = itemList[id_1 - 1];
        Item item_2 = itemList[id_2 - 1];
        if (item_1.item.rare == 0 && item_2.item.rare == 0)
        {
            DeleteItemToList(item_1, itemInferiorList);
            DeleteItemToList(item_2, itemInferiorList);
            ItemManager.Instance.ItemInferiorUncommon.RemoveAt(ItemManager.Instance.ItemInferiorUncommon.IndexOf(item_base));

            return forReturn;
        }
        else if (itemList[id_1 - 1].item.rare >= 1 && itemList[id_2 - 1].item.rare == 0)
        {
            ItemManager.Instance.ItemInferiorUncommon.RemoveAt(ItemManager.Instance.ItemInferiorRare.IndexOf(item_base));
            DeleteItemToList(item_2, itemInferiorList);
            DeleteItemToRare(item_1);

            return DeleteInferiorList(id_1);
        }
        else if (itemList[id_2 - 1].item.rare >= 1 && itemList[id_1 - 1].item.rare == 0)
        {
            DeleteItemToList(item_1, itemInferiorList);
            DeleteItemToRare(item_2);

            return DeleteInferiorList(id_2);
        }
        else
        {
            DeleteItemToList(item_1, itemInferiorList);
            DeleteItemToList(item_2, itemInferiorList);
            DeleteItemToRare(item_1);
            DeleteItemToRare(item_2);
            return forReturn;
        }
    }
    public int[] AddInferiorList(int ItemId)
    {
        int[] forReturn = new int[2];
        // 하위 아이템 id 값을 가져오는 변수 선언
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_2;
        Item item_1 = itemList[id_1 - 1];
        Item item_2 = itemList[id_2 - 1];

        if (item_1.item.rare == 0 && item_2.item.rare == 0)
        {
            AddItemToList(item_1, itemInferiorList);
            AddItemToList(item_2, itemInferiorList);

            return forReturn;
        }
        else if (itemList[id_1 - 1].item.rare >= 1 && itemList[id_2 - 1].item.rare == 0)
        {
            AddItemToList(item_2, itemInferiorList);
            AddItemToRare(item_1);

            return AddInferiorList(id_1);
        }
        else if (itemList[id_2 - 1].item.rare >= 1 && itemList[id_1 - 1].item.rare == 0)
        {
            AddItemToList(item_1, itemInferiorList);
            AddItemToRare(item_2);

            return AddInferiorList(id_2);
        }
        else
        {
            AddItemToRare(item_1);
            AddItemToRare(item_2);
            AddInferiorList(id_1);
            AddInferiorList(id_2);

            return forReturn;
        }
    }

    //! 아이템 리스트에서 아이템을 찾아오는 함수
    private Item FindItemFromList(Item item_, List<Item> itemlist_)
    {
        Item targetItem_ = default;
        targetItem_ = itemlist_.Find(
                (matchItem_) =>
                {
                    return matchItem_.item.id.Equals(item_.item.id);
                });
        // Debug.Log($"Find Func Test : {item_.item.name}, {targetItem_.item.name}");
        return targetItem_;
    }

    //! 아이템 리스트와 아이템을 받아서 아이템 리스트에 추가하거나 더해주는 함수
    private void AddItemToList(Item item_, List<Item> itemlist_)
    {
        // Debug.LogFormat("AddItem to list -> {0}, {1}",
        // item_.item.name, item_.item.id);

        if (itemlist_.Contains(item_))
        {
            Item targetItem_ = FindItemFromList(item_, itemlist_);
            if (targetItem_ != default)
            {
                targetItem_.item.count++;
                // Debug.LogFormat("Item exist! count add: {0}, {1}, {2}",
                // targetItem_.item.name, targetItem_.item.id, targetItem_.item.count);
            }
        }
        else
        {
            item_.item.count++;
            itemlist_.Add(item_);

            // Debug.LogFormat("Item add to list: {0}, {1}, {2}",
            // item_.item.name, item_.item.id, item_.item.count);
        }
    }       // AddItemToList()
    private void DeleteItemToList(Item item_, List<Item> itemlist_)
    {
        // Debug.LogFormat("AddItem to list -> {0}, {1}",
        // item_.item.name, item_.item.id);

        if (itemlist_.Contains(item_))
        {
            Item targetItem_ = FindItemFromList(item_, itemlist_);
            if (targetItem_ != default)
            {
                if (targetItem_.item.count <= 1)
                {
                    Debug.Log(itemList.IndexOf(targetItem_));
                    itemList.RemoveAt(itemList.IndexOf(targetItem_));

                }
                else
                {
                    targetItem_.item.count--;
                }


            }
        }


    }       // AddItemToList()
    private void DeleteItemToRare(Item item_)
    {
        switch (item_.item.rare)
        {
            case 1:
                // Debug.LogFormat("Rare 1 add item: {0}", item_.item.name);
                DeleteItemToList(item_, ItemInferiorUncommon);
                break;
            case 2:
                // Debug.LogFormat("Rare 2 add item: {0}", item_.item.name);
                DeleteItemToList(item_, ItemInferiorRare);
                break;
            default: break;
        }
    }

    private void AddItemToRare(Item item_)
    {
        switch (item_.item.rare)
        {
            case 1:
                // Debug.LogFormat("Rare 1 add item: {0}", item_.item.name);
                AddItemToList(item_, ItemInferiorUncommon);
                break;
            case 2:
                // Debug.LogFormat("Rare 2 add item: {0}", item_.item.name);
                AddItemToList(item_, ItemInferiorRare);
                break;
            default: break;
        }
    }       // AddItemToRare()
            //@Legacy
            // //! Target item의 id 와 아이템 리스트에서 찾은 item 의 id 가 같은지 비교하는 Compare 함수
            // private bool CompareItemFromList(Item item_, List<Item> itemlist_)
            // {
            //     foreach (Item searchItem_ in itemlist_)
            //     {
            //         if (searchItem_.item.id.Equals(item_.item.id))
            //         {

    //             return true;
    //         }
    //         else { continue; }
    //     }

    //     return false;
    // }

    //! 리소스 파일에 있는 프리펩들을 순서대로 정렬해서 저장하기 위한 함수
    public void AddPrefabs()
    {
        List<GameObject> item = Resources.LoadAll<GameObject>("03.Item/Prefabs").ToList();

        int itemKey = -1;
        //가져온 아이템 프리펩의 개수 만큼 순서대로 딕셔너리 정렬
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

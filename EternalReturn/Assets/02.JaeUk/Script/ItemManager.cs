using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : SingleTonBase<ItemManager>
{
    public ItemDefine itemDefine = new ItemDefine();
    public Dictionary<int, GameObject> itemListObj = new Dictionary<int, GameObject>();
    public List<ItemStat> itemList = new List<ItemStat>();
    public Dictionary<ItemDefine, int> itemCombineDictionary = new Dictionary<ItemDefine, int>();
    public List<ItemStat> itemWishList = new List<ItemStat>();
    public List<ItemStat> itemInferiorList = new List<ItemStat>();
    public List<ItemStat> ItemInferiorRare = new List<ItemStat>();
    public List<ItemStat> ItemInferiorUncommon = new List<ItemStat>();
    public List<ItemStat> combineAbleList = new List<ItemStat>();
    public List<ItemStat> inventory = new List<ItemStat>();
    public List<ItemStat> equipmentInven = new List<ItemStat>();
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
        ItemStat item_base = ItemManager.Instance.itemList[ItemId - 1];
        int id_1 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(itemCombineDictionary, ItemId).itemId_2;
        ItemStat item_1 = new ItemStat(itemList[id_1 - 1]);
        ItemStat item_2 = new ItemStat(itemList[id_2 - 1]);
        DeleteItemToRare(itemList[ItemId - 1]);
        if (item_1.rare == 0 && item_2.rare == 0)
        {
            DeleteItemToList(item_1, itemInferiorList);
            DeleteItemToList(item_2, itemInferiorList);
            // ItemManager.Instance.ItemInferiorUncommon.RemoveAt(ItemManager.Instance.ItemInferiorUncommon.IndexOf(item_base));

            return forReturn;
        }
        else if (itemList[id_1 - 1].rare >= 1 && itemList[id_2 - 1].rare == 0)
        {
            // ItemManager.Instance.ItemInferiorUncommon.RemoveAt(ItemManager.Instance.ItemInferiorRare.IndexOf(item_base));
            DeleteItemToList(item_2, itemInferiorList);
            DeleteItemToRare(item_1);

            return DeleteInferiorList(id_1);
        }
        else if (itemList[id_2 - 1].rare >= 1 && itemList[id_1 - 1].rare == 0)
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
        ItemStat item_1 = new ItemStat(itemList[id_1 - 1]);
        ItemStat item_2 = new ItemStat(itemList[id_2 - 1]);

        if (item_1.rare == 0 && item_2.rare == 0)
        {
            AddItemToList(item_1, itemInferiorList);
            AddItemToList(item_2, itemInferiorList);

            return forReturn;
        }
        else if (itemList[id_1 - 1].rare >= 1 && itemList[id_2 - 1].rare == 0)
        {
            AddItemToList(item_2, itemInferiorList);
            AddItemToRare(item_1);

            return AddInferiorList(id_1);
        }
        else if (itemList[id_2 - 1].rare >= 1 && itemList[id_1 - 1].rare == 0)
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
    private ItemStat FindItemFromList(ItemStat item_, List<ItemStat> itemlist_)
    {
        ItemStat targetItem_ = default;
        targetItem_ = itemlist_.Find(
                (matchItem_) =>
                {
                    return matchItem_.id.Equals(item_.id);
                });
        // Debug.Log($"Find Func Test : {item_.name}, {targetItem_.name}");
        return targetItem_;
    }

    //! 아이템 리스트와 아이템을 받아서 아이템 리스트에 추가하거나 더해주는 함수
    public void AddItemToList(ItemStat item_, List<ItemStat> itemlist_)
    {
        bool isChk = false;
        foreach (ItemStat id in itemlist_.ToList())
        {
            if (id.id == item_.id)
            {
                isChk = true;
            }
            else
            {
                continue;
            }
        }


        if (isChk)
        {
            ItemStat targetItem_ = FindItemFromList(item_, itemlist_);
            if (targetItem_ != default)
            {
                targetItem_.count++;
                // Debug.LogFormat("Item exist! count add: {0}, {1}, {2}",
                // targetItem_.item.name, targetItem_.item.id, targetItem_.item.count);
            }
        }
        else
        {
            itemlist_.Add(item_);

            // Debug.LogFormat("Item add to list: {0}, {1}, {2}",
            // item_.item.name, item_.item.id, item_.item.count);
        }
    }       // AddItemToList()
    private void DeleteItemToList(ItemStat item_, List<ItemStat> itemlist_)
    {
        bool isChk = false;
        // Debug.LogFormat("AddItem to list -> {0}, {1}",
        foreach (ItemStat id in itemlist_.ToList())
        {
            if (id.id == item_.id)
            {
                isChk = true;
            }
            else
            {
                continue;
            }
        }
        // item_.item.name, item_.item.id);


        if (isChk)
        {
            ItemStat targetItem_ = FindItemFromList(item_, itemlist_);
            if (targetItem_ != default)
            {
                if (targetItem_.count <= 1)
                {
                    // Debug.Log(itemInferiorList.IndexOf(targetItem_));
                    // Debug.Log(targetItem_.item.name);
                    itemlist_.RemoveAt(itemlist_.IndexOf(targetItem_));

                }
                else
                {
                    targetItem_.count--;
                }


            }
        }




    }       // AddItemToList()
    private void DeleteItemToRare(ItemStat item_)
    {

        switch (item_.rare)
        {
            case 1:
                // Debug.LogFormat("Rare 1 add item: {0}", item_.item.name);
                for (int i = 0; i < ItemInferiorUncommon.Count; i++)
                {
                    if (ItemInferiorUncommon[i].id == item_.id)
                    {
                        DeleteItemToList(item_, ItemInferiorUncommon);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < ItemInferiorRare.Count; i++)
                {
                    Debug.Log(ItemInferiorRare[i].id);
                    if (ItemInferiorRare[i].id == item_.id)
                    {
                        DeleteItemToList(item_, ItemInferiorRare);
                    }
                }

                break;
            case 3:
                for (int i = 0; i < itemWishList.Count; i++)
                {
                    if (itemWishList[i].id == item_.id)
                    {
                        DeleteItemToList(item_, itemWishList);
                    }
                }

                break;
            default: break;
        }
    }

    private void AddItemToRare(ItemStat item_)
    {
        switch (item_.rare)
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


        for (int i = 1; i < itemListObj.Count; i++)
        {
            if (itemListObj[i] == null)
            {

            }
            else
            {
                itemList.Add(itemListObj[i].GetComponent<ItemController>().item);
            }

        }
    }


    public void EquipmentListSet(ItemStat item)
    {
        switch (item.type)
        {
            case 1:
                equipmentInven[0] = item;
                break;
            case 2:
                equipmentInven[0] = item;
                break;
            case 3:
                equipmentInven[0] = item;
                break;
            case 4:
                equipmentInven[0] = item;
                break;
            case 5:
                equipmentInven[0] = item;
                break;
            case 6:
                equipmentInven[0] = item;
                break;
            case 7:
                equipmentInven[0] = item;
                break;
            case 8:
                equipmentInven[0] = item;
                break;
            case 9:
                equipmentInven[0] = item;
                break;
            case 10:
                equipmentInven[0] = item;
                break;
            case 11:
                equipmentInven[0] = item;
                break;
            case 12:
                equipmentInven[0] = item;
                break;
            case 13:
                equipmentInven[0] = item;
                break;
            case 14:
                equipmentInven[1] = item;
                break;
            case 15:
                equipmentInven[2] = item;
                break;
            case 16:
                equipmentInven[3] = item;
                break;
            case 17:
                equipmentInven[4] = item;
                break;
            case 18:
                equipmentInven[5] = item;
                break;
            default:
                break;
        }
    }
}

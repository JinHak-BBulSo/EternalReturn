using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemCombine : MonoBehaviour
{
    public GameObject player;
    public GameObject ItemCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InventoryChange();
            func(ItemManager.Instance.itemList, 1);

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (ItemManager.Instance.combineAbleList.Count != 0)
            {

                CombineItem(ItemManager.Instance.combineAbleList[0], ItemManager.Instance.itemCombineDictionary);
                DeleteInferiorList(ItemManager.Instance.combineAbleList[0]);
                ItemManager.Instance.combineAbleList.RemoveAt(0);
                InventoryChange();

            }

        }
    }
    // 인벤토리에 변동 사항이 있을 경우 호출 되는 메서드
    public void InventoryChange()
    {
        for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
        {
            AddAbleCombineList(ItemManager.Instance.inventory[i], ItemManager.Instance.itemCombineDictionary);

        }
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {
            AddAbleCombineList(ItemManager.Instance.equipmentInven[i], ItemManager.Instance.itemCombineDictionary);


        }
        for (int i = 0; i < ItemManager.Instance.combineAbleList.Count; i++)
        {
            DeleteImpossibleCombine(ItemManager.Instance.combineAbleList[i], ItemManager.Instance.itemCombineDictionary);
        }
        ListSortRareAndUseable(ItemManager.Instance.combineAbleList);
        ItemManager.Instance.isItemPick = true;

    }
    public void func(List<ItemStat> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log($"아이템 이름 : {list[i].name}, 아이템 개수 : {list[i].count},아이템 레어도 : {list[i].rare}");
        }
    }
    public void func(List<ItemStat> list, int type = 0)
    {
        switch (type)
        {
            case 1:
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].rare == 0)
                    {
                        Debug.Log($"아이템 이름 : {list[i].name}, 아이템 id : {list[i].id},아이템 타입: {list[i].type}");
                    }
                }

                break;

            default:
                for (int i = 0; i < list.Count; i++)
                {
                    Debug.Log($"아이템 이름 : {list[i].name}, 아이템 개수 : {list[i].count},아이템 레어도 : {list[i].rare}");
                }
                break;
        }

    }

    //CombinList의 우선 순위를 나타 내기 위한 솔팅 메서드
    public void ListSortRareAndUseable(List<ItemStat> items_) // 콤바인 리스트
    {
        List<ItemStat> toSortWishList = new List<ItemStat>(); // 위시리스트가 아닌 템들을 저장하기 위한 리스트
        List<ItemStat> toSortRareList = new List<ItemStat>();
        List<ItemStat> toSortUncommonList = new List<ItemStat>();
        ItemStat item1 = default;
        // 위시리스트 아이템 리스트랑, 위시리스트가 아닌 아이템 리스트 구별



        foreach (ItemStat item_ in items_.ToList())
        {
            if (item_.rare == 1)
            {
                item1 = item_;
                items_.Remove(item_);
                toSortUncommonList.Add(item1);

            }
            else if (item_.rare == 2)
            {
                item1 = item_;
                items_.Remove(item_);
                toSortRareList.Add(item1);
            }
        }

        items_.AddRange(toSortRareList);
        items_.AddRange(toSortUncommonList);


        // 위시 리스트 나누기

        foreach (ItemStat item_ in items_.ToList())
        {
            if (!item_.isItemWishList)
            {
                item1 = item_;
                items_.Remove(item_);
                toSortWishList.Add(item1);

            }

        }
        items_.AddRange(toSortWishList);


    }
    //! 아이템 생성 및 획득 시 자신의 inferiorList에 있는 아이템을 삭제하는 메서드
    public void DeleteInferiorList(ItemStat item)
    {
        int count = 0;

        switch (item.rare)
        {
            case 0:
                ItemManager.Instance.DeleteInferiorList(item.id);
                break;
            case 1:
                for (int i = 0; i < ItemManager.Instance.ItemInferiorUncommon.Count; i++)
                {
                    ItemStat tier1 = ItemManager.Instance.ItemInferiorUncommon[i];
                    if (tier1.id == item.id)
                    {
                        Debug.Log("!!");
                        ItemManager.Instance.DeleteInferiorList(item.id);
                    }
                }
                break;
            case 2:
                Debug.Log(item.name);
                for (int i = 0; i < ItemManager.Instance.ItemInferiorRare.Count; i++)
                {
                    ItemStat tier2 = ItemManager.Instance.ItemInferiorRare[i];
                    if (tier2.id == item.id)
                    {
                        Debug.Log("!!");
                        ItemManager.Instance.DeleteInferiorList(item.id);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < ItemManager.Instance.itemWishList.Count; i++)
                {
                    ItemStat tier1 = ItemManager.Instance.itemWishList[i];
                    if (tier1.id == item.id)
                    {
                        Debug.Log("!!");
                        ItemManager.Instance.DeleteInferiorList(item.id);
                    }
                }
                break;
        }
    }
    //! 아이템 제작 시 아이템 생성 및 재료 아이템을 삭제하는 메서드 
    public void CombineItem(ItemStat item, Dictionary<ItemDefine, int> dic)
    {
        int[] needItem = ItemManager.Instance.ADDCombineItemToInven(item.id);
        bool idchk1 = false;
        bool idchk2 = false;
        List<int> itemSlot1 = new List<int>();
        List<int> itemSlot2 = new List<int>();
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {
            if (ItemManager.Instance.equipmentInven[i].id == needItem[0])
            {
                itemSlot2.Add(i);
                idchk1 = true;
            }
            else if (ItemManager.Instance.equipmentInven[i].id == needItem[1])
            {
                itemSlot2.Add(i);
                idchk2 = true;
            }
        }


        if (!idchk1 || !idchk2)
        {

            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                if (ItemManager.Instance.inventory[i].id == needItem[0])
                {
                    itemSlot1.Add(i);
                    idchk1 = true;
                }
                else if (ItemManager.Instance.inventory[i].id == needItem[1])
                {
                    itemSlot1.Add(i);
                    idchk2 = true;
                }
            }

        }
        if (idchk1 && idchk2)
        {
            for (int i = itemSlot1.Count - 1; i >= 0; i--)
            {

                if (ItemManager.Instance.inventory[itemSlot1[i]].count < 2)
                {
                    ItemManager.Instance.inventory.RemoveAt(itemSlot1[i]);
                }
                else
                {
                    ItemManager.Instance.inventory[itemSlot1[i]].count--;
                }

            }
            for (int i = itemSlot2.Count - 1; i >= 0; i--)
            {

                if (ItemManager.Instance.equipmentInven[itemSlot2[i]].count < 2)
                {
                    ItemManager.Instance.equipmentInven[itemSlot2[i]] = new ItemStat();
                }

            }
            if (ItemManager.Instance.EquipmentListIsBlank() != null)
            {
                bool chk = false;
                foreach (int i in ItemManager.Instance.EquipmentListIsBlank())
                {
                    if (i == item.type)
                    {
                        ItemManager.Instance.EquipmentListSet(item);
                        chk = true;
                    }



                }
                if (ItemManager.Instance.inventory.Count < 10 && !chk)
                {
                    ItemManager.Instance.AddItemToList(item, ItemManager.Instance.inventory);
                }
                else if (!chk)
                {
                    ItemManager.Instance.DropItem(item, player, ItemCanvas);
                }

            }
            else if (ItemManager.Instance.EquipmentListIsBlank() == null && ItemManager.Instance.inventory.Count < 10)
            {
                ItemManager.Instance.AddItemToList(item, ItemManager.Instance.inventory);
            }
            else
            {
                ItemManager.Instance.DropItem(item, player, ItemCanvas);
            }



        }



    }
    //! 아이템 생성 및 버리기를 통해 아이템 인벤토리가 변경될때, CombinList를 설정해주는 메서드
    public void DeleteImpossibleCombine(ItemStat item, Dictionary<ItemDefine, int> dic)
    {
        ItemDefine itemDefine = new ItemDefine();
        bool idchk1 = false;
        bool idchk2 = false;
        int id_1 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_1;
        int id_2 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_2;
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {
            if (ItemManager.Instance.equipmentInven[i].id == id_1)
            {

                idchk1 = true;
            }
            else if (ItemManager.Instance.equipmentInven[i].id == id_2)
            {

                idchk2 = true;
            }
        }
        if (!idchk1 || !idchk2)
        {

            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                if (ItemManager.Instance.inventory[i].id == id_1)
                {
                    idchk1 = true;
                }
                else if (ItemManager.Instance.inventory[i].id == id_2)
                {
                    idchk2 = true;
                }
            }

        }
        if (!idchk1 || !idchk2)
        {
            ItemManager.Instance.combineAbleList.RemoveAt(ItemManager.Instance.combineAbleList.IndexOf(item));
        }
    }
    // CombineList 에 있는 아이템이 내가 만들어야하는 아이템인지 구분 해주기 위한 메서드;
    public bool isItemNeed(List<ItemStat> List1_, List<ItemStat> List2_, List<ItemStat> List3_, ItemStat item_)
    {
        for (int i = 0; i < List1_.Count; i++)
        {
            if (List1_[i].id == item_.id)
            {
                return true;
            }
        }
        for (int i = 0; i < List2_.Count; i++)
        {
            if (List2_[i].id == item_.id)
            {
                return true;
            }
        }
        for (int i = 0; i < List3_.Count; i++)
        {
            if (List3_[i].id == item_.id)
            {
                return true;
            }
        }
        return false;
    }
    // CombineList에 생성할 수 있는 아이템을 추가 해 주는 메서드
    public void AddAbleCombineList(ItemStat item, Dictionary<ItemDefine, int> dic)
    {
        bool isEquipment = false;
        ItemStat cachingItem = default;
        for (int i = 0; i < ItemManager.Instance.equipmentInven.Count; i++)
        {

            if (ItemManager.Instance.equipmentInven[i] != null && ItemManager.Instance.equipmentInven[i].id != 0)
            {
                Debug.Log(item.id);
                ItemDefine define = new ItemDefine(item.id, ItemManager.Instance.equipmentInven[i].id);

                if (define.itemId_2 >= define.itemId_1)
                {

                    if (define.IsitemCombineValue(dic))
                    {
                        if (define.GetitemCombineValue(dic) == null)
                        {

                        }
                        else
                        {
                            cachingItem = ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1];
                            if (!ItemManager.Instance.combineAbleList.Contains(cachingItem))
                            {
                                cachingItem.isItemWishList = isItemNeed(ItemManager.Instance.itemWishList, ItemManager.Instance.ItemInferiorRare, ItemManager.Instance.ItemInferiorUncommon, cachingItem);
                                ItemManager.Instance.combineAbleList.Add(cachingItem);
                                // Debug.Log(cachingItem.isItemWishList);
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
                            cachingItem = ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1];
                            if (!ItemManager.Instance.combineAbleList.Contains(cachingItem))
                            {
                                cachingItem.isItemWishList = isItemNeed(ItemManager.Instance.itemWishList, ItemManager.Instance.ItemInferiorRare, ItemManager.Instance.ItemInferiorUncommon, cachingItem);
                                ItemManager.Instance.combineAbleList.Add(cachingItem);
                                Debug.Log(cachingItem.isItemWishList);
                                isEquipment = true;
                            }

                        }


                    }
                }
            }
        }
        if (!isEquipment)
        {
            for (int i = 0; i < ItemManager.Instance.inventory.Count; i++)
            {
                ItemDefine define = new ItemDefine(item.id, ItemManager.Instance.inventory[i].id);

                if (define.itemId_2 >= define.itemId_1)
                {
                    if (define.IsitemCombineValue(dic))
                    {
                        if (define.GetitemCombineValue(dic) == null)
                        {

                        }
                        else
                        {
                            cachingItem = ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1];
                            if (!ItemManager.Instance.combineAbleList.Contains(cachingItem))
                            {
                                cachingItem.isItemWishList = isItemNeed(ItemManager.Instance.itemWishList, ItemManager.Instance.ItemInferiorRare, ItemManager.Instance.ItemInferiorUncommon, cachingItem);
                                ItemManager.Instance.combineAbleList.Add(cachingItem);
                                // Debug.Log(cachingItem.isItemWishList);
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
                            cachingItem = ItemManager.Instance.itemList[dic[define.GetitemCombineValue(dic)] - 1];
                            if (!ItemManager.Instance.combineAbleList.Contains(cachingItem))
                            {
                                cachingItem.isItemWishList = isItemNeed(ItemManager.Instance.itemWishList, ItemManager.Instance.ItemInferiorRare, ItemManager.Instance.ItemInferiorUncommon, cachingItem);
                                ItemManager.Instance.combineAbleList.Add(cachingItem);
                                Debug.Log(cachingItem.isItemWishList);
                                isEquipment = true;
                            }

                        }


                    }
                }
            }
        }
    }
}

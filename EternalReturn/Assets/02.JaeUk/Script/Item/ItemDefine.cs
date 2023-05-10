using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemDefine
{
    public int itemId_1; // 하위 아이템 A
    public int itemId_2; // 하위 아이템 B
    public ItemDefine(int a, int b)
    {
        itemId_1 = a;
        itemId_2 = b;
    }
    public ItemDefine()
    {

    }
    public bool IsitemCombineValue(Dictionary<ItemDefine, int> dic)
    {
        ItemDefine define = new ItemDefine(itemId_1, itemId_2);
        for (int i = 0; i < ItemManager.Instance.itemList.Count; i++)
        {
            if (define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].id) != null)
            {
                // Debug.Log("id1 Find" + define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].item.id).itemId_1);

                // Debug.Log("id2 Find" + define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].item.id).itemId_2);

                if (!ComparerItem(define, define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].id)))
                {

                    return true;

                }
            }


        }
        return false;
    }
    public ItemDefine GetitemCombineValue(Dictionary<ItemDefine, int> dic)
    {
        ItemDefine define = new ItemDefine(itemId_1, itemId_2);
        for (int i = 0; i < ItemManager.Instance.itemList.Count; i++)
        {
            if (define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].id) != null)
            {
                // Debug.Log("id1 Find" + define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].item.id).itemId_1);

                // Debug.Log("id2 Find" + define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].item.id).itemId_2);
                if (!ComparerItem(define, define.FineInferiorItemId(dic, ItemManager.Instance.itemList[i].id)))
                {

                    return FineInferiorItemId(dic, ItemManager.Instance.itemList[i].id);

                }
            }


        }
        return null;
    }
    public bool ComparerItem(ItemDefine a, ItemDefine b)
    {
        if (a.itemId_1 == b.itemId_1 && a.itemId_2 == b.itemId_2)
        {
            return false;

        }
        else
        {
            return true;
        }
    }
    public ItemDefine FineInferiorItemId(Dictionary<ItemDefine, int> dic, int i)
    {

        return dic.FirstOrDefault(x => x.Value == i).Key;
    }

}

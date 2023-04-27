using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class FocusedItem : MonoBehaviour
{
    private ItemStat focused;
    private GameObject itemSlotWithName;
    private Image itemSlotImage;
    private Image itemImage;
    private Text itemName;

    private ItemDefine itemDefine = new ItemDefine();

    public void UpdateFocusedItem(ItemStat item)
    {
        if (item == null)
            return;

        focused = item;
        itemSlotImage.sprite = WishListSetting.ItemBgSpritesRO[focused.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[focused.id].GetComponent<SpriteRenderer>().sprite;
        itemName.text = focused.name;

        int itemTreeHeight = UpdateInferiorItems(item, 1);
        Debug.Log(itemTreeHeight);
    }

    private int UpdateInferiorItems(ItemStat item, int currentHeight)
    {
        if (item == null || item.rare == (int)ItemRarityType.Common)
        {
            return currentHeight;
        }

        int inferiorItemID1 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_1;
        int inferiorItemID2 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_2;

        Debug.Log($"1){inferiorItemID1 - 1} {ItemManager.Instance.itemList[inferiorItemID1 - 1].name}, 2){inferiorItemID2 - 1} {ItemManager.Instance.itemList[inferiorItemID2 - 1].name}");

        ItemStat inferiorItemL = ItemManager.Instance.itemList[inferiorItemID1 - 1];
        ItemStat inferiorItemR = ItemManager.Instance.itemList[inferiorItemID2 - 1];

        return Mathf.Max(UpdateInferiorItems(inferiorItemL, currentHeight + 1), UpdateInferiorItems(inferiorItemR, currentHeight + 1));
    }

    private void Awake()
    {
        itemSlotWithName = transform.GetChild(0).gameObject;
        itemSlotImage = itemSlotWithName.transform.GetChild(0).GetComponent<Image>();
        itemImage = itemSlotWithName.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        itemName = itemSlotWithName.transform.GetChild(1).GetComponent<Text>();
    }
}

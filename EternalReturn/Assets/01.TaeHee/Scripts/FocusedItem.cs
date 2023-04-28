using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private GameObject itemSlotWithNamePrefab;

    private ItemDefine itemDefine = new ItemDefine();

    public void UpdateFocusedItem(ItemStat item)
    {
        if (item == null)
            return;

        //focused = item;
        //itemSlotImage.sprite = WishListSetting.ItemBgSpritesRO[focused.rare];
        //itemImage.sprite = ItemManager.Instance.itemListObj[focused.id].GetComponent<SpriteRenderer>().sprite;
        //itemName.text = focused.name;

        int itemTreeHeight = UpdateFocusedItemSlots(item, 1, 0);
        Debug.Log(itemTreeHeight);

        transform.localPosition = new Vector2(-365f, 64 + (itemTreeHeight - 1) * 40);
    }

    private int UpdateFocusedItemSlots(ItemStat item, int currentHeight, float xPos)
    {
        if (item == null)
        {
            return currentHeight - 1;
        }

        GameObject inst = Instantiate(itemSlotWithNamePrefab);
        inst.transform.GetChild(0).GetComponent<Image>().sprite = WishListSetting.ItemBgSpritesRO[item.rare];
        inst.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;
        inst.transform.GetChild(1).GetComponent<Text>().text = item.name;

        inst.transform.GetChild(1).GetComponent<Text>().fontSize = 19 - currentHeight * 1;

        float width = inst.GetComponent<RectTransform>().rect.width;
        float height = inst.GetComponent<RectTransform>().rect.height;
        Vector2 localSize = inst.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(Mathf.Pow(1.2f, 1 - currentHeight), Mathf.Pow(1.2f, 1 - currentHeight));

        inst.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(width * localSize.x, inst.transform.GetChild(1).GetComponent<RectTransform>().rect.height);

        inst.GetComponent<RectTransform>().SetParent(transform, false);
        inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0 - (currentHeight - 1) * height);

        if (item.rare == (int)ItemRarityType.Common)
        {
            Debug.Log(item.id);
            return currentHeight;
        }

        Debug.Log(item.id);
        int inferiorItemId1 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_1;
        int inferiorItemId2 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_2;

        Debug.Log(inferiorItemId1);
        Debug.Log(inferiorItemId2);
        Debug.Log($"1){inferiorItemId1 - 1} {ItemManager.Instance.itemList[inferiorItemId1 - 1].name}, 2){inferiorItemId2 - 1} {ItemManager.Instance.itemList[inferiorItemId2 - 1].name}");

        ItemStat inferiorItemL = ItemManager.Instance.itemList[inferiorItemId1 - 1];
        ItemStat inferiorItemR = ItemManager.Instance.itemList[inferiorItemId2 - 1];

        return Mathf.Max(UpdateFocusedItemSlots(inferiorItemL, currentHeight + 1, xPos - width * Mathf.Pow(localSize.x, 3)), 
            UpdateFocusedItemSlots(inferiorItemR, currentHeight + 1, xPos + width * Mathf.Pow(localSize.x, 3)));
    }

    private void Awake()
    {
        itemSlotWithName = transform.GetChild(0).gameObject;
        itemSlotImage = itemSlotWithName.transform.GetChild(0).GetComponent<Image>();
        itemImage = itemSlotWithName.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        itemName = itemSlotWithName.transform.GetChild(1).GetComponent<Text>();
    }
}

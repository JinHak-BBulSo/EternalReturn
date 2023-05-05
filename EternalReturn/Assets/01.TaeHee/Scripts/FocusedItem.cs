using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusedItem : MonoBehaviour
{
    private const int MAX_SLOT_COUNT = 127; //2^7 - 1 (최대 6줄까지)
    private const int MAX_NAME_FONT_SIZE = 18;
    private List<ItemSlotWithName> itemSlotList = new List<ItemSlotWithName>();
    //private ItemStat focused;
    //private GameObject itemSlotWithName;
    //private Image itemSlotImage;
    //private Image itemImage;
    //private Text itemName;

    public ItemStat CurrentFocusedItem { get; private set; }

    [SerializeField] private RectTransform itemSlotParent;
    [SerializeField] private GameObject itemSlotWithNamePrefab;
    private float slotWidth;
    private float slotHeight;

    private RectTransform rectTransform;
    private float defaultPosX;
    private float defaultPosY;
    private int disabledSlotStartIdx;
    private int prevIdx;

    private ItemDefine itemDefine = new ItemDefine();

    public void UpdateFocusedItem(ItemStat item)
    {
        if (item == null)
            return;

        prevIdx = disabledSlotStartIdx;
        disabledSlotStartIdx = 0;
        int itemTreeHeight = UpdateFocusedItemSlots(item, 1, 0);
        //Debug.Log(itemTreeHeight);

        rectTransform.anchoredPosition = new Vector2(defaultPosX, defaultPosY + (itemTreeHeight - 1) * 40);
        for (int i = disabledSlotStartIdx; i < prevIdx; i++)
        {
            itemSlotList[i].gameObject.SetActive(false);
        }

        CurrentFocusedItem = item;
    }

    private int UpdateFocusedItemSlots(ItemStat item, int currentHeight, float xPos)
    {
        if (item == null)
        {
            return currentHeight - 1;
        }

        GameObject inst = itemSlotList[disabledSlotStartIdx++].gameObject;
        inst.SetActive(true);

        inst.transform.GetChild(0).GetComponent<ItemSlot>().UpdateNewItem(item);
        inst.transform.GetChild(1).GetComponent<Text>().text = item.name;
        inst.transform.GetChild(1).GetComponent<Text>().fontSize = MAX_NAME_FONT_SIZE - (currentHeight * 1 - 1);

        Vector2 localSize = inst.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(Mathf.Pow(1.2f, 1 - currentHeight), Mathf.Pow(1.2f, 1 - currentHeight));
        inst.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth * localSize.x, inst.transform.GetChild(1).GetComponent<RectTransform>().rect.height);
        inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0 - (currentHeight - 1) * slotHeight);

        if (item.rare == (int)ItemRarityType.Common)
        {
            return currentHeight;
        }

        if (itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id) == null)
        {
            Debug.LogWarning($"No combination in dictionary: [itemID]{item.id}");
            return currentHeight;
        }
        int inferiorItemId1 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_1;
        int inferiorItemId2 = itemDefine.FineInferiorItemId(ItemManager.Instance.itemCombineDictionary, item.id).itemId_2;

        //Debug.Log($"1){inferiorItemId1 - 1} {ItemManager.Instance.itemList[inferiorItemId1 - 1].name}, 2){inferiorItemId2 - 1} {ItemManager.Instance.itemList[inferiorItemId2 - 1].name}");

        ItemStat inferiorItemL = ItemManager.Instance.itemList[inferiorItemId1 - 1];
        ItemStat inferiorItemR = ItemManager.Instance.itemList[inferiorItemId2 - 1];

        return Mathf.Max(UpdateFocusedItemSlots(inferiorItemL, currentHeight + 1, xPos - slotWidth * Mathf.Pow(localSize.x, 3)), 
            UpdateFocusedItemSlots(inferiorItemR, currentHeight + 1, xPos + slotWidth * Mathf.Pow(localSize.x, 3)));
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultPosX = rectTransform.anchoredPosition.x;
        defaultPosY = rectTransform.anchoredPosition.y;

        InitalizeItemSlot();
    }

    private void InitalizeItemSlot()
    {
        slotWidth = itemSlotWithNamePrefab.GetComponent<RectTransform>().rect.width;
        slotHeight = itemSlotWithNamePrefab.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotWithNamePrefab);

            itemSlot.GetComponent<RectTransform>().SetParent(itemSlotParent, false);
            itemSlot.SetActive(false);
            itemSlotList.Add(itemSlot.GetComponent<ItemSlotWithName>());
        }
    }
}

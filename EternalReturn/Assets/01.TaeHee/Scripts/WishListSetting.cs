using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

[Serializable] public enum ItemRarityType
{
    Common = 0, //하양
    Uncommon,   //초록
    Rare,       //파랑
    Epic,       //보라
    Legendary   //노랑
}

public enum ItemType
{
    All = -2,
    Weapon = -1,
    Material = 0,
    Head = 14,
    Chest = 15,
    Arm = 16,
    Leg = 17,
    Accessory = 18,
    Axe = 19,
    Beverage = 20,
    Food = 21
}

public class WishListSetting : MonoBehaviour
{
    public static ReadOnlyCollection<Sprite> ItemBgSpritesRO { get; private set; }

    private const int ITEM_SLOT_IMAGE_COUNT = 4;
    private const string PATH_ITEM_SLOT_IMAGE = "03.Item/Ico_ItemGradebg_";//+ [01, 04]
    private const string PATH_ITEM_SLOT_PREFAB = "03.Item/Prefabs/ItemSlot";

    private List<Sprite> itemBgSprites = new List<Sprite>();

    private Dictionary<ItemType, List<ItemStat>> cachedItemPool = new Dictionary<ItemType, List<ItemStat>>();
    private List<ItemStat> focusedItemPool = new List<ItemStat>();
    [SerializeField] private List<ItemStat> searchedItemList = new List<ItemStat>();
    private List<ItemSlot> itemSlotList = new List<ItemSlot>();

    [SerializeField] private RectTransform scrollViewContent;
    [SerializeField] private RectTransform itemSlotParent;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private GameObject itemSlotPrefab;
    private InputField inputField;
    private int maxItemCount;
    private float maxScrollViewContentHeight;

    private float itemSlotWidth;
    private float itemSlotHeight;

    private ItemType focusedItemType;

    public void SetFocusedItemType(ItemType itemType)
    {
        focusedItemType = itemType;
        SetItemSlotList();
    }

    public void SetItemSlotList()
    {
        string inputStr = inputField.text;

        if (!cachedItemPool.TryGetValue(focusedItemType, out focusedItemPool))
        {
            Debug.Log($"No key");
            UpdateItemSlotUI(null);
            return;
        }

        if (inputStr.IsNullOrEmpty())
        {
            Debug.Log($"default");
            UpdateItemSlotUI(focusedItemPool);
            return;
        }

        searchedItemList.Clear();
        List<int> failFunc = GetFailFunc(inputStr);

        foreach (ItemStat item in focusedItemPool)
        {
            string itemName = item.name;
            if (itemName.Length < inputStr.Length)
                continue;

            for (int i = 0, j = 0; i < itemName.Length; i++)
            {
                while (j > 0 && itemName[i] != inputStr[j])
                {
                    j = failFunc[j - 1];
                }

                if (itemName[i] == inputStr[j])
                {
                    if (j == inputStr.Length - 1)
                    {
                        j = failFunc[j];
                        Debug.Log($"[find] input: {inputField.text}, target: {item}");
                        searchedItemList.Add(item);
                    }
                    else
                    {
                        ++j;
                    }
                }
            }
        }
        UpdateItemSlotUI(searchedItemList);
    }

    private void Awake()
    {
        //itemSlotPrefab = Resources.Load(ITEM_SLOT_PREFAB_PATH) as GameObject;
        for (int i = 1; i <= ITEM_SLOT_IMAGE_COUNT; i++)
        {
            string path = $"{PATH_ITEM_SLOT_IMAGE}{(i).ToString().PadLeft(2, '0')}";
            itemBgSprites.Add(Resources.Load<Sprite>(path));
        }

        ItemBgSpritesRO = new ReadOnlyCollection<Sprite>(itemBgSprites);

        inputField = transform.GetChild(0).GetComponent<InputField>();

        itemSlotWidth = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        itemSlotHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
    }

    private void Start()
    {
        focusedItemType = ItemType.All;

        InitalizeCachedItemPool();
        InitializeItemSlot();
        UpdateItemSlotUI(ItemManager.Instance.itemList);
    }

    private void InitializeItemSlot()
    {
        maxItemCount = ItemManager.Instance.itemList.Count;
        scrollViewContent.sizeDelta = new Vector2(0, itemSlotHeight * ((maxItemCount / 5) + 1));

        float xStartPos = scrollViewContent.anchoredPosition.x - (scrollViewContent.rect.width - itemSlotWidth) / 2;
        float yStartPos = (scrollViewContent.rect.height - itemSlotHeight) / 2;
        float xPos = xStartPos;
        float yPos;

        for (int i = 0; i < maxItemCount; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab);
            itemSlot.name = $"Item Slot({i})";
            itemSlot.GetComponent<RectTransform>().SetParent(itemSlotParent, false);

            xPos = (i % 5 == 0) ? xStartPos : xPos + itemSlotWidth;
            yPos = yStartPos - (itemSlotHeight * (i / 5));
            itemSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

            itemSlotList.Add(itemSlot.GetComponent<ItemSlot>());
        }

        maxScrollViewContentHeight = itemSlotHeight * ((maxItemCount / 5) + 1);
    }

    private void UpdateItemSlotUI(List<ItemStat> itemList)
    {
        int itemListCount = itemList?.Count ?? 0;

        scrollViewContent.sizeDelta = new Vector2(0, itemSlotHeight * ((itemListCount / 5) + 1));
        itemSlotParent.anchoredPosition = new Vector2(0, (scrollViewContent.sizeDelta.y - maxScrollViewContentHeight) / 2);

        for (int i = 0; i < maxItemCount; i++)
        {
            if (i >= itemListCount)
            {
                itemSlotList[i].gameObject.SetActive(false);
                continue;
            }

            itemSlotList[i].gameObject.SetActive(true);
            itemSlotList[i].UpdateNewItem(itemList[i]);
        }
    }

    private List<int> GetFailFunc(string inputStr)
    {
        List<int> failFunc = new List<int>();
        for (int i = 0; i < inputStr.Length; i++)
            failFunc.Add(0);

        for (int i = 1, j = 0; i < inputStr.Length; i++)
        {
            while (j > 0 && inputStr[i] != inputStr[j])
            {
                j = failFunc[j - 1];
            }

            if (inputStr[i] == inputStr[j])
            {
                failFunc[i] = ++j;
            }
        }

        return failFunc;
    }

    private void InitalizeCachedItemPool()
    {
        cachedItemPool.Add(ItemType.All, ItemManager.Instance.itemList);
        List<ItemStat> cachedWeaponItemPool = GetCachedItemList((int)ItemType.Material, (int)ItemType.Head);
        cachedWeaponItemPool.AddRange(GetCachedItemList(ItemType.Axe));
        cachedItemPool.Add(ItemType.Weapon, cachedWeaponItemPool);

        int itemTypeMax = Enum.GetValues(typeof(ItemType)).Cast<int>().Max();
        Debug.Log($"MAX {itemTypeMax} {Enum.IsDefined(typeof(ItemType), 21)}");

        for (int i = 0; i <= itemTypeMax; i++)
        {
            if (Enum.IsDefined(typeof(ItemType), i))
            {
                cachedItemPool.Add((ItemType)i, GetCachedItemList((ItemType)i));
            }
        }
    }

    private List<ItemStat> GetCachedItemList(ItemType itemType)
    {
        List<ItemStat> cachedItemPool = new List<ItemStat>();
        foreach (var item in ItemManager.Instance.itemList)
        {
            //if (itemType == ItemType.Food)
            //{
            //    Debug.Log($"{item.name}, {item.type}");
            //}

            if (item.type == (int)itemType)
            {
                cachedItemPool.Add(item);
            }
        }
        return cachedItemPool;
    }

    private List<ItemStat> GetCachedItemList(int exclusiveMin, int exclusiveMax)
    {
        List<ItemStat> cachedItemPool = new List<ItemStat>();
        foreach (var item in ItemManager.Instance.itemList)
        {
            if (item.type > exclusiveMin && item.type < exclusiveMax)
            {
                cachedItemPool.Add(item);
            }
        }

        return cachedItemPool;
    }
}
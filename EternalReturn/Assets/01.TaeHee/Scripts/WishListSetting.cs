using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public enum ItemRarityType
{
    Common = 0, //하양
    Uncommon, //초록
    Rare, //파랑
    Epic, //보라
    Legendary //노랑
}

public enum ItemType
{
    Weapon = 0,
    Head = 14,
    Chest = 15,
    Arm = 16,
    Leg = 17,
    Accessory = 18,
    Food,
    Beverage = 20
}

public class WishListSetting : MonoBehaviour
{
    //
    public int currentToggle;

    private const int ITEM_SLOT_IMAGE_COUNT = 4;
    private const string PATH_ITEM_SLOT_IMAGE = "03.Item/Ico_ItemGradebg_";//+ [01, 04]
    private const string PATH_ITEM_SLOT_PREFAB = "03.Item/Prefabs/ItemSlot";

    public static ReadOnlyCollection<Sprite> itemBgSpritesRO { get; private set; }

    private List<Sprite> itemBgSprites = new List<Sprite>();

    private List<ItemStat> selectedItemPool = new List<ItemStat>();
    [SerializeField] private List<ItemStat> searchedItemList = new List<ItemStat>();
    private List<ItemSlot> itemSlotList = new List<ItemSlot>();

    [SerializeField] private RectTransform scrollViewContent;
    [SerializeField] private RectTransform itemSlotParent;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private GameObject itemSlotPrefab;
    private InputField inputField;
    private int maxItemCount;
    private float itemSlotWidth;
    private float itemSlotHeight;

    public void OnTextChanged()
    {
        string inputStr = inputField.text;
        if (inputStr.IsNullOrEmpty())
        {
            Debug.Log($"default");
            UpdateItemSlot(selectedItemPool);
            return;
        }

        searchedItemList.Clear();
        List<int> failFunc = SetFailFunc(inputStr);

        foreach (ItemStat item in selectedItemPool)
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
        UpdateItemSlot(searchedItemList);
    }

    private void Awake()
    {
        selectedItemPool.AddRange(ItemManager.Instance.itemList);

        //itemSlotPrefab = Resources.Load(ITEM_SLOT_PREFAB_PATH) as GameObject;
        for (int i = 1; i <= ITEM_SLOT_IMAGE_COUNT; i++)
        {
            string path = $"{PATH_ITEM_SLOT_IMAGE}{(i).ToString().PadLeft(2, '0')}";
            itemBgSprites.Add(Resources.Load<Sprite>(path));
        }

        itemBgSpritesRO = new ReadOnlyCollection<Sprite>(itemBgSprites);

        inputField = GetComponent<InputField>();

        itemSlotWidth = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        itemSlotHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
    }

    private void Start()
    {
        InitializeItemSlot();
        UpdateItemSlot(ItemManager.Instance.itemList);
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
    }

    private void UpdateItemSlot(List<ItemStat> itemList)
    {
        float yStartPos, yPos;
        
        scrollViewContent.sizeDelta = new Vector2(0, itemSlotHeight * ((itemList.Count / 5) + 1));
        yStartPos = (scrollViewContent.rect.height - itemSlotHeight) / 2;

        for (int i = 0; i < maxItemCount; i++)
        {
            if (i >= itemList.Count)
            {
                itemSlotList[i].gameObject.SetActive(false);
                continue;
            }

            itemSlotList[i].gameObject.SetActive(true);
            yPos = yStartPos - (itemSlotHeight * (i / 5));

            itemSlotList[i].SetNewItem(itemList[i], yPos);
        }
    }

    private List<int> SetFailFunc(string inputStr)
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
}

using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WishListSetting : MonoBehaviour
{
    private const string ITEM_SLOT_PREFAB_PATH = "03.Item/Prefabs/ItemSlot";

    [SerializeField] private List<string> itemNameList = new List<string>();

    private Transform canvas;
    private InputField inputField;
    [SerializeField] private GameObject itemSlotPrefab;

    private void Awake()
    {
        canvas = transform.root;
        inputField = GetComponent<InputField>();
        //itemSlotPrefab = Resources.Load(ITEM_SLOT_PREFAB_PATH) as GameObject;
    }

    private void Start()
    {
        float xStartPos = -83f;
        float yStartPos = 164f;
        float xPos = xStartPos;
        float yPos = 0;
        float width = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        float height = itemSlotPrefab.GetComponent<RectTransform>().rect.height;

        foreach (var item in ItemManager.Instance.itemList)
        {
            if (item.rare > 0)
            {
                itemNameList.Add(item.name);
                GameObject itemSlot = Instantiate(itemSlotPrefab);
                itemSlot.name = "ItemSlot";
                itemSlot.transform.SetParent(canvas);

                if ((itemNameList.Count - 1) % 5 == 0)
                {
                    xPos = xStartPos;
                }
                else
                {
                    xPos += width;
                }
                yPos = yStartPos - (height * ((itemNameList.Count - 1) / 5));

                itemSlot.GetComponent<RectTransform>().position = new Vector2 (xPos, yPos);
            }
        }
    }

    public void OnTextChanged()
    {
        string inputStr = inputField.text;
        if (inputStr.IsNullOrEmpty())
        {
            Debug.Log("default");
            return;
        }

        List<int> failFunc = SetFailFunc(inputStr);

        foreach (string itemName in itemNameList)
        {
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
                        Debug.Log($"[find] input: {inputField.text}, target: {itemName}");
                    }
                    else
                    {
                        ++j;
                    }
                }
            }
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

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.SceneManagement;
public class CSVReader
{
    public static List<string[]> Read(string file)
    {
        List<string[]> list = new List<string[]>();
        TextAsset data = Resources.Load<TextAsset>(file);




        string[] arrayLaw = data.text.Split("#end");
        for (int i = 0; i < arrayLaw.Length - 1; i++)
        {
            string[] array = arrayLaw[i].Split(new char[] { ',' });
            list.Add(array);
        }
        return list;
    }
}

public class ItemCombineLoad : MonoBehaviour
{
    private List<ItemStat> itemList;
    string combinePath = "ItemCombine";
    string itemPath = "ItemList";
    private ItemDefine itemDefine;
    public List<ItemStat> itemInferiorList;
    public List<ItemStat> itemInferiorRare;
    public List<ItemStat> itemInferiorUncommon;
    private Dictionary<ItemDefine, int> itemCombineDictionary;
    // public int[,] itemCombineKeyArray = new int[300, 300];

    // Start is called before the first frame update
    private void Start()
    {
        itemList = ItemManager.Instance.itemList;
        List<string[]> combineList = CSVReader.Read(combinePath);
        List<string[]> itemListCSV = CSVReader.Read(itemPath);
        itemCombineDictionary = new Dictionary<ItemDefine, int>();
        object[] ItemLoadObjs = Resources.LoadAll("03.Item/Prefabs");
        List<ItemStat> ItemWishList = new List<ItemStat>();
        for (int i = 0; i < combineList.Count; i++)
        {
            itemDefine = new ItemDefine(int.Parse(combineList[i][1]), int.Parse(combineList[i][2]));
            itemCombineDictionary.Add(itemDefine, int.Parse(combineList[i][3]));
        }



        ItemManager.Instance.itemCombineDictionary = itemCombineDictionary;

        for (int i = 0; i < itemList.Count; i++)
        {
            ItemStat item = new ItemStat(float.Parse(itemListCSV[i][0]), float.Parse(itemListCSV[i][1]), float.Parse(itemListCSV[i][2]),
            float.Parse(itemListCSV[i][3]), float.Parse(itemListCSV[i][4]), float.Parse(itemListCSV[i][5]), float.Parse(itemListCSV[i][6]), float.Parse(itemListCSV[i][7]),
            float.Parse(itemListCSV[i][8]), float.Parse(itemListCSV[i][9]), float.Parse(itemListCSV[i][10]), float.Parse(itemListCSV[i][11]), float.Parse(itemListCSV[i][12]),
            float.Parse(itemListCSV[i][13]), float.Parse(itemListCSV[i][14]), float.Parse(itemListCSV[i][15]), float.Parse(itemListCSV[i][16]), float.Parse(itemListCSV[i][17]),
            float.Parse(itemListCSV[i][18]), int.Parse(itemListCSV[i][19]), int.Parse(itemListCSV[i][20]), int.Parse(itemListCSV[i][21]), itemListCSV[i][22], int.Parse(itemListCSV[i][23]),
            int.Parse(itemListCSV[i][24]), int.Parse(itemListCSV[i][25]));
            itemList[i] = item;

        }
        ItemManager.Instance.itemList = itemList;

        // func(ItemManager.Instance.itemList, false);

        // ItemWishList.Add(AddItem(132));
        ItemWishList.Add(AddItem(102));
        ItemWishList.Add(AddItem(171));
        ItemWishList.Add(AddItem(148));
        ItemWishList.Add(AddItem(187));
        ItemWishList.Add(AddItem(81));

        // func(ItemWishList, true);
        ItemManager.Instance.itemWishList = default;
        ItemManager.Instance.itemWishList = ItemWishList;
        ItemManager.Instance.SetDefault();
        // ItemManager.Instance.EquipmentListSet(AddItem(0));
        // ItemManager.Instance.EquipmentListSet(AddItem(20));
        // ItemManager.Instance.EquipmentListSet(AddItem(27));
        // ItemManager.Instance.EquipmentListSet(AddItem(28));
        // ItemManager.Instance.EquipmentListSet(AddItem(34));


        // ItemManager.Instance.GetItem(AddItem(30));
        // ItemManager.Instance.GetItem(AddItem(23));
        // ItemManager.Instance.GetItem(AddItem(2));
        // ItemManager.Instance.GetItem(AddItem(40));



        // // ItemManager.Instance.inventory.Add(AddItem(54));
        // // ItemManager.Instance.inventory.Add(AddItem(226));
        // // ItemManager.Instance.inventory.Add(AddItem(26));
        // // ItemManager.Instance.inventory.Add(AddItem(42));
        // // ItemManager.Instance.inventory.Add(AddItem(43));
        // // ItemManager.Instance.inventory.Add(AddItem(47));
        // // ItemManager.Instance.inventory.Add(AddItem(56));
        // // ItemManager.Instance.inventory.Add(AddItem(52));
        // // ItemManager.Instance.inventory.Add(AddItem(31));
        // // ItemManager.Instance.inventory[0].count += 2;
        // // ItemManager.Instance.inventory[1].count += 1;




        // for (int i = 0; i < ItemWishList.Count; i++)
        // {
        //     ItemManager.Instance.AddInferiorList(ItemWishList[i].id);
        // }

        // itemInferiorList = ItemManager.Instance.itemInferiorList;
        // itemInferiorRare = ItemManager.Instance.ItemInferiorRare;
        // itemInferiorUncommon = ItemManager.Instance.ItemInferiorUncommon;

        //SceneManager.LoadScene("Lobby");
        // func(ItemManager.Instance.itemList, false);
    }
    public void func(List<ItemStat> list, bool flag)
    {
        if (flag)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log($"아이템 이름 : {list[i].name}, 아이템 개수 : {list[i].count},아이템 레어도 : {list[i].rare}");
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].count > 1)
                {
                    Debug.Log($"아이템 이름 : {list[i].name}, 아이템 개수 : {list[i].count},아이템 레어도 : {list[i].rare}");
                }

            }
        }

    }
    public ItemStat AddItem(int i)
    {
        ItemStat item = new ItemStat(ItemManager.Instance.itemList[i]);
        // Debug.Log(item);
        return item;
    }

}
// Update is called once per frame




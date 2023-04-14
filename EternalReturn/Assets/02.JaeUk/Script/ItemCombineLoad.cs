using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
// public class CSVLoad
// {
//     string path = "ItemCombine.csv";
//     List<Dictionary<string, object>> CSVlist;
//     public void LoadCSVFile()
//     {

//         CSVlist = CSVReader.Read(path);

//         for (int i = 0; i < CSVlist.Count; i++)
//         {
//             Debug.Log("KeyValue" + CSVlist[i]["KeyValue"]);
//         }

//     }
// }
public class ItemCombineLoad : MonoBehaviour
{
    string path = "ItemCombine";

    private Dictionary<int, int> itemCombineList;
    public int[,] itemCombineKeyArray = new int[500, 500];

    // Start is called before the first frame update
    private void Awake()
    {
        List<string[]> CSVlist = CSVReader.Read(path);
        // for (var i = 0; i < CSVlist.Count; i++)
        // {
        //     Debug.Log(CSVlist[i][0]);
        //     Debug.Log(CSVlist[i][1]);
        //     Debug.Log(CSVlist[i][2]);
        //     Debug.Log(CSVlist[i][3]);
        // }

        itemCombineList = new Dictionary<int, int>();
        for (int i = 0; i < 177; i++)
        {
            itemCombineKeyArray[int.Parse(CSVlist[i][1]), int.Parse(CSVlist[i][2])] = i;
            itemCombineList.Add(i, int.Parse(CSVlist[i][3]));
        }

        for (int i = 0; i < 178; i++)
        {
            for (int j = 0; j < 178; j++)
            {
                if (itemCombineList[i] == 0)
                {

                }
                else
                {
                    Debug.Log(itemCombineList[itemCombineKeyArray[i, j]]);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

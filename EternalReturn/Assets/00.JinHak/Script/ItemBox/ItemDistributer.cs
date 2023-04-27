using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDistributer : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>();
    public List<int> itemCountList = new List<int>();
    public int[] indexArray = default;

    private ItemBox[] areaItemBoxes;
    
    void Start()
    {
        areaItemBoxes = GetComponentsInChildren<ItemBox>();
        ItemSet();
    }

    private void Shuffle(int[] array)
    {
        int n = array.Length;
        int last = n - 2;

        for (int i = 0; i <= last; i++)
        {
            int r = Random.Range(i, n);
            Swap(i, r);
        }

        // Local Method
        void Swap(int idxA, int idxB)
        {
            int temp = array[idxA];
            array[idxA] = array[idxB];
            array[idxB] = temp;
        }
    }

    private void ItemSet()
    {
        foreach(var itemBox in areaItemBoxes)
        {
            indexArray = new int[itemList.Count];

            while (itemCountList.Contains(0))
            {
                int index = itemCountList.IndexOf(0);
                itemCountList.RemoveAt(index);
                itemList.RemoveAt(index);
            }

            indexArray = new int[itemList.Count];
            
            for (int i = 0; i < indexArray.Length; i++)
            {
                indexArray[i] = i;
            }

            for (int i = 0; i < 10; i++)
            {
                Shuffle(indexArray);
            }

            int r = Random.Range(4, 6 + 1);
            for (int i = 0; i < r; i++)
            {
                if (i >= itemList.Count) break;
                
                itemBox.itemPrefabs.Add(itemList[indexArray[i]]);
                itemCountList[indexArray[i]]--;
            }

            itemBox.SetItems();
        }
    }
}

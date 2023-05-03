using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class ItemDistributer : MonoBehaviourPun
{
    public List<GameObject> itemList = new List<GameObject>();
    public List<int> itemCountList = new List<int>();

    public int[] indexArray = default;
    public int r = default;

    private ItemBox[] areaItemBoxes;
    
    void Start()
    {
        areaItemBoxes = GetComponentsInChildren<ItemBox>();

        ItemSetStart();
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

    [PunRPC]
    private void ItemIndexSet(int[] array_, int r_)
    {
        indexArray = array_.ToArray();
        r = r_;
    }
    
    public void ItemSetStart()
    {
        StartCoroutine(ItemSet());
    }

    IEnumerator ItemSet()
    {
        foreach(var itemBox in areaItemBoxes)
        {
            //indexArray = new int[itemList.Count];

            while (itemCountList.Contains(0))
            {
                int index = itemCountList.IndexOf(0);
                itemCountList.RemoveAt(index);
                itemList.RemoveAt(index);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                r = Random.Range(2, 6 + 1);
                indexArray = new int[itemList.Count];

                for (int i = 0; i < indexArray.Length; i++)
                {
                    indexArray[i] = i;
                }

                for (int i = 0; i < 10; i++)
                {
                    Shuffle(indexArray);
                }

                photonView.RPC("ItemIndexSet", RpcTarget.Others, indexArray, r);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                BoxSet(itemBox, r);
                yield return new WaitForSeconds(0.15f);
            }
            else
            {
                while (true)
                {
                    if (indexArray != default)
                    {
                        BoxSet(itemBox, r);
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }
    }

    private void BoxSet(ItemBox itemBox_, int r_)
    {
        for (int i = 0; i < r_; i++)
        {
            if (i >= itemList.Count) break;

            itemBox_.itemPrefabs.Add(itemList[indexArray[i]]);
            itemCountList[indexArray[i]]--;
        }
        Debug.Log("BoxSet");
        itemBox_.SetItems();
        indexArray = default;
    }
}

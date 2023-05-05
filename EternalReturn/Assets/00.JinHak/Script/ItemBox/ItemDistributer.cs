using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class ItemDistributer : MonoBehaviourPun
{
    private AllItemBox allItemBox = default;
    public List<GameObject> itemList = new List<GameObject>();
    public List<int> itemCountList = new List<int>();

    public int[] indexArray = default;
    public int r = default;

    private ItemBox[] areaItemBoxes;
    
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
   
    public void ItemSet()
    {
        allItemBox = transform.parent.GetComponent<AllItemBox>();
        areaItemBoxes = GetComponentsInChildren<ItemBox>();

        foreach (var itemBox in areaItemBoxes)
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
            }

            if (PhotonNetwork.IsMasterClient)
            {
                int index = 0;
                foreach (var i in indexArray)
                {
                    if (index >= r) break;
                    int itemIndex = itemList[i].GetComponent<ItemController>().item.id;
                    photonView.RPC("BoxSet", RpcTarget.All, itemBox.itemBoxIndex, itemIndex);
                }
                photonView.RPC("SetNotOpenImg", RpcTarget.All, itemBox.itemBoxIndex);
            }
        }
    }

    [PunRPC]
    private void BoxSet(int itemBoxIndex_, int index_)
    {
        if(allItemBox == default)
        {
            allItemBox = transform.parent.GetComponent<AllItemBox>();
        }
        allItemBox.allItemBoxes[itemBoxIndex_].AddItem(index_);
    }

    [PunRPC]
    private void SetNotOpenImg(int itemBoxIndex_)
    {
        allItemBox.allItemBoxes[itemBoxIndex_].notOpenItemBoxImg = Instantiate(allItemBox.itemBoxImgPrefab, allItemBox.worldCanvas.transform);
        allItemBox.allItemBoxes[itemBoxIndex_].notOpenItemBoxImg.transform.position = allItemBox.allItemBoxes[itemBoxIndex_].transform.position + new Vector3(0, 2.5f, 0);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemBox : MonoBehaviour
{
    public List<ItemBox> allItemBoxes = new List<ItemBox>();
    public List<ItemDistributer> allDistributers = new List<ItemDistributer>();

    private void Start()
    {
        int index = 0;
        foreach(var itemBox in allItemBoxes)
        {
            itemBox.itemBoxIndex = index;
            index++;
        }

        foreach (var distributer in allDistributers)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                distributer.ItemSet();
            }
        }
    }
}

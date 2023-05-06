using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemBox : MonoBehaviour
{
    public List<ItemBox> allItemBoxes = new List<ItemBox>();
    public List<ItemDistributer> allDistributers = new List<ItemDistributer>();
    private bool isGameStart = false;
    public GameObject itemBoxImgPrefab = default;
    public GameObject worldCanvas = default;

    private void Start()
    {
        worldCanvas = GameObject.Find("WorldCanvas");
        int index = 0;
        foreach (var itemBox in allItemBoxes)
        {
            itemBox.itemBoxIndex = index;
            index++;
        }  
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsGameStart && !isGameStart)
        {
            isGameStart = true;
            if (PhotonNetwork.IsMasterClient)
            {
                SetAllItem();
            }
            else
            {
                /* Do nothing */
            }
        }
        
    }

    private void SetAllItem()
    {
        foreach (var distributer in allDistributers)
        {
            distributer.ItemSet();
        }
        this.enabled = false;
    }
}

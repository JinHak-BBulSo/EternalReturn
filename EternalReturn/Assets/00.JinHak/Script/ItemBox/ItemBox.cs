using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    private GameObject itemBoxUi = default;
    private bool playerEnter = false;
    private GameObject[] itemBoxSlot = new GameObject[8];
    public List<GameObject> boxItems = new List<GameObject>();

    void Start()
    {
        itemBoxUi = GameObject.Find("TestUi").transform.GetChild(1).gameObject;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(other.GetComponent<PlayerBase>().clickTarget == this.gameObject)
            {
                playerEnter = true;
                itemBoxUi.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playerEnter)
            {
                itemBoxUi.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringItemBox : MonoBehaviour
{
    private Outline outline = default;

    public GameObject itemPrefab = default;
    public ItemStat gatheringItem = new ItemStat();

    void Start()
    {
        outline = GetComponent<Outline>();
        gatheringItem = itemPrefab.GetComponent<ItemController>().item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if (!outline.isClick)
        {
            outline.enabled = false;
        }
    }
}

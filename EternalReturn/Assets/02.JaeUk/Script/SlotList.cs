using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotList : MonoBehaviour
{
    public List<ItemStat> inventory;

    public List<ItemStat> itemListClone;
    // Start is called before the first frame update
    public virtual void Start()
    {
        itemListClone = ItemManager.Instance.itemList;

    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public virtual void Addslot()
    {

    }
}

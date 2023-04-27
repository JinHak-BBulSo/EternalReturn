using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatheringItemBox : MonoBehaviour
{
    public enum GatherItemType
    {
        NONE = -1,
        BRANCH,
        FLOWER,
        PEBBLE,
        POTATO,
        WATERPOINT,
        FISHING
    }

    private Outline outline = default;

    public GameObject itemPrefab = default;
    public ItemStat gatheringItem = new ItemStat();
    public GatherItemType itemType = GatherItemType.NONE;
    public bool gatherAble = false;
    private float gatherTime = 2;
    public float GatherTime
    {
        get { return gatherTime; }
    }
    private float respawnItemTime = 15;

    void Start()
    {
        outline = GetComponent<Outline>();
        gatheringItem = itemPrefab.GetComponent<ItemController>().item;
        gatherAble = true;
    }

    IEnumerator RespawnItem(float respawnTime_)
    {
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        yield return new WaitForSeconds(respawnTime_);
        transform.localScale = new Vector3(1, 1, 1);
        gatherAble = true;
    }

    public void GetItem()
    {
        if (!gatherAble) return;

        ItemStat item = new ItemStat(gatheringItem);
        ItemManager.Instance.GetItem(item);

        if (!ItemManager.Instance.isInventoryFull)
        {
            gatherAble = false;
            StartCoroutine(RespawnItem(respawnItemTime));
        }
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

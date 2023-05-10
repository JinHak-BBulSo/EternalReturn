using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GatheringItemBox : MonoBehaviourPun
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
    private GameObject worldCanvas = default;

    public GameObject gatherItemImgPrfab = default;
    public GameObject dontGatherItemImgPrefab = default;
    private GameObject gatherItemImg = default;
    private GameObject dontGatherItemImg = default;
    public GameObject itemPrefab = default;
    public ItemStat gatheringItem = new ItemStat();
    public GatherItemType itemType = GatherItemType.NONE;
    public bool gatherAble = false;
    private float gatherTime = 2;
    public float GatherTime
    {
        get { return gatherTime; }
    }
    public AudioSource gatherAudio = default;
    private float respawnItemTime = 15;

    void Start()
    {
        worldCanvas = GameObject.Find("WorldCanvas");
        gatherAudio = GetComponent<AudioSource>();
        outline = GetComponent<Outline>();
        gatheringItem = itemPrefab.GetComponent<ItemController>().item;
        gatherAble = true;
        gatherItemImg = Instantiate(gatherItemImgPrfab, worldCanvas.transform);
        gatherItemImg.transform.position = transform.position + new Vector3(0, 2.5f, 0);

        if(itemType == GatherItemType.FISHING)
        {
            dontGatherItemImg = Instantiate(dontGatherItemImgPrefab, worldCanvas.transform);
            dontGatherItemImg.transform.position = transform.position + new Vector3(0, 2.5f, 0);
            dontGatherItemImg.SetActive(false);
            respawnItemTime = 180;
        }
        else if(itemType == GatherItemType.WATERPOINT)
        {
            respawnItemTime = 120;
        }
    }

    [PunRPC]
    public void Respawn()
    {
        StartCoroutine(RespawnItem(respawnItemTime));
    }
    IEnumerator RespawnItem(float respawnTime_)
    {
        gatherItemImg.SetActive(false);
        if (itemType == GatherItemType.FISHING)
        {
            dontGatherItemImg.SetActive(true);
        }
        else
        {
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        }
        yield return new WaitForSeconds(respawnTime_);
        if (itemType == GatherItemType.FISHING)
        {
            dontGatherItemImg.SetActive(false);
        }
        transform.localScale = new Vector3(1, 1, 1);
        gatherItemImg.SetActive(true);
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
            photonView.RPC("Respawn", RpcTarget.All);
            gatherItemImg.SetActive(false);
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

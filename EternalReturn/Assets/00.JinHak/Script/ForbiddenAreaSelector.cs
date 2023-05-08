using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ForbiddenAreaSelector : MonoBehaviourPun
{
    public List<int> areaIndex = new List<int>();
    public List<GameObject> allArea = new List<GameObject>();
    public List<SpriteRenderer> areaSprite = new List<SpriteRenderer>();
    private float selectDelay = 300;
    private bool isGameStart = false;
    private int selectCount = 0;
    public Announce announce = default;

    void Start()
    {
        for (int i = 0; i < allArea.Count; i++)
        {
            areaIndex.Add(i);
        }
    }

    
    void Update()
    {
        if(PlayerManager.Instance.IsGameStart && !isGameStart)
        {
            isGameStart = true;
            if (PhotonNetwork.IsMasterClient)
            {
                ForbiddenAreaSelect();
            }
        }
    }
    
    public void ForbiddenAreaSelect()
    {
        int r_ = Random.Range(0, areaIndex.Count);
        int first_ = areaIndex[r_];
        areaIndex.Remove(first_);

        int r2_ = Random.Range(0, areaIndex.Count);
        int second_ = areaIndex[r2_];
        areaIndex.Remove(second_);

        photonView.RPC("ForbiddenAreaSet", RpcTarget.All, first_, second_);
    }

    [PunRPC]
    public void ForbiddenAreaSet(int firstArea_, int secondArea_)
    {
        Debug.Log(firstArea_);
        Debug.Log(secondArea_);
        photonView.RPC("EmergencyAreaSet", RpcTarget.All, firstArea_, secondArea_);

        StartCoroutine(ForbiddenAreaSetStart(firstArea_));
        StartCoroutine(ForbiddenAreaSetStart(secondArea_));
        selectCount++;

        if(selectCount < 7 && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SelectDelay());
        }
    }
    [PunRPC]
    public void EmergencyAreaSet(int firstArea_, int secondArea_)
    {
        areaSprite[firstArea_].color = new Color(1, 197.0f / 255.0f, 37.0f / 255.0f, 180.0f / 255.0f);
        areaSprite[secondArea_].color = new Color(1, 197.0f / 255.0f, 37.0f / 255.0f, 180.0f / 255.0f);
    }
    IEnumerator ForbiddenAreaSetStart(int areaIndex_)
    {
        yield return new WaitForSeconds(selectDelay);
        areaSprite[areaIndex_].color = new Color(1, 0, 0, 180.0f / 255.0f);

        for (int i = 0; i < allArea[areaIndex_].transform.childCount; i++)
        {
            if (i < allArea[areaIndex_].transform.childCount / 2)
            {
                allArea[areaIndex_].transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                allArea[areaIndex_].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        announce.announceAudio.clip = announce.allAnnounce[2];
        announce.announceAudio.Play();
    }
    IEnumerator SelectDelay()
    {
        yield return new WaitForSeconds(selectDelay);
        ForbiddenAreaSelect();
    }
}

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
    private int first;
    private int second;
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
                photonView.RPC("ForbiddenAreaSet", RpcTarget.All, first, second);
            }
        }
    }
    
    public void ForbiddenAreaSelect()
    {
        int r_ = Random.Range(0, areaIndex.Count);
        first = areaIndex[r_];
        areaIndex.Remove(first);

        int r2_ = Random.Range(0, areaIndex.Count);
        second = areaIndex[r2_];
        areaIndex.Remove(second);
    }

    [PunRPC]
    public void ForbiddenAreaSet(int firstArea_, int secondArea_)
    {
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
        areaSprite[firstArea_].color = new Color(1, 0, 0, 180 / 255);
        areaSprite[secondArea_].color = new Color(1, 197 / 255, 37 / 255, 180 / 255);
    }
    IEnumerator ForbiddenAreaSetStart(int areaIndex_)
    {
        yield return new WaitForSeconds(selectDelay);
        areaSprite[areaIndex_].color = new Color(1, 0, 0, 180);

        for (int i = 0; i < allArea[areaIndex_].transform.childCount; i++)
        {
            allArea[areaIndex_].transform.GetChild(i).GetComponent<SafetyArea>().enabled = false;
            allArea[areaIndex_].transform.GetChild(i).GetComponent<ForbiddenArea>().enabled = true;
        }
        announce.announceAudio.clip = announce.allAnnounce[2];
        announce.announceAudio.Play();
    }
    IEnumerator SelectDelay()
    {
        yield return new WaitForSeconds(selectDelay);
        ForbiddenAreaSelect();
        photonView.RPC("ForbiddenAreaSet", RpcTarget.All, first, second);
    }
}

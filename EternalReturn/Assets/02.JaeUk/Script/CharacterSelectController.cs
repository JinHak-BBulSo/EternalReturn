using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterSelectController : MonoBehaviourPun
{
    public GameObject canvas;
    [SerializeField]
    public int playerNumber;
    public int playerTotalNumber;

    string PlayerName;
    public bool isEnter = false;
    public bool isSelect;
    public int selectNumber;
    public int selectCharacterNum;
    public int selectViewID;

    // Start is called before the first frame update
    void Start()
    {
        canvas = PlayerManager.Instance.canvas;
        transform.SetParent(canvas.transform.GetChild(0), false);
        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayerList", RpcTarget.All);
            photonView.RPC("EnterGuest", RpcTarget.MasterClient);
        }
        selectViewID = transform.GetComponent<PhotonView>().ViewID;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect)
        {
            isSelect = false;
            PlayerManager.Instance.SelectChk = true;

        }
        if (PlayerManager.Instance.IsSelect)
        {
            PlayerManager.Instance.IsSelect = false;
            photonView.RPC("ReadyCheck", RpcTarget.All, selectViewID);
        }

    }
    [PunRPC]
    void EnterGuest()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            playerTotalNumber++;
        }
        else
        {

        }
    }
    [PunRPC]
    void SetPlayerList()
    {

        if (PhotonNetwork.IsMasterClient)
        {
        }
        else if (photonView.IsMine)
        {
            playerNumber++;
            isEnter = true;
            PlayerManager.Instance.PlayerNumber = playerNumber;

        }

    }
    [PunRPC]
    void ReadyCheck(int ViewID)
    {
        isSelect = true;
        PlayerManager.Instance.characterNum = selectCharacterNum;
        selectViewID = ViewID;


    }
    [PunRPC]
    void SetImageChange()
    {
        if (photonView.IsMine)
        {
        }

    }

    [PunRPC]
    void Select(int Selectnum)
    {

        if (PhotonNetwork.IsMasterClient)
        {

        }
        else
        {
            playerNumber++;
        }


    }
    [PunRPC]
    void ChageImage(int viewID, int characterNum)
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).GetComponent<PhotonView>().ViewID == viewID)
            {
                selectNumber = i;
            }
        }
        selectCharacterNum = characterNum;
    }
}

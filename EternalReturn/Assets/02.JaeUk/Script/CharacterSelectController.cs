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

    public int ReadyPlayerNum;

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
            PlayerManager.Instance.SelectChk = true;

        }
        if (PlayerManager.Instance.IsSelect)
        {
            PlayerManager.Instance.IsSelect = false;
            selectCharacterNum = PlayerManager.Instance.characterNum;
            photonView.RPC("ReadyCheck", RpcTarget.All, selectViewID, PlayerManager.Instance.characterNum);

        }

    }
    [PunRPC]
    void LoadingGame()
    {
        transform.parent.parent.GetComponent<CharacterSelect>().totalTime = 40;
        transform.parent.parent.GetComponent<AudioSource>().time = 45f;
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

        if (photonView.IsMine)
        {

            isEnter = true;
            PlayerManager.Instance.PlayerNumber++;

        }

    }
    [PunRPC]
    void ReadyCheck(int ViewID, int selectCharacterNum_)
    {
        int chk = 0;
        isSelect = true;
        selectCharacterNum = selectCharacterNum_;
        selectViewID = ViewID;
        ReadyPlayerNum++;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).GetComponent<CharacterSelectController>().ReadyPlayerNum == 1)
            {
                chk++;
            }
        }
        if (chk == transform.parent.childCount)
        {
            Debug.Log(transform.parent.childCount);
            photonView.RPC("LoadingGame", RpcTarget.All);
        }


    }



}

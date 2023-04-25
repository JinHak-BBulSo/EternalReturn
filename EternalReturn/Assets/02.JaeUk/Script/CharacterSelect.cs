using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CharacterSelect : MonoBehaviourPun
{
    [SerializeField]
    public string[] playerList = new string[4];
    [SerializeField]
    public int playerNumber;
    string PlayerName;
    bool isEnter = true;


    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("SetPlayerList", RpcTarget.All);
    }

    [PunRPC]
    void SetPlayerList()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            playerList[0] = "host";
        }
        else
        {
            playerList[0] = "user";
            playerList[1] = "host";
            isEnter = true;
        }

    }

    [PunRPC]
    void EnterGuest()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber++;
            playerList[playerNumber] = "User";
        }


    }


    // Update is called once per frame
    void Update()
    {
        if (isEnter && PhotonNetwork.IsMasterClient)
        {
            isEnter = false;
            photonView.RPC("EnterGuest", RpcTarget.MasterClient);
        }
    }
}


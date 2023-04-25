using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CharacterSelect : MonoBehaviourPun
{
    [SerializeField]
    public string[] playerList = new string[4];
    [SerializeField]
    public int playerNumber;
    string PlayerName;
    public bool isEnter = false;
    public Image[] PlayerSprite = null;
    public Sprite[] ImageSprite;
    public Button startBtn;
    public bool isSelect;
    public int selectNumber;


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
            playerNumber = 1;
            isEnter = true;
        }

    }

    [PunRPC]
    void EnterGuest()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber++;
            for (int i = playerNumber; i < playerList.Length; i++)
                playerList[playerNumber] = "User";
        }
        else
        {
            playerNumber++;
            for (int i = playerNumber; i < playerList.Length; i++)
                playerList[playerNumber] = "User";
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
            for (int i = playerNumber; i < playerList.Length; i++)
                playerList[playerNumber] = "User";
        }


    }


    // Update is called once per frame
    void Update()
    {
        if (isEnter)
        {
            isEnter = false;
            photonView.RPC("EnterGuest", RpcTarget.MasterClient);
        }
        if (isSelect)
        {
            isSelect = false;
            // photonView.RPC("");
        }




    }
    void OnClick()
    {
        PlayerSprite[0].sprite = ImageSprite[0];
        selectNumber = 0;
        isSelect = true;
    }
}


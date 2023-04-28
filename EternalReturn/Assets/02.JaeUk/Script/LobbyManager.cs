using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0.0";

    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("try to Enter the Server");
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Enter the Server");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Failed to Enter the Server. ReTrying... ");
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Try to Enter the room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Failed to Enter the Server. ReTrying... ");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Empty room doesn't exist, make a Empty Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Enter the Room");
        photonView.RPC("IncreasePlayer", RpcTarget.All);
    }
    [PunRPC]
    void IncreasePlayer()
    {
        RoomCheckManager.Instance.TotalPlayerNumber++;
    }
    // Update is called once per frame

}

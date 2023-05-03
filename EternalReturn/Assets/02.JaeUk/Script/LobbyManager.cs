using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0.0";
    public bool isChk = false;
    public GameObject startButton;
    public AudioSource audio;

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
        RoomCheckManager.Instance.isEnter = true;
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
        if (!isChk)
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Try to Enter the room");
                PhotonNetwork.JoinRandomRoom();
                isChk = true;
                audio.Play();
                startButton.transform.GetChild(2).GetComponent<Text>().text = "매칭 중";

            }
            else
            {
                Debug.Log("Failed to Enter the Server. ReTrying... ");
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                RoomCheckManager.Instance.TotalPlayerNumber--;
                startButton.transform.GetChild(2).GetComponent<Text>().text = "게임 시작";
                audio.Play();
                isChk = false;
            }
        }

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Empty room doesn't exist, make a Empty Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
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
        int number = RoomCheckManager.Instance.TotalPlayerNumber;
        number++;
        RoomCheckManager.Instance.TotalPlayerNumber = number;
        if (photonView.IsMine && RoomCheckManager.Instance.TotalPlayerNumber == 2)
        {
            photonView.RPC("SceneChanger", RpcTarget.All);
        }
    }
    [PunRPC]
    void SceneChanger()
    {
        PhotonNetwork.LoadLevel("CharacterSelect");
    }
    // Update is called once per frame
}

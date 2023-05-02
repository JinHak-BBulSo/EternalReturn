using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerInstanate : MonoBehaviourPun
{
    public int startPlayerNum;
    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("SetSingltonNum", RpcTarget.MasterClient);

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.LoadingFinishPlayer == 2)
        {
            Debug.Log(PlayerManager.Instance.LoadingFinishPlayer);
            PlayerManager.Instance.LoadingFinishPlayer = 0;
            photonView.RPC("InstanatePlayer", RpcTarget.All);
        }
        if (startPlayerNum == 2)
        {
            PlayerManager.Instance.IsGameStart = true;
            Destroy(this);
        }
    }
    [PunRPC]
    void SetSingltonNum()
    {
        PlayerManager.Instance.LoadingFinishPlayer++;
    }
    [PunRPC]
    void InstanatePlayer()
    {
        Debug.Log(PlayerManager.Instance.characterNum);
        switch (PlayerManager.Instance.characterNum)
        {
            case 0:
                PlayerManager.Instance.canvas = transform.gameObject;
                GameObject playerClone = PhotonNetwork.Instantiate("08.Player/Prefabs/Aya_S002", PlayerManager.Instance.PlayerPos, Quaternion.identity, 0);
                PlayerManager.Instance.Player = playerClone;
                playerClone.transform.SetParent(transform, false);
                break;
            case 1:
                PlayerManager.Instance.canvas = transform.gameObject;
                playerClone = PhotonNetwork.Instantiate("08.Player/Prefabs/Jackie_S003", PlayerManager.Instance.PlayerPos, Quaternion.identity, 0);
                PlayerManager.Instance.Player = playerClone;
                playerClone.transform.SetParent(transform, false);
                break;
        }
        startPlayerNum++;

    }
}

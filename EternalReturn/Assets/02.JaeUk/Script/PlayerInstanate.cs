using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerInstanate : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerManager.Instance.characterNum)
        {
            case 0:
                PlayerManager.Instance.canvas = transform.gameObject;
                GameObject playerClone = PhotonNetwork.Instantiate("08.Player/Prefabs/Jackie_S003", new Vector3(0, 0, 0), Quaternion.identity, 0);
                PlayerManager.Instance.Player = playerClone;
                playerClone.transform.SetParent(transform, false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

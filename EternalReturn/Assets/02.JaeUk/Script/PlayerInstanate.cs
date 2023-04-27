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
        GameObject playerClone = PhotonNetwork.Instantiate("Global/Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

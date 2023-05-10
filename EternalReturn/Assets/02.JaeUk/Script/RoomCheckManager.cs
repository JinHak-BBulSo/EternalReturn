using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RoomCheckManager : MonoBehaviourPun
{
    private static RoomCheckManager instance;

    public static RoomCheckManager Instance
    {
        get
        {
            if (instance == null)
            {

                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<RoomCheckManager>();

                }
            }
            return instance;
        }
    }
    public int TotalPlayerNumber;
    public bool isEnter = false;

    private void Update()
    {

    }


}

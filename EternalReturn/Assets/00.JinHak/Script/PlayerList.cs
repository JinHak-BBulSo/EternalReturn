using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    private static PlayerList instance;
    public Dictionary<int, PlayerBase> playerDictionary = new Dictionary<int, PlayerBase>();

    public static PlayerList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (PlayerList)FindObjectOfType(typeof(PlayerList));
            }
            return instance;
        }
    }
}

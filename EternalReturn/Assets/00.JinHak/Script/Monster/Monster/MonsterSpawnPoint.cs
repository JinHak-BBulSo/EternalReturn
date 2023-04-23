using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    public Monster monster = default;
    public Action enterPlayer;
    public Action exitPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            enterPlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            exitPlayer();
        }
    }
}

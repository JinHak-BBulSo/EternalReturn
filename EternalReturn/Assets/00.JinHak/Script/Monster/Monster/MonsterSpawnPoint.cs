using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MonsterSpawnPoint : MonoBehaviourPun
{
    public Monster monster = default;
    public Action enterPlayer;
    public Action exitPlayer;
    public float firstSpawnDelay = default;
    public float respawnDelay = default;
    public bool firstSpawn = false;
    public bool firstSpawnEnd = false;

    void Start()
    {
        monster = transform.GetChild(1).GetComponent<Monster>();
        monster.gameObject.SetActive(false);
        StartCoroutine(MonsterSpawnDelay(firstSpawnDelay));
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && !firstSpawn && !firstSpawnEnd)
        {
            StartCoroutine(MonsterSpawnDelay(firstSpawnDelay));
        }
        if (firstSpawnEnd && !firstSpawn)
        {
            firstSpawn = true;
            monster.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && enterPlayer != default)
        {
            enterPlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && enterPlayer != default)
        {
            exitPlayer();
        }
    }

    public void RespawnMonster()
    {
        StartCoroutine(MonsterSpawnDelay(respawnDelay));
    }

    IEnumerator MonsterSpawnDelay(float spawnDelay_)
    {
        yield return new WaitForSeconds(spawnDelay_);
        monster.gameObject.SetActive(true);
        photonView.RPC("FirstSpawn", RpcTarget.All);
    }
    [PunRPC]
    public void FirstSpawn()
    {
        firstSpawnEnd = true;
    }
}

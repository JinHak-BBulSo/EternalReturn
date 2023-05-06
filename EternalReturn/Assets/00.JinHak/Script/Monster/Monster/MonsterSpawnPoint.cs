using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    public Monster monster = default;
    public Action enterPlayer;
    public Action exitPlayer;
    public float firstSpawnDelay = default;
    public float respawnDelay = default;

    void Start()
    {
        monster = transform.GetChild(1).GetComponent<Monster>();
        monster.gameObject.SetActive(false);
        StartCoroutine(MonsterSpawnDelay(firstSpawnDelay));
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
    }
}

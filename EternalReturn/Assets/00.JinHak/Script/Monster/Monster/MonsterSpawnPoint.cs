using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    public Monster monster = default;
    public Action enterPlayer;
    public Action exitPlayer;
    public float spawnDelay = default;

    void Start()
    {
        monster = transform.GetChild(1).GetComponent<Monster>();
        monster.gameObject.SetActive(true);
    }

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

    public void RespawnMonster()
    {
        StartCoroutine(MonsterSpawnDelay(spawnDelay));
    }

    IEnumerator MonsterSpawnDelay(float spawnDelay_)
    {
        yield return new WaitForSeconds(spawnDelay_);
        monster.gameObject.SetActive(true);
    }
}

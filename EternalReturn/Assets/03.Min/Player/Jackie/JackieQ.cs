using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackieQ : MonoBehaviour
{
    [SerializeField]
    private Jackie player = default;
    private BoxCollider skillRange = default;
    public float firstAttackDamage = 0f;
    public float secondAttackDamage = 0f;
    public float bloodDamage = 0f;

    private void Start()
    {
        skillRange = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Monster>() != null)
        {
            Monster enemy = other.GetComponent<Monster>();
            player.enemyHunt.Add(enemy);
        }
        if (other.GetComponent<PlayerBase>() != null)
        {
            player.enemyPlayer.Add(other.GetComponent<PlayerBase>());
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.GetComponent<Monster>() != null)
    //     {
    //         player.enemyHunt.Remove(other.GetComponent<Monster>());
    //     }
    //     if (other.GetComponent<PlayerBase>() != null)
    //     {
    //         player.enemyPlayer.Remove(other.GetComponent<PlayerBase>());
    //     }
    // }
}

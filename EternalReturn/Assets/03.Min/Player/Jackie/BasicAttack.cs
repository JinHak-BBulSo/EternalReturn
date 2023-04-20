using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField]
    private Jackie player = default;
    private SphereCollider attackRange = default;

    private void Start()
    {
        attackRange = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.enemy == null && (other.GetComponent<Monster>() != null || other.GetComponent<PlayerBase>() != null))
        {
            player.enemy = other.gameObject;
        }
    }


}

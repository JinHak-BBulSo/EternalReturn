using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackieQ : MonoBehaviour
{
    private Jackie player = default;
    // private BoxCollider
    public float firstAttackDamage = 0f;
    public float secondAttackDamage = 0f;
    public float bloodDamage = 0f;

    private void Start()
    {
        player = GetComponent<Jackie>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }


}

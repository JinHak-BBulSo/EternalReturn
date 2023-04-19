using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterBattleArea : MonoBehaviour
{
    public Monster monster = default;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
        if(other.gameObject.Equals(monster.gameObject))
        {
            Debug.Log("아웃");
            monster.isBattleAreaOut = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterBattleArea : MonoBehaviour
{
    public Monster monster = default;

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(monster.gameObject))
        {
            monster.isBattleAreaOut = true;
        }
    }
}

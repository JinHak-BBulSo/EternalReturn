using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterBattleArea : MonoBehaviour
{
    public Monster monster = default;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            monster.isBattleAreaOut = true;
        }
    }
}

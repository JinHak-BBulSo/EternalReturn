using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public int type;
    public int rare;
    public string name;
    public int count;
    public float weaponAttackSpeedPercent;
    public float weaponAttackRangePercent;
    public float foodIncrease;
    public Stat ItemStat = new Stat();
    private void Start()
    {
        switch (type)
        {
            case 1:
                weaponAttackSpeedPercent = 0.58f;
                weaponAttackRangePercent = 1.05f;
                break;
            case 2:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.65f;
                break;
            case 3:
                weaponAttackSpeedPercent = 0.485f;
                weaponAttackRangePercent = 1.45f;
                break;
            case 4:
                weaponAttackSpeedPercent = 0.445f;
                weaponAttackRangePercent = 2.2f;
                break;
            case 5:
                weaponAttackSpeedPercent = 0.5f;
                weaponAttackRangePercent = 1.2f;
                break;
            case 6:
                weaponAttackSpeedPercent = 0.63f;
                weaponAttackRangePercent = 0.95f;
                break;
            case 7:
                weaponAttackSpeedPercent = 0.51f;
                weaponAttackRangePercent = 5.5f;
                break;
            case 8:
                weaponAttackSpeedPercent = 0.55f;
                weaponAttackRangePercent = 4.7f;
                break;
            case 9:
                weaponAttackSpeedPercent = 0.65f;
                weaponAttackRangePercent = 5.85f;
                break;
            case 10:
                weaponAttackSpeedPercent = 0.505f;
                weaponAttackRangePercent = 4.2f;
                break;
            case 11:
                weaponAttackSpeedPercent = 0.43f;
                weaponAttackRangePercent = 5.3f;
                break;
            case 12:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.55f;
                break;
            case 13:
                weaponAttackSpeedPercent = 0.61f;
                weaponAttackRangePercent = 1.25f;
                break;
            default:
                weaponAttackSpeedPercent = 0;
                break;

        }
    }


}


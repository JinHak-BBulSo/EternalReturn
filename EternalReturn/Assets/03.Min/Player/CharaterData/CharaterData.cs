using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharData", menuName = "Scriptable Object/CharData", order = int.MinValue)]
public class CharaterData : ScriptableObject
{
    public float hp;
    public float stamina;
    public float attackPower;
    public float defense;
    public float attackSpeed;
    public float moveSpeed;

    public float visionRange;
    public float attackRange;
    public float hpRegen;
    public float staminaRegen;

    public float increaseAttack;
    public float increaseDef;
    public float increaseHp;
    public float increaseHpRegen;
    public float increaseStamina;
    public float increaseStaminaRegen;


}

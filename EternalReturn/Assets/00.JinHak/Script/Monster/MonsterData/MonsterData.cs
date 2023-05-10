using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterData", order = 0)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    private float attackPower;
    public float AttackPower { get { return attackPower; } }
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    [SerializeField]
    private float defense;
    public float Defense { get { return defense; } }
    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed { get { return attackSpeed; } }
    [SerializeField]
    private float attackRange;
    public float AttackRange { get {  return attackRange; } }
    [SerializeField]
    private int monsterLevel;
    public int MonsterLevel { get {  return monsterLevel; } }
    [SerializeField]
    private int monsterMaxLevel;
    public int MonsterMaxLevel { get { return monsterMaxLevel; } }
    
}

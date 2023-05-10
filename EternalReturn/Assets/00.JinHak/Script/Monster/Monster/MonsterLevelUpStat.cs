using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterLevelUp", order = 0)]
public class MonsterLevelUpStat : ScriptableObject
{
    [SerializeField]
    private int levelUpAmount;
    public int LevelUpAmount { get { return levelUpAmount; } }
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
    private float levelUpCount;
    public float LevelUpCount { get {  return levelUpCount; } }
}

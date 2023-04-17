using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "MonsterStatus")]
public class MonsterStatus : ScriptableObject
{
    [SerializeField]
    private string monsterName = default;
    public string MonsterName { get { return name; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } }
    public int Hp { get { return hp; } }


    [SerializeField]
    private int hp;
    [SerializeField]
    private int def;
    public int Def { get { return def; } }

    [SerializeField]
    private int power;
    public int Power { get { return power; } }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return  moveSpeed; } }

    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed { get {  return attackSpeed; } }
}

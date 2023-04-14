using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    MonsterStatus monsterStatus = default;

    public string monsterName = default;
    public int maxHp;
    public int nowHp;
    public int def;
    public int power;
    public float moveSpeed;
    public float attackSpeed;
    public int level;

    // 영역 내 플레이어 캐싱
    

    void Start()
    {

    }

    void Update()
    {

    }
    
    protected virtual void SetStatus()
    {
        maxHp = monsterStatus.Hp;
        nowHp = maxHp;

        def = monsterStatus.Def;
        power = monsterStatus.Power;
        monsterName = monsterStatus.MonsterName;

        moveSpeed = monsterStatus.MoveSpeed;
        attackSpeed = monsterStatus.AttackSpeed;
    }
    public virtual void LevelUp()
    {
        /* each monster override using */
    }

    public virtual void Attack()
    {

    }
    
    public virtual void Move()
    {

    }

    public virtual void Recall()
    {

    }

    public virtual void Beware()
    {

    }

    public virtual void Skill()
    {

    }
    
    public void Die()
    {

    }
}

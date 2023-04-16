using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    MonsterStatus monsterStatus = default;
    private MonsterController monsterController;

    public string monsterName = default;
    public int maxHp;
    public int nowHp;
    public int def;
    public int power;
    public float moveSpeed;
    public float attackSpeed;
    public int level;
    public bool isSkillAble = false;
    public bool isAttackAble = false;

    // 영역 내 플레이어 캐싱
     

    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        SetStatus();
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

        isSkillAble = true;
    }
    public virtual void LevelUp()
    {
        /* each monster override using */
    }

    public virtual void Attack()
    {
        monsterController.monsterAni.SetBool("isAttack", true);
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
        /* each monster override using */
    }

    public void Die()
    {

    }

    public virtual void ExitAttack()
    {
        monsterController.monsterAni.SetBool("isAttack", false);
    }

    public virtual void ExitSkill()
    {
    }
}

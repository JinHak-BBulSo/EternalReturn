using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IHitHandler
{
    [SerializeField]
    private MonsterStatus monsterStatus = default;
    private MonsterController monsterController;
    [SerializeField]
    private MonsterSpawnPoint spawnPoint = default;

    public string monsterName = default;
    [SerializeField]
    private MonsterData monsterData;
    
    public bool isSkillAble = false;
    public bool isAttackAble = false;
    
    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        spawnPoint = transform.parent.GetComponent<MonsterSpawnPoint>();

        if (spawnPoint != null)
        {
            spawnPoint.monster = this;
        }

        SetStatus();
        Appear();
    }

    void Update()
    {

    }
    
    protected virtual void SetStatus()
    {
        monsterStatus.maxHp = monsterData.Hp;
        monsterStatus.nowHp = monsterStatus.maxHp;
        monsterStatus.defense = monsterData.Defense;
        monsterStatus.attackPower = monsterData.AttackPower;
        monsterStatus.attackSpeed = monsterData.AttackSpeed;
        monsterStatus.attackRange = monsterData.AttackRange;
        monsterStatus.moveSpeed = monsterData.MoveSpeed;

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
        monsterController.monsterAni.SetBool("isMove", true);
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

    public virtual void Appear()
    {
        monsterController.monsterAni.SetTrigger("Appear");
    }
    public virtual void ExitAppear()
    {
        monsterController.monsterAni.SetTrigger("EndAppear");
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

    public virtual void ExitBeware()
    {

    }
    
    public virtual void ExitRecall()
    {

    }
    public void HitDamage(float targetHp_, float power_)
    {
        throw new System.NotImplementedException();
    }
}

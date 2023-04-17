using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    public Debuff monsterDebuff = default;

    private float 
    
    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        spawnPoint = transform.parent.GetComponent<MonsterSpawnPoint>();
        monsterDebuff = GetComponent<Debuff>();

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

    /// <summary>
    /// 기본 공격 공식
    /// 공격력 * 기본공격증폭 = message.damageAmount
    /// (100 / (100 + 상대 방어력)) * message.damageAmount
    /// 스킬 공격 공식
    /// 
    /// 받는 피해량 공식
    /// (넘겨준 데미지 * ((100 - 피해감소)/100) 
    /// </summary>
    /// <param name="message"></param>
    public void TakeDamage(DamageMessage message)
    {
        if (message.debuffIndex == -1)
        {
            monsterStatus.nowHp -= (int)(message.damageAmount * (100 / (100 + monsterStatus.defense)));
        }
        else
        {
            StartCoroutine(IHitHandler.ContinousDamage(message, monsterDebuff, message.debuffIndex));
        }
    }

    public void TakeSolidDamage(DamageMessage message)
    {
        monsterStatus.nowHp -= message.damageAmount;
    }

    /// <summary>
    /// 출혈 데미지를 입히는 함수
    /// 5초간 1초간격으로 총 5회의 피해
    /// 제키의 Q의 경우 레벨당 16 22 28 34 40
    /// </summary>
    /// <param name="message"></param> // 입히는 데미지
    /// <param name="delay_"></param> // 피해 간격 출혈은 1초
    /// <param name="continuousTime_"></param> // 지속 시간 출혈은 5초
    /// <param name="debuff_"></param> // 몬스터의 디버프
    /// <returns></returns>
    IEnumerator IHitHandler.ContinousDamage(DamageMessage message, Debuff debuff_, int debuffIndex)
    {
        float time = 0;
        // 이미 상태이상이 걸린 경우
        if (debuff_.applyDebuffCheck[debuffIndex])
        {
            debuff_.continousTime[debuffIndex] = 0;
            yield break;
        }
        // 상태이상이 걸려있지 않은 경우
        else
        {
            float delay_ = debuff_.debuffDelayTime[debuffIndex];
            while (time > debuff_.continousTime[debuffIndex])
            {
                TakeDamage(message);
                yield return new WaitForSeconds(delay_);
                debuff_.continousTime[debuffIndex] += delay_;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour, IHitHandler
{
    public MonsterStatus monsterStatus = default;
    private MonsterController monsterController;

    private MonsterSpawnPoint spawnPoint = default;
    public GameObject monsterBattleArea = default;

    public string monsterName = default;
    [SerializeField]
    private MonsterData monsterData;
    
    public bool isSkillAble = false;
    public bool isAttackAble = false;
    public bool isBattleAreaOut = false;

    public bool[] applyDebuffCheck = new bool[10];      // 해당 디버프가 걸렸는지 체크
    public float[] debuffContinousTime = new float[10]; // 디버프 유지 시간
    public float[] debuffDelayTime = new float[10];     // 디버프 틱 간격
    public float[] debuffRemainTime = new float[10];    // 디버프 남은 시간
    public float[] debuffDamage = new float[10];        // 디버프 데미지
    public Queue<float>[] debuffDamageQueues = new Queue<float>[10];
    public List<float>[] debuffRemainList = new List<float>[10];

    private PlayerBase firstAttackPlayer = default;

    void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        spawnPoint = transform.GetChild(1).GetComponent<MonsterSpawnPoint>();
        monsterBattleArea = spawnPoint.transform.GetChild(0).gameObject;

        monsterStatus = new MonsterStatus();

        if (spawnPoint != null)
        {
            spawnPoint.monster = this;
        }

        monsterController.AwakeOrder();
    }

    void OnEnable()
    {
        spawnPoint.enterPlayer -= EnterPlayer;
        spawnPoint.enterPlayer += EnterPlayer;

        spawnPoint.exitPlayer -= ExitPlayer;
        spawnPoint.exitPlayer += ExitPlayer;

        Appear();
        SetStatus();
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
    public virtual void Recall()
    {
        isBattleAreaOut = true;
        monsterController.monsterAni.SetBool("isMove", true);
    }

    public virtual void Beware()
    {
        monsterController.monsterAni.SetBool("isBeware", true);
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
        monsterController.monsterAni.SetTrigger("Die");
    }

    public virtual void ExitAttack()
    {
        DamageMessage dm = new DamageMessage(gameObject, monsterStatus.attackPower);
        monsterController.targetPlayer.TakeDamage(dm);
        monsterController.monsterAni.SetBool("isAttack", false);
    }

    public virtual void ExitSkill()
    {
        monsterController.monsterAni.SetBool("isSkill", false);
    }

    public virtual void ExitBeware()
    {
        monsterController.monsterAni.SetBool("isBeware", false);
    }
    
    public virtual void ExitRecall()
    {
        isBattleAreaOut = false;
        monsterController.monsterAni.SetBool("isMove", false);
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
        if(firstAttackPlayer == default)
        {
            firstAttackPlayer = message.causer.GetComponent<PlayerBase>();
            monsterController.targetPlayer = firstAttackPlayer;
        }
        monsterStatus.nowHp -= (int)(message.damageAmount * (100 / (100 + monsterStatus.defense)));
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
    public void TakeSolidDamage(DamageMessage message)
    {
        monsterStatus.nowHp -= message.damageAmount;
    }


    public void TakeSolidDamage(DamageMessage message, float damageAmount)
    {
        monsterStatus.nowHp -= damageAmount;
    }

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    public IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_)
    {
        // 이미 상태이상이 걸린 경우
        if (applyDebuffCheck[debuffIndex_])
        {
            debuffRemainTime[debuffIndex_] = debuffContinousTime[debuffIndex_];
        }
        // 상태이상이 걸려있지 않은 경우
        else
        {
            // 상태이상 남은 시간 기록
            debuffRemainTime[debuffIndex_] = debuffContinousTime[debuffIndex_];
            // 상태이상 데미지를 저장
            debuffDamage[debuffIndex_] = message.damageAmount;

            // 상태이상 틱 간격
            float delayTime_ = 0;

            //float resetDamageCount = 0;

            while (debuffRemainTime[debuffIndex_] > 0)
            {
                // 프레임마다 틱타임 계산
                delayTime_ += Time.deltaTime;

                // 프레임마다 지속시간 감소
                debuffRemainTime[debuffIndex_] -= Time.deltaTime;
                // 프레임마다 리셋시간 증가
                //resetDamageCount += Time.deltaTime;

                // 딜레이 시간이 다 되었을시 대미지를 입힘
                if(delayTime_ > debuffDelayTime[debuffIndex_])
                {
                    TakeSolidDamage(message, debuffDamage[debuffIndex_]);
                    delayTime_ = 0;
                }

                yield return null;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            debuffDamage[debuffIndex_] = 0;
        }
    }

    public void EnterPlayer()
    {
        monsterController.encountPlayerCount++;
    }
    public void ExitPlayer()
    {
        monsterController.encountPlayerCount--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public CharaterData charaterData = default;
    public PlayerStat playerStat = default;
    public Transform attackRange = default;

    private void Start()
    {
        InitStat();
    }

    private void Update()
    {
        ShowAttackRange(playerStat.attackRange);
    }

    // 스탯 초기값 할당
    private void InitStat()
    {
        playerStat.attackPower = charaterData.attackPower; // 공격력
        playerStat.defense = charaterData.defense; // 방어력
        playerStat.attackSpeed = charaterData.attackSpeed; // 공격속도
        playerStat.moveSpeed = charaterData.moveSpeed; // 이동속도
        playerStat.visionRange = charaterData.visionRange; // 시야
        playerStat.attackRange = charaterData.attackRange; // 공격범위
        playerStat.maxHp = charaterData.hp; // 체력 
        playerStat.maxStamina = charaterData.stamina; // 스태미나
        playerStat.hpRegen = charaterData.hpRegen; // hp젠
        playerStat.staminaRegen = charaterData.staminaRegen; // 스태미나젠
    }

    private void LevelUp(PlayerExp playerExp_)
    {
        if (playerExp_.level >= playerExp_.maxLevel)
        {
            return;
        }
        while (true)
        {
            if (playerExp_.maxExp > playerExp_.nowExp)
            {
                break;
            }
            else
            {
                playerExp_.level++;
                playerExp_.nowExp -= playerExp_.maxExp;
                playerExp_.maxExp += playerExp_.expDelta;
            }
        }
    }

    public void GetExp(float exp_)
    {

    }
    public void Craft()
    {

    }


    private void Attack(PlayerStat playerStat_)
    {
        // 애니메이션 실행
    }

    protected virtual void ShowAttackRange(float attackRange_)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRange.gameObject.SetActive(true);
            attackRange.localScale = new Vector3(0.01f * attackRange_ * 2f, 0.01f * attackRange_ * 2f, 0.01f);
        }
    }

    private void Rest()
    {
        playerStat.nowHp += playerStat.maxHp * 10f;
    }

    private IEnumerator MotionDelay(float attackDelay_)
    {
        // 공격불가 시간
        yield return new WaitForSeconds(attackDelay_);
    }
}

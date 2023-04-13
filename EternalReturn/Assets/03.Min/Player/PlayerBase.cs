using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private PlayerController playerController = default;
    public CharaterData charaterData = default;
    public PlayerStat playerStat = default;
    public Transform attackRange = default;
    public bool isAttackAble = true;
    public bool isMove = false;
    private Vector3 destination = default;
    public Animator playerAni = default;
    public int attackType = 0;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAni = gameObject.GetComponent<Animator>();
        InitStat();
    }

    private void Update()
    {
        ShowAttackRange();
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }
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
    public virtual void Attack()
    {
        switch (attackType)
        {
            case 0:
                playerAni.SetBool("isAttack", true);
                playerAni.SetFloat("AttackType", attackType);
                attackType = 1;
                break;
            case 1:
                playerAni.SetBool("isAttack", true);
                playerAni.SetFloat("AttackType", attackType);
                attackType = 0;
                break;
        }
    }

    protected virtual void ShowAttackRange()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRange.gameObject.SetActive(true);
            attackRange.localScale = new Vector3(0.01f * playerStat.attackRange * 4f, 0.01f * playerStat.attackRange * 4f, 0.01f);
        }
    }



    public void Move()
    {
        if (isMove)
        {
            if (Vector3.Distance(destination, transform.position) <= 0.1f)
            {
                isMove = false;
                return;
            }
            var dir = destination - transform.position;
            // transform.LookAt(destination);
            Quaternion viewRoate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, viewRoate, 6f * Time.deltaTime);
            transform.position += dir.normalized * Time.deltaTime * playerStat.moveSpeed;
        }
    }

    private void SetDestination(Vector3 dest_)
    {
        destination = dest_;
        isMove = true;
    }



    public void Rest()
    {
        playerStat.nowHp += playerStat.maxHp * 10f;
    }

    private void AttackEnd()
    {
        playerController.ChangeState(new PlayerIdle());
        StartCoroutine(MotionDelay(playerStat.attackSpeed));

    }

    IEnumerator MotionDelay(float attackDelay_)
    {
        // 공격불가 시간
        isAttackAble = false;
        yield return new WaitForSeconds(attackDelay_);
        isAttackAble = true;
    }
}

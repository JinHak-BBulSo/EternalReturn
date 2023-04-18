using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IHitHandler
{
    private PlayerController playerController = default;
    private Vector3 destination = default;
    [SerializeField]
    protected GameObject weapon = default;
    public Transform attackRange = default;
    public GameObject Skill_Q_Range = default;
    public CharaterData charaterData = default;
    public PlayerStat playerStat = default;
    [HideInInspector]
    public Animator playerAni = default;
    public bool isAttackAble = true;
    public bool isMove = false;
    public int attackType = 0;
    public bool isAttackRangeShow = false;
    public bool[] skillCooltimes = new bool[5];




    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAni = gameObject.GetComponent<Animator>();
        InitStat();
    }

    protected virtual void Update()
    {

        ShowAttackRange();
        DisableAttackRange();
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
        playerAni.SetFloat("MotionSpeed", playerStat.attackSpeed);
        switch (attackType)
        {
            case 0:
                playerAni.SetBool("isAttack", true);
                playerAni.SetFloat("AttackType", attackType);
                playerController.ChangeState(new PlayerIdle());
                break;
            case 1:
                playerAni.SetBool("isAttack", true);
                playerAni.SetFloat("AttackType", attackType);
                playerController.ChangeState(new PlayerIdle());
                break;
        }
    }

    private void SkillEnd()
    {
        playerController.ChangeState(new PlayerIdle());
    }
    private void AttackEnd()
    {
        isAttackAble = false;
        AnimatorStateInfo currentAnimationState = playerAni.GetCurrentAnimatorStateInfo(0);
        float delay_ = currentAnimationState.length - currentAnimationState.length * currentAnimationState.normalizedTime;
        switch (attackType)
        {
            case 0:
                attackType = 1;
                break;
            case 1:
                attackType = 0;
                break;
        }
        StartCoroutine(MotionDelay(delay_));
    }

    private void AttackAniEnd()
    {
        playerAni.SetBool("isAttack", false);
    }

    IEnumerator MotionDelay(float delay_)
    {
        // 공격불가 시간
        Debug.Log(delay_);
        yield return new WaitForSeconds(delay_);
        isAttackAble = true;
    }

    protected virtual void ShowAttackRange()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRange.gameObject.SetActive(true);
            attackRange.localScale = new Vector3(0.01f * playerStat.attackRange * 4f, 0.01f * playerStat.attackRange * 4f, 0.01f);
            isAttackRangeShow = true;
        }
    }

    protected virtual void DisableAttackRange()
    {
        if (isAttackRangeShow)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                attackRange.gameObject.SetActive(false);
            }
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
        playerStat.nowHp += playerStat.maxHp * 0.1f;
    }

    public virtual void Skill_Q() { }

    public virtual void Skill_W() { }

    public virtual void Skill_E() { }

    public virtual void Skill_R() { }

    public virtual void Skill_D() { }

    public virtual void Skill_T() { }

    public void TakeDamage(DamageMessage message)
    {
        playerStat.nowHp = playerStat.nowHp - ((message.damageAmount - playerStat.defense) * (100 - playerStat.damageReduce) / 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageMessage dm = new DamageMessage(gameObject, playerStat.attackPower);
        IHitHandler test = other.GetComponent<IHitHandler>();
        if (test != null)
        {
            test.TakeDamage(dm);
        }
    }
}

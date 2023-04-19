using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [HideInInspector]
    public NavMeshAgent playerNav = default;
    public NavMeshPath path = default;
    public bool isAttackAble = true;
    public bool isMove = false;
    public int attackType = 0;
    public bool isAttackRangeShow = false;
    public bool[] skillCooltimes = new bool[5];
    public bool[] applyDebuffCheck = new bool[10];      // 해당 디버프가 걸렸는지 체크
    public float[] debuffContinousTime = new float[10]; // 디버프 유지 시간
    public float[] debuffDelayTime = new float[10];     // 디버프 틱 간격
    public float[] debuffRemainTime = new float[10];    // 디버프 남은 시간
    public float[] debuffDamage = new float[10];        // 디버프 데미지
    public Queue<float>[] debuffDamageQueues = new Queue<float>[10];
    public List<float>[] debuffRemainList = new List<float>[10];
    public List<Vector3> corners = new List<Vector3>();
    private int currentCorner = 0;




    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAni = GetComponent<Animator>();
        playerNav = GetComponent<NavMeshAgent>();
        Camera.main.transform.parent.GetComponent<MoveCamera>().player = this;
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
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(hit.point, out navHit, 5.0f, NavMesh.AllAreas))
                {
                    // destination = new Vector3(navHit.position.x, hit.point.y, navHit.position.z);
                    SetDestination(new Vector3(navHit.position.x, hit.point.y, navHit.position.z));
                    path = new NavMeshPath();
                    playerNav.CalculatePath(destination, path);

                    corners.Clear();
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        corners.Add(path.corners[i]);
                    }
                    currentCorner = 0;
                }
                //SetDestination(hit.point);
            }
            Debug.Log(hit);

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

    public void MoveCheck()
    {
        if (playerNav.enabled)
        {
            if (playerNav.remainingDistance <= playerNav.stoppingDistance)
            {
                playerNav.enabled = false;
            }
        }
        else
        {
            playerNav.enabled = true;
            playerNav.SetDestination(destination);
        }
    }

    public void Move()
    {
        //if (playerNav.enabled)
        //{
        //    float distance = Vector3.Distance(
        //        new Vector3(transform.position.x, 0, transform.position.y),
        //        new Vector3(destination.x, 0, destination.y)
        //        );
        //    if (distance <= playerNav.stoppingDistance)
        //    {
        //        Debug.Log("도착");
        //        playerNav.enabled = false;
        //        isMove = false;
        //        playerNav.ResetPath();
        //    }
        //}
        if (isMove)
        {
            if (corners.Count > 0 && currentCorner < corners.Count)
            {
                if (Vector3.Distance(corners[currentCorner], transform.position) <= 0.2f)
                {
                    currentCorner++;
                }
                if (currentCorner < corners.Count)
                {
                    var dir = corners[currentCorner] - transform.position;
                    Quaternion viewroate = Quaternion.LookRotation(dir);
                    viewroate = Quaternion.Euler(transform.rotation.x, viewroate.eulerAngles.y, transform.rotation.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, viewroate, 6f * Time.deltaTime);
                    transform.position += dir.normalized * Time.deltaTime * playerStat.moveSpeed;
                }
                else
                {
                    isMove = false;
                }
            }
            // else
            // {
            //     if (Vector3.Distance(destination, transform.position) <= 0.2f)
            //     {
            //         isMove = false;
            //         return;
            //     }
            //     var dir = destination - transform.position;
            //     Quaternion viewroate = Quaternion.LookRotation(dir);
            //     viewroate = Quaternion.Euler(transform.rotation.x, viewroate.eulerAngles.y, transform.rotation.z);
            //     transform.rotation = Quaternion.Slerp(transform.rotation, viewroate, 6f * Time.deltaTime);
            //     transform.position += dir.normalized * Time.deltaTime * playerStat.moveSpeed;
            // }
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

    private void OnTriggerEnter(Collider other)
    {

        // if (other.CompareTag("Skill"))
        // {
        //     // DamageMessage dm = other.GetComponent<Skill>().dm;
        //     // DamageMessage debuffdm = other.GetComponent<Skill>()

        //     TakeDamage(dm);

        //     if(debuffdm != default){
        //         ContinousDamage();
        //     }
        // }
        // DamageMessage dm = new DamageMessage(gameObject, playerStat.attackPower);
        // IHitHandler enemy = other.GetComponent<IHitHandler>();
        // Debug.Log("?");
        // if (enemy != null)
        // {
        //     enemy.TakeDamage(dm);
        // }
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
        playerStat.nowHp -= (int)(message.damageAmount * (100 / (100 + playerStat.defense)));
        Debug.Log(playerStat.nowHp);
    }
    public void TakeDamage(DamageMessage message, float damageAmount)
    {
        throw new System.NotImplementedException();
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
        playerStat.nowHp -= message.damageAmount;
    }


    public void TakeSolidDamage(DamageMessage message, float damageAmount)
    {
        playerStat.nowHp -= damageAmount;
    }

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    public IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_, float continousTime_)
    {
        // 이미 상태이상이 걸린 경우
        if (applyDebuffCheck[debuffIndex_])
        {
            StartCoroutine(ContinousDamageEnd(continousTime_, debuffIndex_, message.damageAmount));
            debuffDamage[debuffIndex_] += message.damageAmount;

            if (continousTime_ > debuffRemainTime[debuffIndex_])
                debuffRemainTime[debuffIndex_] = continousTime_;
        }
        // 상태이상이 걸려있지 않은 경우
        else
        {
            // 상태이상 남은 시간 기록
            debuffRemainTime[debuffIndex_] = continousTime_;
            // 상태이상 데미지를 저장
            debuffDamage[debuffIndex_] = message.damageAmount;

            // 상태이상 틱 간격
            float delayTime_ = 0;
            StartCoroutine(ContinousDamageEnd(continousTime_, debuffIndex_, message.damageAmount));

            while (debuffRemainTime[debuffIndex_] > 0)
            {
                // 프레임마다 틱타임 계산
                delayTime_ += Time.deltaTime;

                // 프레임마다 지속시간 감소
                debuffRemainTime[debuffIndex_] -= Time.deltaTime;
                // 프레임마다 리셋시간 증가
                //resetDamageCount += Time.deltaTime;

                // 딜레이 시간이 다 되었을시 대미지를 입힘
                if (delayTime_ > debuffDelayTime[debuffIndex_])
                {
                    TakeSolidDamage(message, debuffDamage[debuffIndex_]);
                    delayTime_ = 0;
                }

                yield return null;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            debuffDamage[debuffIndex_] = 0;
            applyDebuffCheck[debuffIndex_] = false;
        }
    }

    IEnumerator ContinousDamageEnd(float debuffContinousTime_, int debuffIndex_, float debuffDamage_)
    {
        yield return new WaitForSeconds(debuffContinousTime_);
        debuffDamage[debuffIndex_] -= debuffDamage_;
    }
}

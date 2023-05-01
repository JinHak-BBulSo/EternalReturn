using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBase : MonoBehaviour, IHitHandler
{
    protected PlayerController playerController = default;
    protected Vector3 destination = default;
    protected int currentCorner = 0;
    public List<PlayerBase> enemyPlayer = new List<PlayerBase>();
    public List<Monster> enemyHunt = new List<Monster>();
    public Vector3 nowMousePoint = default;
    public GameObject weapon = default;
    public GameObject fishingRod = default;

    public GameObject enemy = default;

    //[KJH] Add. 마우스 클릭 타겟 기록 및 외곽선 표시
    public GameObject clickTarget = default;
    public Outline outline = default;

    public Transform attackRange = default;
    public GameObject[] SkillRange = new GameObject[5];
    public CharaterData charaterData = default;
    public PlayerStat playerStat = default;
    public PlayerStat extraStat = default;
    public PlayerStat playerTotalStat = default;

    [HideInInspector]
    public Animator playerAni = default;
    [HideInInspector]
    public NavMeshAgent playerNav = default;
    public NavMeshPath path = default;
    public bool isAttackAble = true;
    public bool isMove = false;
    public bool isAttackMove = false;
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
    private SpriteRenderer[] attackRangeRender = new SpriteRenderer[2];

    //[KJH] Add. MiniMap move
    private Camera miniMapCamera = default;
    public GameObject stunFBX = default;
    public GameObject itemBoxUi = default;
    public ItemBoxSlotList itemBoxSlotList = default;
    private bool isMoveAble = true;

    protected virtual void Start()
    {
        transform.SetParent(PlayerManager.Instance.canvas.transform, false);
        ItemManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
        playerAni = GetComponent<Animator>();
        playerNav = GetComponent<NavMeshAgent>();
        Camera.main.transform.parent.GetComponent<MoveCamera>().player = this;
        attackRangeRender[0] = attackRange.GetComponent<SpriteRenderer>();
        attackRangeRender[1] = attackRange.transform.GetChild(0).GetComponent<SpriteRenderer>();
        InitStat();
        //KJH Add. MinimapCamera Add
        miniMapCamera = Camera.main.transform.parent.GetChild(1).GetComponent<Camera>();
        //KJH Add. Each Player InventoryBoxUi Add

        //GameObject itemBoxUi_ = Resources.Load<GameObject>("06.ItemBox/Prefab/ItemBoxUI/ItemBoxUi");

        itemBoxUi = Instantiate(itemBoxUi, GameObject.Find("TestUi").transform);
        itemBoxSlotList = itemBoxUi.transform.GetChild(0).GetChild(4).GetComponent<ItemBoxSlotList>();

        stunFBX = transform.GetChild(2).gameObject;
    }


    protected virtual void Update()
    {
        ShowAttackRange();
        DisableAttackRange();
        RaycastHit mousePoint;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mousePoint);
        nowMousePoint = mousePoint.point;

        if (isMoveAble && Input.GetMouseButtonDown(1) || (isAttackMove && Input.GetMouseButtonDown(0)))
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                NavMeshHit navHit;
                //[KJH] Add. 마우스 클릭 타겟 기록
                clickTarget = hit.collider.gameObject;

                if (clickTarget.GetComponent<Outline>() != null && clickTarget.GetComponent<Outline>().monster != null)
                {
                    Monster monster = clickTarget.GetComponent<Outline>().monster;
                    enemy = monster.gameObject;
                    isAttackMove = true;
                    playerController.ChangeState(new PlayerAttackMove());
                }
                //[KJH] Add. 클릭 타겟 Outline 표시
                //외곽선 초기화
                if (outline != default)
                {
                    outline.enabled = false;
                    outline.isClick = false;
                    outline = default;
                }
                if (clickTarget.GetComponent<Outline>() != null)
                {
                    outline = clickTarget.GetComponent<Outline>();
                    outline.isClick = true;
                    outline.enabled = true;
                }

                if (NavMesh.SamplePosition(hit.point, out navHit, 5.0f, NavMesh.AllAreas))
                {
                    // destination = new Vector3(navHit.position.x, hit.point.y, navHit.position.z);
                    //SetDestination(new Vector3(navHit.position.x, hit.point.y, navHit.position.z));
                    //[KJH] ADD. destionation yPos 변경
                    SetDestination(new Vector3(navHit.position.x, navHit.position.y, navHit.position.z));

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

            // MiniMap Click Player Move
            if (Physics.Raycast(miniMapCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 clickPos = Input.mousePosition;
                if (clickPos.x < 625 || clickPos.x > 735 ||
                    clickPos.y < 10 || clickPos.y > 110)
                {
                    return;
                }
                //if(clickPos.x < 625 || clickPos.x > 735)
                //630, 10 ~ 110 734.5

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(hit.point, out navHit, 5.0f, NavMesh.AllAreas))
                {
                    // destination = new Vector3(navHit.position.x, hit.point.y, navHit.position.z);
                    //SetDestination(new Vector3(navHit.position.x, hit.point.y, navHit.position.z));
                    //[KJH] ADD. destionation yPos 변경
                    SetDestination(new Vector3(navHit.position.x, navHit.position.y, navHit.position.z));

                    path = new NavMeshPath();
                    playerNav.CalculatePath(destination, path);
                    corners.Clear();
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        corners.Add(path.corners[i]);
                    }
                    currentCorner = 0;
                }
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
        playerStat.nowHp = playerStat.maxHp;
        playerStat.nowStamina = playerStat.maxStamina;
    }

    public void AddExtraStat() // 아이템 추가스텟
    {
        extraStat.attackPower = ItemManager.Instance.equipmentTotalState.attackPower;
        extraStat.defense = ItemManager.Instance.equipmentTotalState.defense;
        extraStat.armorReduce = ItemManager.Instance.equipmentTotalState.armorReduce;
        extraStat.attackRange = ItemManager.Instance.equipmentTotalState.attackRange;
        extraStat.attackSpeed = ItemManager.Instance.equipmentTotalState.attackSpeed;
        extraStat.basicAttackPower = ItemManager.Instance.equipmentTotalState.basicAttackPower;
        extraStat.coolDown = ItemManager.Instance.equipmentTotalState.coolDown;
        extraStat.criticalDamage = ItemManager.Instance.equipmentTotalState.criticalDamage;
        extraStat.criticalPercent = ItemManager.Instance.equipmentTotalState.criticalPercent;
        extraStat.damageReduce = ItemManager.Instance.equipmentTotalState.damageReduce;
        extraStat.extraHp = ItemManager.Instance.equipmentTotalState.extraHp;
        extraStat.extraStamina = ItemManager.Instance.equipmentTotalState.extraStamina;
        extraStat.hpRegen = ItemManager.Instance.equipmentTotalState.hpRegen;
        extraStat.lifeSteel = ItemManager.Instance.equipmentTotalState.lifeSteel;
        extraStat.staminaRegen = ItemManager.Instance.equipmentTotalState.staminaRegen;
        extraStat.moveSpeed = ItemManager.Instance.equipmentTotalState.moveSpeed;
        extraStat.skillPower = ItemManager.Instance.equipmentTotalState.skillPower;
        extraStat.tenacity = ItemManager.Instance.equipmentTotalState.tenacity;
        extraStat.visionRange = ItemManager.Instance.equipmentTotalState.visionRange;
    }

    public void AddTotalStat() // 플레이어 총 스탯
    {
        AddExtraStat();
        playerTotalStat.attackPower = playerStat.attackPower + extraStat.attackPower;
        playerTotalStat.defense = playerStat.defense + extraStat.defense;
        playerTotalStat.armorReduce = playerStat.armorReduce + extraStat.armorReduce;
        playerTotalStat.attackRange = playerStat.attackRange + extraStat.attackRange + ItemManager.Instance.equipmentTotalState.weaponAttackRangePercent;
        playerTotalStat.attackSpeed = (playerStat.attackSpeed + extraStat.attackSpeed) + ItemManager.Instance.equipmentTotalState.weaponAttackSpeedPercent;
        playerTotalStat.basicAttackPower = playerStat.basicAttackPower + extraStat.basicAttackPower;
        playerTotalStat.coolDown = playerStat.coolDown + extraStat.coolDown;
        playerTotalStat.criticalDamage = playerStat.criticalDamage + extraStat.criticalDamage;
        playerTotalStat.criticalPercent = playerStat.criticalPercent + extraStat.criticalPercent;
        playerTotalStat.damageReduce = playerStat.damageReduce + extraStat.damageReduce;
        playerTotalStat.extraHp = playerStat.extraHp + extraStat.extraHp;
        playerTotalStat.extraStamina = playerStat.extraStamina + extraStat.extraStamina;
        playerTotalStat.hpRegen = playerStat.hpRegen + extraStat.hpRegen;
        playerTotalStat.lifeSteel = playerStat.lifeSteel + extraStat.lifeSteel;
        playerTotalStat.staminaRegen = playerStat.staminaRegen + extraStat.staminaRegen;
        playerTotalStat.moveSpeed = playerStat.moveSpeed + extraStat.moveSpeed;
        playerTotalStat.skillPower = playerStat.skillPower + extraStat.skillPower;
        playerTotalStat.tenacity = playerStat.tenacity + extraStat.tenacity;
        playerTotalStat.visionRange = playerStat.visionRange + extraStat.visionRange;
        playerTotalStat.maxHp = playerStat.maxHp + playerTotalStat.extraHp;
        playerTotalStat.maxStamina = playerStat.maxStamina + playerTotalStat.extraStamina;
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
        playerAni.SetFloat("MotionSpeed", playerTotalStat.attackSpeed);
        transform.LookAt(enemy.transform);
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

    public virtual void ExtraAni()
    {
    }
    private void MotionStart()
    {
        playerAni.SetBool("skillStart", true);
    }

    public virtual void MotionEnd()
    {
        playerController.ResetAni();
        playerController.ResetRange();
    }

    private void SkillEnd()
    {
        playerController.ChangeState(new PlayerIdle());
    }
    protected virtual void AttackEnd()
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
        // Debug.Log(delay_);
        yield return new WaitForSeconds(delay_);
        isAttackAble = true;
    }

    protected virtual void ShowAttackRange()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRangeRender[0].color = new Color(attackRangeRender[0].color.r, attackRangeRender[0].color.g, attackRangeRender[0].color.b, 0.5f);
            attackRangeRender[1].color = new Color(attackRangeRender[1].color.r, attackRangeRender[1].color.g, attackRangeRender[1].color.b, 1f);
            attackRange.localScale = new Vector3(0.01f * playerTotalStat.attackRange * 4f, 0.01f * playerTotalStat.attackRange * 4f, 0.01f);
            isAttackRangeShow = true;
        }
    }


    protected virtual void DisableAttackRange()
    {
        if (isAttackRangeShow)
        {

            if (Input.GetMouseButtonDown(0))
            {
                attackRangeRender[0].color = new Color(attackRangeRender[0].color.r, attackRangeRender[0].color.g, attackRangeRender[0].color.b, 0f);
                attackRangeRender[1].color = new Color(attackRangeRender[1].color.r, attackRangeRender[1].color.g, attackRangeRender[1].color.b, 0f);
                isAttackRangeShow = false;
                isAttackMove = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                attackRangeRender[0].color = new Color(attackRangeRender[0].color.r, attackRangeRender[0].color.g, attackRangeRender[0].color.b, 0f);
                attackRangeRender[1].color = new Color(attackRangeRender[1].color.r, attackRangeRender[1].color.g, attackRangeRender[1].color.b, 0f);
                isAttackRangeShow = false;
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
                    transform.position += dir.normalized * Time.deltaTime * playerTotalStat.moveSpeed;
                }
                else
                {
                    isMove = false;
                    isAttackMove = false;
                    playerController.ChangeState(new PlayerIdle());
                    if (playerController.playerState == PlayerController.PlayerState.IDLE)
                    {
                        playerAni.SetBool("isMove", false);
                    }

                }
            }
        }
    }

    protected void SetDestination(Vector3 dest_)
    {
        destination = dest_;
        isMove = true;
    }
    public virtual void ExtraRange()
    {
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
        playerStat.nowHp -= (int)(message.damageAmount * (100 / (100 + playerTotalStat.defense)));
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
    public IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_, float continousTime_, float tickTime_)
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
            applyDebuffCheck[debuffIndex_] = true;
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
                if (delayTime_ > tickTime_)
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

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    /// 
    public void Debuff(DamageMessage message, int debuffIndex_, float continousTime_)
    {
        StartCoroutine(DebuffStart(message, debuffIndex_, continousTime_));
    }
    public IEnumerator DebuffStart(DamageMessage message, int debuffIndex_, float continousTime_)
    {
        // 이미 상태이상이 걸린 경우
        if (applyDebuffCheck[debuffIndex_])
        {
            if (continousTime_ > debuffRemainTime[debuffIndex_])
                debuffRemainTime[debuffIndex_] = continousTime_;
        }
        // 상태이상이 걸려있지 않은 경우
        else
        {
            // 상태이상 남은 시간 기록
            debuffRemainTime[debuffIndex_] = continousTime_;

            switch (debuffIndex_)
            {
                // 스턴
                case 2:
                    isMove = false;
                    isMoveAble = false;
                    playerController.enabled = false;
                    stunFBX.SetActive(true);
                    yield return null;
                    break;
                // 속박
                case 3:
                    isMove = false;
                    isMoveAble = false;
                    yield return null;
                    break;
            }

            while (debuffRemainTime[debuffIndex_] > 0)
            {
                // 프레임마다 지속시간 감소
                debuffRemainTime[debuffIndex_] -= Time.deltaTime;

                // 상태이상 종류 체크
                if (debuffIndex_ == 3 || debuffIndex_ == 4) isMove = false;

                yield return null;
            }

            // 디버프 종류
            switch (debuffIndex_)
            {
                // 스턴
                case 2:
                    stunFBX.SetActive(false);
                    isMoveAble = true;
                    playerController.enabled = true;
                    break;
                // 속박
                case 3:
                    isMoveAble = true;
                    break;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            applyDebuffCheck[debuffIndex_] = false;
        }
    }

    IEnumerator ContinousDamageEnd(float debuffContinousTime_, int debuffIndex_, float debuffDamage_)
    {
        yield return new WaitForSeconds(debuffContinousTime_);
        debuffDamage[debuffIndex_] -= debuffDamage_;
    }
}

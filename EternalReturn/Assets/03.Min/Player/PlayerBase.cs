using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System.ComponentModel;

public class PlayerBase : MonoBehaviourPun, IHitHandler
{
    public enum PlayerSound
    {
        NONE = -1,
        ATTACK,
        BOXOPEN,
        COLLECTBRUCNH,
        COLLECTFISH,
        COLLECTFLOWER,
        COLLECTSTONE,
        COLLECTWATER,
        CRAFTEPIC,
        CRAFTRARE,
        CRAFTUNCOMMON,
        DIE,
        HYPERLOOP,
        KILLMONSTER,
        FIRSTWEAPONLEARN,
        SECONDWEAPONLEARN,
        MOVE,
        SKILLQ,
        SKILLW,
        SKILLE,
        SKILLR,
        SKILLD,
        REST,
        VICTORY
    }
    protected PlayerController playerController = default;
    public PlayerController PlayerController { get { return playerController; } }
    protected Vector3 destination = default;
    public Rigidbody playerRigid = default;
    public Vector3 Destination { get { return destination; } }
    public int currentCorner = 0;
    public List<PlayerBase> enemyPlayer = new List<PlayerBase>();
    public List<Monster> enemyHunt = new List<Monster>();
    public Vector3 nowMousePoint = default;
    public GameObject weapon = default;
    public GameObject fishingRod = default;
    public GameObject craftTool = default;
    public GameObject hammer = default;
    public GameObject driver = default;

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
    public float[] debuffRemainTime = new float[10];    // 디버프 남은 시간
    public float[] debuffDamage = new float[10];        // 디버프 데미지

    public List<Vector3> corners = new List<Vector3>();
    private SpriteRenderer[] attackRangeRender = new SpriteRenderer[2];
    //[KJH] Add. MiniMap move
    private Camera miniMapCamera = default;
    public GameObject stunFBX = default;
    public GameObject itemBoxUi = default;
    public ItemBoxSlotList itemBoxSlotList = default;
    private bool isMoveAble = true;
    public int[] item = new int[6];
    public AudioClip[] audioClips = default;
    protected AudioSource playerAudio = default;
    //[KJH] Add. PlayerStatusUi
    public GameObject mainUi = default;
    public GameObject playerStatusUiPrefab = default;
    public PlayerStatusUI playerStatusUi = default;
    public GameObject worldCanvas = default;
    public PlayerSkillSystem skillSystem = default;
    public int weaponType = 0;
    private float regenTime = 0f;
    public int huntKill = 0;
    public int playerKill = 0;
    public int[] SkillPoint = new int[6];
    private Outline playerOutLine = default;
    public Image castingBar = default;
    public bool isInForbiddenArea = false;
    public int forbiddenCount = 45;
    public float forbiddenDelay = 0;
    public Text forbiddenCountTxt;
    public GameObject forbiddenEnterImage;
    public AudioSource restrictSound = default;

    //[KJH] Add. Each Player Index
    public int playerIndex = -1;

    protected virtual void Start()
    {
        // 플레이어 물리 상태 초기화
        playerRigid = GetComponent<Rigidbody>();
        playerRigid.useGravity = false;
        playerRigid.velocity = Vector3.zero;

        playerOutLine = GetComponent<Outline>();
        playerOutLine.player = this;
        playerOutLine.enabled = false;
        playerAudio = GetComponent<AudioSource>();
        transform.SetParent(PlayerManager.Instance.canvas.transform, false);
        playerController = GetComponent<PlayerController>();
        playerAni = GetComponent<Animator>();
        playerNav = GetComponent<NavMeshAgent>();
        attackRangeRender[0] = attackRange.GetComponent<SpriteRenderer>();
        attackRangeRender[1] = attackRange.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //KJH Add. MinimapCamera Add
        miniMapCamera = Camera.main.transform.parent.GetChild(1).GetComponent<Camera>();
        //KJH Add. Each Player InventoryBoxUi Add

        //GameObject itemBoxUi_ = Resources.Load<GameObject>("06.ItemBox/Prefab/ItemBoxUI/ItemBoxUi");

        mainUi = GameObject.Find("Main UI Canvas");
        itemBoxUi = mainUi.transform.GetChild(3).gameObject;
        itemBoxSlotList = itemBoxUi.transform.GetChild(0).GetChild(4).GetComponent<ItemBoxSlotList>();
        craftTool.transform.GetChild(0).GetComponent<CraftTool>().craftPlayer = this;
        stunFBX = transform.GetChild(2).gameObject;
        castingBar = mainUi.transform.GetChild(4).GetChild(1).GetComponent<Image>();

        restrictSound = GameObject.Find("RestrictSound").GetComponent<AudioSource>();
        worldCanvas = GameObject.Find("WorldCanvas");
        playerStatusUi = Instantiate(playerStatusUiPrefab, worldCanvas.transform).GetComponent<PlayerStatusUI>();
        playerStatusUi.player = this;

        //[KJH] ADD. PlayerIndex 구분
        playerIndex = photonView.ViewID;
        PlayerList.Instance.playerDictionary.Add(playerIndex, this);
        forbiddenCountTxt = mainUi.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        forbiddenEnterImage = mainUi.transform.GetChild(0).GetChild(3).gameObject;
        PlayerList.Instance.playerCount++;

        if (photonView.IsMine)
        {
            ItemManager.Instance.Player = this;
            GetComponent<AudioListener>().enabled = true;
            gameObject.AddComponent<AllyFowUnit>();
            FowManager.Instance.InitMap();
        }
        else
        {
            gameObject.AddComponent<FoeFowUnit>();
        }

        skillSystem = GetComponent<PlayerSkillSystem>();
        InitStat();
        SkillPoint[5] = 1;
    }


    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            if (ItemManager.Instance.isEquipmentChang)
            {
                ItemManager.Instance.isEquipmentChang = false;
                AddTotalStat();
                ItemChang();
            }

        }

        if (photonView.IsMine && isInForbiddenArea && forbiddenCount != 0)
        {
            forbiddenEnterImage.SetActive(true);
            forbiddenDelay += Time.deltaTime;
            if (forbiddenDelay >= 1)
            {
                restrictSound.Play();
                forbiddenCount--;
                forbiddenDelay = 0;
                forbiddenCountTxt.text = forbiddenCount.ToString();

                if (forbiddenCount == 0)
                {
                    playerStat.nowHp = 0;
                }
            }
        }
        else if (photonView.IsMine && !isInForbiddenArea)
        {
            forbiddenEnterImage.SetActive(false);
        }

        if (photonView.IsMine)
        {

            if (playerStat.nowHp <= 0 && PlayerController.playerState != PlayerController.PlayerState.DIE)
            {
                playerStat.nowHp = 0;
                playerController.ChangeState(new PlayerDie());
                return;
            }
            TestLevelUp();
            ShowAttackRange();
            DisableAttackRange();
            Regen();
            Skill_T();
            PlayerUI.Instance.UpdateHpUI(playerStat.nowHp, playerTotalStat.maxHp);
            PlayerUI.Instance.UpdateSpUI(playerStat.nowStamina, playerTotalStat.maxStamina);
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
                    enemy = default;

                    //[KJH] Add. 클릭 타겟 Outline 표시
                    //외곽선 초기화
                    if (outline != default)
                    {
                        outline.enabled = false;
                        outline.isClick = false;
                        outline = default;
                    }
                    if (clickTarget.GetComponent<Outline>() != null && clickTarget.gameObject != this.gameObject)
                    {
                        outline = clickTarget.GetComponent<Outline>();
                        outline.isClick = true;
                        outline.enabled = true;
                    }

                    if (clickTarget.GetComponent<Outline>() != null && clickTarget.GetComponent<Outline>().monster != null)
                    {
                        Monster monster = clickTarget.GetComponent<Outline>().monster;
                        if (!monster.isDie)
                        {
                            enemy = monster.gameObject;
                            isAttackMove = true;
                            playerController.ChangeState(new PlayerAttackMove());
                        }
                    }
                    else if (clickTarget.GetComponent<Outline>() != null && clickTarget.GetComponent<Outline>().player != null && clickTarget.gameObject != this.gameObject)
                    {
                        PlayerBase player = clickTarget.GetComponent<Outline>().player;
                        if (playerController.playerState != PlayerController.PlayerState.DIE)
                        {
                            enemy = player.gameObject;
                            isAttackMove = true;
                            playerController.ChangeState(new PlayerAttackMove());
                        }
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
    }

    private void TestLevelUp()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetExp(1000, PlayerStat.PlayerExpType.WEAPON);
        }
    }

    public void PlayAudio(PlayerSound playerSound_)
    {
        if (!playerAudio.isPlaying)
        {
            playerAudio.clip = audioClips[(int)playerSound_];
            playerAudio.Play();
        }
    }

    // 스탯 초기값 할당
    protected virtual void InitStat()
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
        extraStat = new PlayerStat();
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] != 0)
            {
                extraStat.attackPower += ItemManager.Instance.itemList[item[i] - 1].attackPower;
                extraStat.defense += ItemManager.Instance.itemList[item[i] - 1].defense;
                extraStat.armorReduce += ItemManager.Instance.itemList[item[i] - 1].armorReduce;
                extraStat.attackRange += ItemManager.Instance.itemList[item[i] - 1].attackRange;
                extraStat.attackSpeed += ItemManager.Instance.itemList[item[i] - 1].attackSpeed;
                extraStat.basicAttackPower += ItemManager.Instance.itemList[item[i] - 1].basicAttackPower;
                extraStat.coolDown += ItemManager.Instance.itemList[item[i] - 1].coolDown;
                extraStat.criticalDamage += ItemManager.Instance.itemList[item[i] - 1].criticalDamage;
                extraStat.criticalPercent += ItemManager.Instance.itemList[item[i] - 1].criticalPercent;
                extraStat.damageReduce += ItemManager.Instance.itemList[item[i] - 1].damageReduce;
                extraStat.extraHp += ItemManager.Instance.itemList[item[i] - 1].extraHp;
                extraStat.extraStamina += ItemManager.Instance.itemList[item[i] - 1].extraStamina;
                extraStat.hpRegen += ItemManager.Instance.itemList[item[i] - 1].hpRegen;
                extraStat.lifeSteel += ItemManager.Instance.itemList[item[i] - 1].lifeSteel;
                extraStat.staminaRegen += ItemManager.Instance.itemList[item[i] - 1].staminaRegen;
                extraStat.moveSpeed += ItemManager.Instance.itemList[item[i] - 1].moveSpeed;
                extraStat.skillPower += ItemManager.Instance.itemList[item[i] - 1].skillPower;
                extraStat.tenacity += ItemManager.Instance.itemList[item[i] - 1].tenacity;
                extraStat.visionRange += ItemManager.Instance.itemList[item[i] - 1].visionRange;
            }

        }

    }

    public void AddTotalStat() // 플레이어 총 스탯
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < 6; i++)
            {
                item[i] = ItemManager.Instance.equipmentInven[i].id;
            }
        }


        if (item[0] != 0)
        {
            AddExtraStat();
            playerTotalStat.attackPower = playerStat.attackPower + extraStat.attackPower;
            playerTotalStat.defense = playerStat.defense + extraStat.defense;
            playerTotalStat.armorReduce = playerStat.armorReduce + extraStat.armorReduce;
            playerTotalStat.attackRange = playerStat.attackRange + extraStat.attackRange + ItemManager.Instance.itemList[item[0] - 1].weaponAttackRangePercent;
            playerTotalStat.attackSpeed = (playerStat.attackSpeed + extraStat.attackSpeed) + ItemManager.Instance.itemList[item[0] - 1].weaponAttackSpeedPercent;
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
                if (playerExp_ != playerStat.playerExp)
                {
                    playerStat.playerExp.nowExp += playerExp_.maxExp;
                    LevelUp(playerStat.playerExp);
                    playerStatusUi.playerExpBar.fillAmount = playerExp_.nowExp / playerExp_.maxExp;
                }
                else
                {
                    playerStat.attackPower += charaterData.increaseAttack;
                    playerStat.defense += charaterData.increaseDef;
                    playerStat.maxHp += charaterData.increaseHp;
                    playerStat.nowHp += charaterData.increaseHp;
                    playerStat.hpRegen += charaterData.increaseHpRegen;
                    playerStat.maxStamina += charaterData.increaseStamina;
                    playerStat.nowStamina += charaterData.increaseStamina;
                    playerStat.hpRegen += charaterData.increaseStaminaRegen;
                    AddTotalStat();
                    photonView.RPC("SetPlayerStat", RpcTarget.All, playerStat.playerExp.level, playerStat.nowHp, playerStat.nowStamina, item, SkillPoint);
                    photonView.RPC("SendLevelUp", RpcTarget.All);
                }
                playerExp_.nowExp -= playerExp_.maxExp;
                playerExp_.maxExp += playerExp_.expDelta;
                if (photonView.IsMine)
                {
                    PlayerUI.Instance.UpdatePlayerLevelUI(playerStat.playerExp.level);
                    PlayerUI.Instance.UpdateExpUI(playerStat.playerExp.nowExp, playerStat.playerExp.maxExp);
                    PlayerUI.Instance.UpdateSkillLevelUpUI(skillSystem.GetTotalSkillLevel(), skillSystem.GetWeaponSkillLevel(), playerStat.playerExp.level, playerStat.weaponExp.level);
                }
            }
        }
    }
    [PunRPC]
    public void SendLevelUp()
    {
        playerStatusUi.playerLevelTxt.text = $"{playerStat.playerExp.level}";
    }
    public void GetExp(float exp_, PlayerStat.PlayerExpType exptype_)
    {
        switch (exptype_)
        {
            case PlayerStat.PlayerExpType.WEAPON:
                playerStat.weaponExp.nowExp += exp_;
                LevelUp(playerStat.weaponExp);
                break;
            case PlayerStat.PlayerExpType.CRAFT:
                playerStat.craftExp.nowExp += exp_;
                LevelUp(playerStat.craftExp);
                break;
            case PlayerStat.PlayerExpType.SEARCH:
                playerStat.searchExp.nowExp += exp_;
                LevelUp(playerStat.searchExp);
                break;
            case PlayerStat.PlayerExpType.MOVE:
                playerStat.moveExp.nowExp += exp_;
                LevelUp(playerStat.moveExp);
                break;
            case PlayerStat.PlayerExpType.HEALTH:
                playerStat.healthExp.nowExp += exp_;
                LevelUp(playerStat.healthExp);
                break;
            case PlayerStat.PlayerExpType.DEF:
                playerStat.defExp.nowExp += exp_;
                LevelUp(playerStat.defExp);
                break;
        }
    }


    public virtual void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }
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
        if (!photonView.IsMine)
        {
            return;
        }
        playerAni.SetBool("skillStart", true);
    }

    public virtual void MotionEnd()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        playerController.ResetAni();
        playerController.ResetRange();
    }

    private void SkillEnd()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        playerController.ChangeState(new PlayerIdle());
    }
    protected virtual void AttackEnd()
    {
        if (!photonView.IsMine)
        {
            return;
        }
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
        if (!photonView.IsMine)
        {
            return;
        }
        playerAni.SetBool("isAttack", false);
    }

    IEnumerator MotionDelay(float delay_)
    {

        // 공격불가 시간
        // Debug.Log(delay_);
        yield return new WaitForSeconds(delay_);
        isAttackAble = true;

        if (isAttackAble && enemy != default)
        {
            playerController.ChangeState(new PlayerAttackMove());
        }
    }

    protected virtual void ShowAttackRange()
    {
        if (!photonView.IsMine)
        {
            return;
        }
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
        if (!photonView.IsMine)
        {
            return;
        }
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
        if (!photonView.IsMine)
        {
            return;
        }
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
        if (!photonView.IsMine)
        {
            return;
        }
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

    public void SetDestination(Vector3 dest_)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        destination = dest_;
        isMove = true;
    }
    public virtual void ExtraRange()
    {
    }

    public void Regen()
    {
        if (playerController.playerState == PlayerController.PlayerState.DIE)
        {
            return;
        }
        if (regenTime >= 0.5f)
        {
            regenTime = 0f;
            if (playerStat.nowHp < playerTotalStat.maxHp)
            {
                playerStat.nowHp += playerTotalStat.hpRegen * 0.5f;
                if (playerStat.nowHp >= playerTotalStat.maxHp)
                {
                    playerStat.nowHp = playerTotalStat.maxHp;
                }
            }
            if (playerStat.nowStamina < playerTotalStat.maxStamina)
            {
                playerStat.nowStamina += playerTotalStat.staminaRegen * 0.5f;
                if (playerStat.nowStamina >= playerTotalStat.maxStamina)
                {
                    playerStat.nowStamina = playerTotalStat.maxStamina;
                }
            }
            photonView.RPC("SetPlayerStat", RpcTarget.All, playerStat.nowHp, playerStat.nowStamina);
        }
        regenTime += Time.deltaTime;

    }



    public virtual void Skill_Q()
    {
        PlayAudio(PlayerSound.SKILLQ);
        if (!photonView.IsMine)
        {
            return;
        }
    }


    public virtual void Skill_W()
    {
        PlayAudio(PlayerSound.SKILLW);
        if (!photonView.IsMine)
        {
            return;
        }
    }

    public virtual void Skill_E()
    {
        PlayAudio(PlayerSound.SKILLE);
        if (!photonView.IsMine)
        {
            return;
        }
    }

    public virtual void Skill_R()
    {
        PlayAudio(PlayerSound.SKILLR);

        if (!photonView.IsMine)
        {
            return;
        }
    }

    public virtual void Skill_D()
    {
        PlayAudio(PlayerSound.SKILLD);
        if (!photonView.IsMine)
        {
            return;
        }
    }

    public virtual void Skill_T()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    [PunRPC]
    public void DieCheck(int playerIndex_)
    {
        PlayerBase player_ = PlayerList.Instance.playerDictionary[playerIndex_];
        if (playerStat.nowHp <= 0)
        {
            playerStat.nowHp = 0;
            player_.playerKill++;
            playerController.ChangeState(new PlayerDie());
            return;
        }
    }
    [PunRPC]
    public void DieCheckMonster()
    {
        if (playerStat.nowHp <= 0)
        {
            playerStat.nowHp = 0;
            playerController.ChangeState(new PlayerDie());
            return;
        }
    }
    [PunRPC]
    public void GetDefExp(int playerIndex_, float exp_)
    {
        PlayerBase player_ = PlayerList.Instance.playerDictionary[playerIndex_];
        player_.GetExp(exp_, PlayerStat.PlayerExpType.DEF);
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
        float totalDamageAmount = (int)(message.damageAmount * (100 / (100 + playerTotalStat.defense)));

        playerStat.nowHp -= totalDamageAmount;
        playerStatusUi.playerHpBar.fillAmount = playerStat.nowHp / playerStat.maxHp;

        photonView.RPC("SetPlayerStat", RpcTarget.All, playerStat.nowHp, playerStat.nowStamina);
        if (message.causer.GetComponent<PlayerBase>() != default)
        {
            photonView.RPC("DieCheck", RpcTarget.All, message.causer.GetComponent<PlayerBase>().playerIndex);
        }
        else
        {
            photonView.RPC("DieCheckMonster", RpcTarget.All);
        }
        if (message.causer.CompareTag("Enemy"))
        {
            photonView.RPC("GetDefExp", RpcTarget.All, playerIndex, totalDamageAmount * 0.1f);
        }
        else if (message.causer.CompareTag("Player"))
        {
            photonView.RPC("GetDefExp", RpcTarget.All, playerIndex, totalDamageAmount * 0.6f);
        }
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
        playerStatusUi.playerHpBar.fillAmount = playerStat.nowHp / playerStat.maxHp;
        photonView.RPC("SetPlayerStat", RpcTarget.All, playerStat.nowHp, playerStat.nowStamina);
        if (message.causer.GetComponent<PlayerBase>() != default)
        {
            photonView.RPC("DieCheck", RpcTarget.All, message.causer.GetComponent<PlayerBase>().playerIndex);
        }
        else
        {
            photonView.RPC("DieCheckMonster", RpcTarget.All);
        }
    }


    public void TakeSolidDamage(DamageMessage message, float damageAmount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerStat.nowHp -= damageAmount;
            playerStatusUi.playerHpBar.fillAmount = playerStat.nowHp / playerStat.maxHp;
            photonView.RPC("SetPlayerStat", RpcTarget.All, playerStat.nowHp, playerStat.nowStamina);
            if (message.causer.GetComponent<PlayerBase>() != default)
            {
                photonView.RPC("DieCheck", RpcTarget.All, message.causer.GetComponent<PlayerBase>().playerIndex);
            }
            else
            {
                photonView.RPC("DieCheckMonster", RpcTarget.All);
            }
        }
    }

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    [PunRPC]
    public void ContinousDamageStarteSet(int debuffIndex_, float time_)
    {
        applyDebuffCheck[debuffIndex_] = true;
        debuffRemainTime[debuffIndex_] = time_;
    }
    [PunRPC]
    public void ContinousDamageEndSet(int debuffIndex_)
    {
        applyDebuffCheck[debuffIndex_] = false;
        debuffRemainTime[debuffIndex_] = 0;
    }
    public IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_, float continousTime_, float tickTime_)
    {
        // 이미 상태이상이 걸린 경우
        if (applyDebuffCheck[debuffIndex_])
        {
            StartCoroutine(ContinousDamageEnd(continousTime_, debuffIndex_, message.damageAmount));
            debuffDamage[debuffIndex_] += message.damageAmount;

            if (continousTime_ > debuffRemainTime[debuffIndex_])
            {
                debuffRemainTime[debuffIndex_] = continousTime_;
                photonView.RPC("ContinousDamageStarteSet", RpcTarget.All, debuffIndex_, continousTime_);
            }
        }
        // 상태이상이 걸려있지 않은 경우
        else
        {
            photonView.RPC("ContinousDamageStarteSet", RpcTarget.All, debuffIndex_, continousTime_);
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
                    Debug.Log("출혈틱댐");
                    TakeSolidDamage(message, debuffDamage[debuffIndex_]);
                    delayTime_ = 0;
                }

                yield return null;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            debuffDamage[debuffIndex_] = 0;
            applyDebuffCheck[debuffIndex_] = false;
            photonView.RPC("ContinousDamageEndSet", RpcTarget.All, debuffIndex_);
        }
    }

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박, 4 = 에어본
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    /// 
    [PunRPC]
    public void Debuff(int debuffIndex_, float continousTime_)
    {
        photonView.RPC("DebuffSet", RpcTarget.All, debuffIndex_, continousTime_);
    }
    [PunRPC]
    public void DebuffSet(int debuffIndex_, float continousTime_)
    {
        StartCoroutine(DebuffStart(debuffIndex_, continousTime_));
    }
    public IEnumerator DebuffStart(int debuffIndex_, float continousTime_)
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
                    break;
                // 속박
                case 3:
                    isMove = false;
                    isMoveAble = false;
                    break;
                // 에어본
                case 4:
                    isMove = false;
                    isMoveAble = false;
                    playerController.player.playerNav.enabled = false;
                    playerRigid.useGravity = true;
                    playerController.GetComponent<Rigidbody>().AddForce(new Vector3(0, 6, 0), ForceMode.Impulse);
                    break;
            }

            while (debuffRemainTime[debuffIndex_] > 0)
            {
                // 프레임마다 지속시간 감소
                debuffRemainTime[debuffIndex_] -= Time.deltaTime;

                // 상태이상 종류 체크
                if (debuffIndex_ == 2 || debuffIndex_ == 3) isMove = false;
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
                // 에어본
                case 4:
                    isMoveAble = true;
                    playerRigid.useGravity = false;
                    playerRigid.velocity = Vector3.zero;
                    playerController.player.playerNav.enabled = true;
                    playerController.enabled = true;
                    break;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            applyDebuffCheck[debuffIndex_] = false;
        }
    }
    public void ItemChang()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetPlayerStat", RpcTarget.MasterClient,
            playerStat.playerExp.level, playerStat.nowHp, playerStat.nowStamina, item, SkillPoint);
        }
        else
        {
            photonView.RPC("SetPlayerStat", RpcTarget.Others,
             playerStat.playerExp.level, playerStat.nowHp, playerStat.nowStamina, item, SkillPoint);
        }
    }
    void MasterSpread()
    {
        photonView.RPC("SetPlayerStat", RpcTarget.Others,
                     playerStat.playerExp.level, playerStat.nowHp, playerStat.nowStamina, item, SkillPoint);
    }

    IEnumerator ContinousDamageEnd(float debuffContinousTime_, int debuffIndex_, float debuffDamage_)
    {
        yield return new WaitForSeconds(debuffContinousTime_);
        debuffDamage[debuffIndex_] -= debuffDamage_;
    }
    [PunRPC]
    public void SetAnimationBool(string name, bool flag)
    {
        playerAni.SetBool(name, flag);
    }
    [PunRPC]
    public void SetPlayerStat(int level_, float hp_, float mp_, int[] item_, int[] Skill_)
    {
        playerStat.playerExp.level = level_;
        playerStat.nowHp = hp_;
        playerStat.nowStamina = mp_;
        item = item_;
        AddTotalStat();
        if (PhotonNetwork.IsMasterClient)
        {
            MasterSpread();
        }

    }
    [PunRPC]
    public void SetPlayerStat(float hp_, float mp_)
    {
        playerStat.nowHp = hp_;
        playerStat.nowStamina = mp_;

    }

    private void OnMouseEnter()
    {
        if (!photonView.IsMine)
        {
            playerOutLine.enabled = true;
        }
        /*if (this.gameObject != PlayerManager.Instance.Player)
        {
        }*/
    }
    private void OnMouseExit()
    {
        if (!photonView.IsMine)
        {
            playerOutLine.enabled = false;
        }
        /*if (this.gameObject != PlayerManager.Instance.Player)
        {
        }*/
    }
}

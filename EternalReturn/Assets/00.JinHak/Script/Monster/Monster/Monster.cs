using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : MonoBehaviourPun, IHitHandler
{
    public MonsterStatus monsterStatus = default;
    protected MonsterController monsterController;

    private MonsterSpawnPoint spawnPoint = default;
    public MonsterSpawnPoint SpawnPoint { get { return spawnPoint; } }
    private MonsterBattleArea monsterBattleArea = default;
    public MonsterBattleArea MonsterBattleArea { get { return monsterBattleArea; } }

    public Outline outline = default;

    public string monsterName = default;
    [SerializeField]
    private MonsterData monsterData;

    public bool isBattleAreaOut = false;

    public bool[] applyDebuffCheck = new bool[2];      // 해당 디버프가 걸렸는지 체크
    public float[] debuffDelayTime = new float[2];     // 디버프 틱 간격
    public float[] debuffRemainTime = new float[2];    // 디버프 남은 시간
    public float[] debuffDamage = new float[2];        // 디버프 데미지
    
    public PlayerBase firstAttackPlayer = default;
    public bool isDie = false;

    public GameObject worldCanvas = default;
    public ItemBox monsterItemBox = default;
    public GameObject monsterUiPrefab = default;
    public GameObject monsterStatusUi = default;
    public Image monsterHpBar = default;
    private Text monsterLeveltxt = default;
    private Vector3 statusUiOffset = new Vector3(-0.3f, 2.7f, 0);

    private bool isFirstSpawn = false;

    public AudioSource audioSource = default;
    // 0 = 어택, 1 = 히트, 2 = 사망, 3 = 레디
    public List<AudioClip> sounds = new List<AudioClip>();

    void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        spawnPoint = transform.parent.GetComponent<MonsterSpawnPoint>();
        monsterBattleArea = spawnPoint.transform.GetChild(0).GetComponent<MonsterBattleArea>();
        audioSource = GetComponent<AudioSource>();
        monsterItemBox = GetComponent<ItemBox>();

        spawnPoint.monster = this;
        monsterBattleArea.monster = this;

        monsterStatus = new MonsterStatus();
        outline = GetComponent<Outline>();
        SetStatus();

        if (spawnPoint != null)
        {
            spawnPoint.monster = this;
        }

        monsterController.AwakeOrder();
        worldCanvas = GameObject.Find("WorldCanvas");

        if(monsterUiPrefab != default)
        {
            monsterStatusUi = Instantiate(monsterUiPrefab, worldCanvas.transform);
            monsterHpBar = monsterStatusUi.transform.GetChild(1).GetComponent<Image>();
            monsterLeveltxt = monsterStatusUi.transform.GetChild(2).GetChild(0).GetComponent<Text>();
            monsterStatusUi.transform.position = transform.position + statusUiOffset;
            monsterStatusUi.SetActive(false);
        }
    }

    [PunRPC]
    public void MonsterBoxSet(int index_)
    {
        monsterItemBox.AddItem(index_);
    }

    private void Update()
    {
        monsterStatusUi.transform.position = transform.position + statusUiOffset;
    }

    void OnEnable()
    {
        monsterItemBox.boxItems.Clear();

        spawnPoint.enterPlayer -= EnterPlayer;
        spawnPoint.enterPlayer += EnterPlayer;

        spawnPoint.exitPlayer -= ExitPlayer;
        spawnPoint.exitPlayer += ExitPlayer;

        spawnPoint = transform.parent.GetComponent<MonsterSpawnPoint>();
        monsterBattleArea = spawnPoint.transform.GetChild(0).GetComponent<MonsterBattleArea>();

        spawnPoint.monster = this;
        monsterBattleArea.monster = this;
        monsterController.monster.isDie = false;

        Appear();
        SetStatus();

        monsterController.navMeshAgent.enabled = true;
        monsterController.monster.monsterItemBox.enabled = false;
        monsterController.targetPlayer = default;
        firstAttackPlayer = default;

        if (!isFirstSpawn)
        {
            isFirstSpawn = true;
        }
        else
        {
            monsterStatusUi.SetActive(true);
        }

        monsterLeveltxt.text = monsterStatus.level.ToString();
        monsterHpBar.fillAmount = 1;

        //몬스터 아이템 셋팅
        monsterItemBox.SetItems(0);

        if (monsterItemBox.itemPrefabs.Count > 1)
        {
            int r_ = Random.Range(1, monsterItemBox.itemPrefabs.Count);
            int itemIndex_ = monsterItemBox.itemPrefabs[r_].GetComponent<ItemController>().item.id;
            photonView.RPC("MonsterBoxSet", RpcTarget.All, itemIndex_);
        }
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
        monsterStatus.level = monsterData.MonsterLevel;
        monsterStatus.maxLevel = monsterData.MonsterMaxLevel;
    }

    protected virtual void SetDebuffData()
    {
        
    }
    public virtual void LevelUp()
    {
        /* each monster override using */
    }

    public virtual void Skill()
    {
        /* each monster override using */
        
        // 공격형 스킬의 경우 예시
        /*GameObject target_ = monsterController.gameObject;
        float damageAmount_ = monsterController.monster.monsterStatus.attackPower;
        DamageMessage dm = new DamageMessage(target_, damageAmount_);
        monsterController.targetPlayer.TakeDamage(dm);*/
    }
    public void SoundPlay()
    {
        audioSource.Play();
    }
    public virtual void Appear()
    {
        monsterController.monsterAni.SetTrigger("Appear");
        audioSource.clip = sounds[3];
    }
    public virtual void ExitAppear()
    {
        monsterController.monsterAni.SetTrigger("EndAppear");
    }

    public void FirstAttackCheck(DamageMessage message)
    {
        if (firstAttackPlayer == default)
        {
            firstAttackPlayer = message.causer.GetComponent<PlayerBase>();
            monsterController.targetPlayer = firstAttackPlayer;
        }
        else
        {
            return;
        }
    }

    public void DieCheck(PlayerBase attackPlayer_)
    {
        if (monsterStatus.nowHp < 0)
        {
            monsterStatus.nowHp = 0;
            photonView.RPC("Die", RpcTarget.All);
            attackPlayer_.GetExp(50, PlayerStat.PlayerExpType.WEAPON);
            attackPlayer_.huntKill++;
            attackPlayer_.enemy = default;
        }
    }

    [PunRPC]
    public void Die()
    {
        isDie = true;
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
        FirstAttackCheck(message);
        float damageAmount_ = (int)(message.damageAmount * (100 / (100 + monsterStatus.defense)));

        if (PhotonNetwork.IsMasterClient)
        {
            if (message.debuffIndex == -1)
                monsterStatus.nowHp -= damageAmount_;
            monsterHpBar.fillAmount = monsterStatus.nowHp / monsterStatus.maxHp;
            photonView.RPC("SetMonsterStat", RpcTarget.All, monsterStatus.nowHp);
        }

        PlayerBase attackPlayer_ = message.causer.GetComponent<PlayerBase>();
        attackPlayer_.GetExp(damageAmount_ / 5, PlayerStat.PlayerExpType.WEAPON);
        
        if (!isDie)
        {
            DieCheck(attackPlayer_);
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
        if (PhotonNetwork.IsMasterClient)
        {
            monsterStatus.nowHp -= message.damageAmount;
            monsterHpBar.fillAmount = monsterStatus.nowHp / monsterStatus.maxHp;
            photonView.RPC("SetMonsterStat", RpcTarget.All, monsterStatus.nowHp);
        }

        PlayerBase attackPlayer_ = message.causer.GetComponent<PlayerBase>();
        attackPlayer_.GetExp(message.damageAmount / 20, PlayerStat.PlayerExpType.WEAPON);

        if (!isDie)
        {
            DieCheck(attackPlayer_);
        }
    }
    public void TakeSolidDamage(DamageMessage message, float damageAmount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            monsterStatus.nowHp -= message.damageAmount;
            monsterHpBar.fillAmount = monsterStatus.nowHp / monsterStatus.maxHp;
            photonView.RPC("SetMonsterStat", RpcTarget.All, monsterStatus.nowHp);
        }

        PlayerBase attackPlayer_ = message.causer.GetComponent<PlayerBase>();
        attackPlayer_.GetExp(damageAmount / 20, PlayerStat.PlayerExpType.WEAPON);

        if (!isDie)
        {
            DieCheck(attackPlayer_);
        }
    }

    /// <summary>
    /// debuffIndex의 순서
    /// 0 = 출혈, 1 = 독, 2 = 스턴, 3 = 속박
    /// </summary>
    /// <param name="message"></param>
    /// <param name="debuffIndex_"></param>
    /// <returns></returns>
    /// 서로 다른 플레이어라면 각자 출혈을 걸도록 고쳐야할 필요가 있음
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
                if(delayTime_ > tickTime_)
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

    [PunRPC]
    public void Debuff(int debuffIndex_, float continousTime_)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("DebuffSet", RpcTarget.All, debuffIndex_, continousTime_);
        }
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
                    monsterController.isMoveAble = false;
                    monsterController.enabled = false;
                    break;
                // 속박
                case 3:
                    monsterController.isMoveAble = false;
                    break;
                // 에어본
                case 4:
                    monsterController.isMoveAble = false;
                    monsterController.navMeshAgent.enabled = false;
                    monsterController.enabled = false;
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, 6, 0), ForceMode.Impulse);
                    break;
            }

            while (debuffRemainTime[debuffIndex_] > 0)
            {
                // 프레임마다 지속시간 감소
                debuffRemainTime[debuffIndex_] -= Time.deltaTime;

                // 상태이상 종류 체크
                if (debuffIndex_ == 2 || debuffIndex_ == 3) monsterController.isMoveAble = false;
                yield return null;
            }

            // 디버프 종류
            switch (debuffIndex_)
            {
                // 스턴
                case 2:
                    monsterController.isMoveAble = true;
                    monsterController.enabled = true;
                    break;
                // 속박
                case 3:
                    monsterController.isMoveAble = true;
                    break;
                // 에어본
                case 4:
                    monsterController.isMoveAble = true;
                    monsterController.navMeshAgent.enabled = true;
                    monsterController.enabled = true;
                    break;
            }

            // 지속 종료시 리셋
            debuffRemainTime[debuffIndex_] = 0;
            applyDebuffCheck[debuffIndex_] = false;
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
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if (!outline.isClick)
        {
            outline.enabled = false;
        }
    }
    [PunRPC]
    public void SetMonsterStat(int level_, float hp_)
    {
        monsterStatus.level = level_;
        monsterStatus.nowHp = hp_;
    }
    [PunRPC]
    public void SetMonsterStat(float hp_)
    {
        monsterStatus.nowHp = hp_;
        monsterHpBar.fillAmount = monsterStatus.nowHp / monsterStatus.maxHp;
    }
}

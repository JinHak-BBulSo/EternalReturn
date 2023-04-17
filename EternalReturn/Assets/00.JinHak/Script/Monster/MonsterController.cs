using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        NONE = -1,
        IDLE,
        MOVE,
        BEWARE,
        ATTACk,
        SKILL,
        RECALL,
        DIE
    }

    public Action skillActive = default;

    private Dictionary<MonsterState, IMonsterState> monsterStateDic = new Dictionary<MonsterState, IMonsterState>();
    private MonsterStateMachine monsterStateMachine = default;
    public MonsterState monsterState = MonsterState.NONE;
    public MonsterStateMachine MonsterStateMachine { get; private set; }

    private bool isBattleStart = false;
    private bool isDie = false;

    public Monster monster = default;
    public Rigidbody monsterRigid = default;
    public Animator monsterAni = default;
    public AudioSource monsterAudio = default;
    public NavMeshAgent navMeshAgent = default;
    public PlayerController targetPlayer = default;

    protected bool isInSkillUse = false;
    private int encountPlayerCount = 0;

    void Start()
    {
        // Component Set
        monsterRigid = GetComponent<Rigidbody>();
        monsterAni = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 100f;
        navMeshAgent.angularSpeed = 180f;
        // navMeshAgent.speed = monster.moveSpeed;
        navMeshAgent.enabled = true;

        IMonsterState idle = new MonsterIdle();
        IMonsterState move = new MonsterMove();
        IMonsterState beware = new MonsterBeware();
        IMonsterState attack = new MonsterAttack();
        IMonsterState skill = new MonsterSkill();
        IMonsterState recall = new MonsterRecall();
        IMonsterState die = new MonsterDie();

        monsterStateDic.Add(MonsterState.IDLE, idle);
        monsterStateDic.Add(MonsterState.MOVE, move);
        monsterStateDic.Add(MonsterState.BEWARE, beware);
        monsterStateDic.Add(MonsterState.ATTACk, attack);
        monsterStateDic.Add(MonsterState.SKILL, skill);
        monsterStateDic.Add(MonsterState.RECALL, recall);
        monsterStateDic.Add(MonsterState.DIE, die);

        monsterStateMachine = new MonsterStateMachine(idle, this);
    }

    void Update()
    {
        if(!isDie && !isBattleStart)
        {
            return;
        }
        else if(!isDie && isBattleStart)
        {
            UpdateState();
        }
    }

    private void FixedUpdate()
    {
        monsterStateMachine.DoFixedUpdate();
    }

    public void CallCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void CallStopCoroutine(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }

    private void UpdateState()
    {
        if(targetPlayer == null && encountPlayerCount == 0)
        {
            return;
        }
        else
        {
            if(encountPlayerCount != 0 && targetPlayer == null)
            {
                monsterStateMachine.SetState(monsterStateDic[MonsterState.BEWARE]);
            }
            else if(encountPlayerCount != 0 && targetPlayer != null)
            {

            }
        }
    }

    protected virtual void UseSkill()
    {
        isInSkillUse = true;
        skillActive();
    }
    protected virtual void EndSkill()
    {
        isInSkillUse = false;
    }
}

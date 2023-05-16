using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
public class MonsterController : MonoBehaviourPun
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
        DELAY,
        DIE
    }

    //public Action skillActive = default;

    private Dictionary<MonsterState, IMonsterState> monsterStateDic = new Dictionary<MonsterState, IMonsterState>();
    public Dictionary<MonsterState, IMonsterState> MonsterStateDic {  get { return monsterStateDic; } }
    public MonsterState monsterState = MonsterState.NONE;

    private MonsterStateMachine monsterStateMachine;
    public MonsterStateMachine MonsterStateMachine { get { return monsterStateMachine; } }

    public Monster monster = default;
    public Rigidbody monsterRigid = default;
    public Animator monsterAni = default;
    public AudioSource monsterAudio = default;
    public NavMeshAgent navMeshAgent = default;
    public PlayerBase targetPlayer = default;

    public bool isMoveAble = true;
    public bool isSkillAble = false;
    public bool isInSkillUse = false;
    public int encountPlayerCount = 0;
    public bool isAttack = false;

    public bool actionDelay = false;
    public void AwakeOrder()
    {
        // Component Set
        monsterRigid = GetComponent<Rigidbody>();
        monsterAni = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        monster = GetComponent<Monster>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 100;
        navMeshAgent.angularSpeed = 1800f;
        navMeshAgent.speed = monster.monsterStatus.moveSpeed;
        navMeshAgent.enabled = true;

        IMonsterState idle = new MonsterIdle();
        IMonsterState move = new MonsterMove();
        IMonsterState beware = new MonsterBeware();
        IMonsterState attack = new MonsterAttack();
        IMonsterState skill = new MonsterSkill();
        IMonsterState recall = new MonsterRecall();
        IMonsterState delay = new MonsterDelay();
        IMonsterState die = new MonsterDie();

        monsterStateDic.Add(MonsterState.IDLE, idle);
        monsterStateDic.Add(MonsterState.MOVE, move);
        monsterStateDic.Add(MonsterState.BEWARE, beware);
        monsterStateDic.Add(MonsterState.ATTACk, attack);
        monsterStateDic.Add(MonsterState.SKILL, skill);
        monsterStateDic.Add(MonsterState.RECALL, recall);
        monsterStateDic.Add(MonsterState.DELAY, delay);
        monsterStateDic.Add(MonsterState.DIE, die);

        monsterStateMachine = new MonsterStateMachine(idle, this);
    }

    public void SetStateMachine(MonsterStateMachine machine_)
    {
        monsterStateMachine = machine_;
    }
    void Update()
    {
        if(monsterStateMachine == null)
        {
            return;
        }
        UpdateState();
        monsterStateMachine.DoUpdate();
    }

    private void FixedUpdate()
    {
        if (monsterStateMachine != default)
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

    // Animation Call
    public void Delay()
    {
        actionDelay = true;
        monsterStateMachine.SetState(monsterStateDic[MonsterState.DELAY]);
    }

    public void UpdateState()
    {
        // 공통 State 전이
        if (monster.isDie)
        {
            monsterStateMachine.SetState(monsterStateDic[MonsterState.DIE]);
            return;
        }

        if (monster.isBattleAreaOut)
        {
            monsterStateMachine.SetState(monsterStateDic[MonsterState.RECALL]);
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMonsterState
{
    private MonsterController monsterController = default;

    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.MOVE;
        monsterController.CallCoroutine(Move());
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        monsterController.navMeshAgent.SetDestination(monsterController.targetPlayer.transform.position);
    }
    public void StateExit()
    {

    }

    private IEnumerator Move()
    {
        monsterController.monsterAni.SetBool("isMove", true);
        while (true)
        {
            if (monsterController.monster.monsterStatus.attackRange >
                Vector3.Distance(monsterController.transform.position, monsterController.targetPlayer.transform.position))
            {
                monsterController.monsterAni.SetBool("isMove", false);
                monsterController.monsterState = MonsterController.MonsterState.IDLE;
                yield break;
            }
            yield return null;
        }
    }
}

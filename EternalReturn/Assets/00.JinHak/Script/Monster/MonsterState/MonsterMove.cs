using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMonsterState
{
    private MonsterController monsterController = default;
    private bool isMoveEnd = false;

    public void StateEnter(MonsterController monsterCtrl_)
    {
        isMoveEnd = false;
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
        isMoveEnd = true;
    }

    private IEnumerator Move()
    {
        monsterController.monsterAni.SetBool("isMove", true);
        while (true)
        {
            if (monsterController.monster.isDie)
            {
                yield break;
            }
            if (monsterController.monster.monsterStatus.attackRange >
                Vector3.Distance(monsterController.transform.position, monsterController.targetPlayer.transform.position)
                || isMoveEnd)
            {
                monsterController.monsterAni.SetBool("isMove", false);
                yield break;
            }
            yield return null;
        }
    }
}

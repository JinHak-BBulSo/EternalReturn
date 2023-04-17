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
                yield break;
            }
            monsterController.transform.LookAt(monsterController.targetPlayer.transform);
            monsterController.transform.Translate(monsterController.transform.forward * Time.deltaTime);
            yield return null;
        }
    }
}

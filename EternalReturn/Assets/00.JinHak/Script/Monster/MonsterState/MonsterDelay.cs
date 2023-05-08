using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDelay : IMonsterState
{
    private MonsterController monsterController = default;

    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.DELAY;
        monsterController.CallCoroutine(Delay());
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
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.1f);
        monsterController.monsterRigid.velocity = Vector3.zero;
        monsterController.navMeshAgent.enabled = true;
        monsterController.monsterAni.SetBool("isAttack", false);
        monsterController.monsterAni.SetBool("isSkill", false);
        monsterController.isInSkillUse = false;
        monsterController.actionDelay = false;
    }
}

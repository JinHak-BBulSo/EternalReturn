using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterIdle : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.IDLE;
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        if (monsterController.targetPlayer == null && monsterController.encountPlayerCount == 0)
        {
            monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.IDLE]);
        }
        else
        {
            if (monsterController.encountPlayerCount != 0 && monsterController.targetPlayer == null)
            {
                monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.BEWARE]);
            }
            else if (monsterController.targetPlayer != null)
            {
                if (Vector3.Distance(monsterController.targetPlayer.transform.position, monsterController.transform.position) > monsterController.monster.monsterStatus.attackRange)
                {
                    monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.MOVE]);
                }
                else
                {
                    if (monsterController.isSkillAble && !monsterController.isInSkillUse && !monsterController.isAttack)
                    {
                        monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.SKILL]);
                        monsterController.isInSkillUse = true;
                        monsterController.isSkillAble = false;

                        return;
                    }
                    else
                    {
                        monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.ATTACk]);
                    }
                }
            }
        }
    }
    public void StateExit()
    {

    }

    public virtual void Idle()
    {

    }
}

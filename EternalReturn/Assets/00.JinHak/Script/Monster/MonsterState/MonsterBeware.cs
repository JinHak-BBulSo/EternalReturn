using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBeware : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.BEWARE;
        monsterController.monster.audioSource.clip = monsterController.monster.sounds[3];
        Beware();
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        if (monsterController.targetPlayer == default || monsterController.targetPlayer == null) return;

        if (monsterController.targetPlayer != null)
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
                }
                else
                {
                    monsterController.MonsterStateMachine.SetState(monsterController.MonsterStateDic[MonsterController.MonsterState.ATTACk]);
                }
            }
        }
    }
    public void StateExit()
    {
        ExitBeware();
    }
    public void Beware()
    {
        monsterController.monsterAni.SetBool("isBeware", true);
    }
    public virtual void ExitBeware()
    {
        monsterController.monsterAni.SetBool("isBeware", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static MonsterController;

public class MonsterAttack : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.ATTACk;
        Attack();
    }

    public void StateFixedUpdate()
    {
        
    }

    public void StateUpdate()
    {
        
    }
    public void StateExit()
    {
        ExitAttack();
    }
    public void Attack()
    {
        monsterController.monsterAni.SetBool("isAttack", true);
    }
    public void ExitAttack()
    {
        GameObject target_ = monsterController.gameObject;
        float damageAmount_ = monsterController.monster.monsterStatus.attackPower;
        DamageMessage dm = new DamageMessage(target_, damageAmount_);

        monsterController.targetPlayer.TakeDamage(dm);
        monsterController.monsterAni.SetBool("isAttack", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterAttack : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.ATTACk;
        Attack();
        monsterController.monster.audioSource.clip = monsterController.monster.sounds[0];
        monsterController.navMeshAgent.enabled = false;
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
        monsterController.navMeshAgent.enabled = false;
        monsterController.transform.LookAt(monsterController.monster.firstAttackPlayer.transform);
        monsterController.monsterAni.SetBool("isAttack", true);
    }
    public void ExitAttack()
    {
        GameObject target_ = monsterController.gameObject;
        float damageAmount_ = monsterController.monster.monsterStatus.attackPower;
        DamageMessage dm = new DamageMessage(target_, damageAmount_);

        monsterController.navMeshAgent.enabled = true;
        monsterController.targetPlayer.TakeDamage(dm);
        monsterController.navMeshAgent.enabled = true;
    }
}

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
        monsterController.isAttack = true;
        Attack();
        monsterController.monster.audioSource.clip = monsterController.monster.sounds[0];
        monsterController.navMeshAgent.enabled = false;
    }

    public void StateFixedUpdate()
    {
        
    }

    public void StateUpdate()
    {
        if(monsterController.targetPlayer.PlayerController.playerState == PlayerController.PlayerState.DIE)
        {
            monsterController.monster.isBattleAreaOut = true;
            monsterController.targetPlayer = default;
            monsterController.monster.firstAttackPlayer = default;
        }
    }
    public void StateExit()
    {
        ExitAttack();
    }
    public void Attack()
    {
        if (monsterController.targetPlayer != default)
        {
            monsterController.navMeshAgent.enabled = false;
            monsterController.transform.LookAt(monsterController.targetPlayer.transform);
            monsterController.monsterAni.SetBool("isAttack", true);
        }
    }
    public void ExitAttack()
    {
        GameObject target_ = monsterController.gameObject;
        float damageAmount_ = monsterController.monster.monsterStatus.attackPower;
        DamageMessage dm = new DamageMessage(target_, damageAmount_);
        monsterController.isAttack = false;

        monsterController.navMeshAgent.enabled = true;
        if (monsterController.targetPlayer != default)
        {
            monsterController.targetPlayer.TakeDamage(dm);
        }
    }
}

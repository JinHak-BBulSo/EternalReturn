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
        monsterController.monster.Attack();
    }

    public void StateFixedUpdate()
    {
        
    }

    public void StateUpdate()
    {
        
    }
    public void StateExit()
    {
        monsterController.monster.ExitAttack();
    }
}

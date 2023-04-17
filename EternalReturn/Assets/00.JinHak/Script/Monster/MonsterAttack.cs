using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : IMonsterState
{
    private MonsterController monsterController;

    void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterCtrl_.monsterState = MonsterController.MonsterState.ATTACk;
        monsterCtrl_.monster.Attack();
    }
    void StateFixedUpdate()
    {

    }
    void StateUpdate()
    {

    }
    void StateExit()
    {
        
    }
}

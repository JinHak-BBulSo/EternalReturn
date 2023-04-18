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
        monsterController.monster.Beware();
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {

    }
    public void StateExit()
    {
        monsterController.monster.ExitBeware();
    }
}

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


        yield return null;
    }
}

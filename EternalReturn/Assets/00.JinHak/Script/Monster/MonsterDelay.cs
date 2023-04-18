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
        monsterController.actionDelay = false;
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.4f);
        monsterController.monsterState = MonsterController.MonsterState.IDLE;
    }
}

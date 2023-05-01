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
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.3f);
        monsterController.actionDelay = false;
    }
}

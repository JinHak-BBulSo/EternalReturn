using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterStateMachine
{
    private MonsterController monsterController = default;

    public delegate void MonsterStateHandler(IMonsterState state_);
    public Action<IMonsterState> monsterStateChange;

    public IMonsterState nowState;

    public MonsterStateMachine(IMonsterState idleState_, MonsterController monsterController_)
    {
        monsterStateChange += SetState;
        monsterController = monsterController_;
        nowState = idleState_;
        nowState.StateEnter(monsterController);

        monsterController.SetStateMachine(this);
    }
    
    public void SetState(IMonsterState state_)
    {
        if(nowState == state_)
        {
            return;
        }
        nowState.StateExit();
        nowState = state_;
        nowState.StateEnter(monsterController);
    }

    public void DoFixedUpdate()
    {
        nowState.StateFixedUpdate();
    }

    public void DoUpdate()
    {
        nowState.StateUpdate();
    }
}

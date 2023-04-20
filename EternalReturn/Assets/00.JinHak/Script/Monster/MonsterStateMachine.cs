using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    private MonsterController monsterController = default;

    public delegate void MonsterStateHandler(IMonsterState state_);
    public Action<IMonsterState> monsterStateChange;

    public IMonsterState nowState
    {
        get;
        private set;
    }

    public MonsterStateMachine(IMonsterState idleState_, MonsterController monsterController_)
    {
        monsterStateChange += SetState;
        nowState = idleState_;
        monsterController = monsterController_;
        SetState(nowState);
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

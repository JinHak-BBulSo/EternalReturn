using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    public void StateEnter(MonsterController monsterCtrl_);
    public void StateFixedUpdate();
    public void StateUpdate();
    public void StateExit();
}

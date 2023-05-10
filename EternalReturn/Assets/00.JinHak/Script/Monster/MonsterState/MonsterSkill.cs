using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.SKILL;
        monsterController.monster.Skill();
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {

    }
    public void StateExit()
    {
        ExitSkill();
    }
    public virtual void ExitSkill()
    {
        
    }
}

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
        monsterController.monster.audioSource.clip = monsterController.monster.sounds[3];
        Beware();
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {

    }
    public void StateExit()
    {
        ExitBeware();
    }
    public void Beware()
    {
        monsterController.monsterAni.SetBool("isBeware", true);
    }
    public virtual void ExitBeware()
    {
        monsterController.monsterAni.SetBool("isBeware", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDie : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.DIE;
        Die();
        monsterController.CallCoroutine(EraseMonster());
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

    IEnumerator EraseMonster()
    {
        yield return new WaitForSeconds(60);
        monsterController.gameObject.SetActive(false);
    }

    public void Die()
    {
        monsterController.monster.monsterItemBox.enabled = true;
        monsterController.monsterAni.SetTrigger("Die");
        monsterController.navMeshAgent.enabled = false;
    }
}

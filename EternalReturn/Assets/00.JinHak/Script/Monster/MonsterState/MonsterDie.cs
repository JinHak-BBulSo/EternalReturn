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
        monsterController.monster.audioSource.clip = monsterController.monster.sounds[2];
        Die();
        monsterController.CallCoroutine(EraseMonster());
        monsterController.monster.SpawnPoint.RespawnMonster();
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        monsterController.monsterRigid.velocity = Vector3.zero;
        monsterController.monster.monsterStatusUi.SetActive(false);
    }
    public void StateExit()
    {

    }

    IEnumerator EraseMonster()
    {
        yield return new WaitForSeconds(60);
        monsterController.gameObject.SetActive(false);
        monsterController.monsterAni.SetBool("isDie", false);
    }

    public void Die()
    {
        monsterController.monster.isDie = true;
        monsterController.monster.monsterItemBox.enabled = true;
        monsterController.monsterAni.SetBool("isDie", true);
        monsterController.navMeshAgent.enabled = false;
        monsterController.targetPlayer = default;
        monsterController.monsterRigid.velocity = Vector3.zero;
        monsterController.monster.monsterStatusUi.SetActive(false);
    }
}

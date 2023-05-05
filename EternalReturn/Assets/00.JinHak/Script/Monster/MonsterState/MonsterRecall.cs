using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRecall : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.RECALL;
        monsterController.CallCoroutine(Recall());
    }
    public void StateFixedUpdate()
    {

    }
    public void StateUpdate()
    {
        monsterController.navMeshAgent.SetDestination(monsterController.monster.MonsterBattleArea.transform.position);
    }
    public void StateExit()
    {

    }

    IEnumerator Recall()
    {
        yield return null;
        RecallStart();
        monsterController.navMeshAgent.SetDestination(monsterController.monster.SpawnPoint.transform.position);
        float healDelay_ = 0f;

        while (true)
        {
            Vector3 monsterPos_ = monsterController.transform.localPosition;
            Vector3 recallPos_ = monsterController.monster.MonsterBattleArea.transform.localPosition;
            healDelay_ += Time.deltaTime;

            if(healDelay_ > 0.25f)
            {
                monsterController.monster.monsterStatus.nowHp +=
                    (int)(monsterController.monster.monsterStatus.maxHp * 0.1f);
                healDelay_ = 0;
                if(monsterController.monster.monsterStatus.nowHp > monsterController.monster.monsterStatus.maxHp)
                {
                    monsterController.monster.monsterStatus.nowHp = monsterController.monster.monsterStatus.maxHp;
                }

                monsterController.monster.monsterHpBar.fillAmount =
                    monsterController.monster.monsterStatus.nowHp / monsterController.monster.monsterStatus.maxHp;
                monsterController.monster.CallSetMonsterStat();
            }

            if (ExceptY.ExceptYDistance(monsterPos_, recallPos_) < 0.01f)
            {
                ExitRecall();
                yield break;
            }
            yield return null;
        }
    }

    public void RecallStart()
    {
        monsterController.monster.isBattleAreaOut = true;
        monsterController.monsterAni.SetBool("isMove", true);
    }
    public virtual void ExitRecall()
    {
        monsterController.monster.firstAttackPlayer = default;
        monsterController.targetPlayer = default;
        monsterController.monster.isBattleAreaOut = false;
        monsterController.monsterAni.SetBool("isMove", false);
    }
}

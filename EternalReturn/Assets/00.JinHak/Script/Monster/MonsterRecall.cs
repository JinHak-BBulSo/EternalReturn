using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRecall : IMonsterState
{
    private MonsterController monsterController = default;
    public void StateEnter(MonsterController monsterCtrl_)
    {
        this.monsterController = monsterCtrl_;
        monsterController.monsterState = MonsterController.MonsterState.MOVE;
        monsterController.CallCoroutine(Recall());
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

    IEnumerator Recall()
    {
        while (true)
        {
            monsterController.monsterAni.SetBool("isMove", true);
            if (Vector3.Distance(monsterController.transform.position, monsterController.monster.monsterBattleArea.transform.position) < 0.1f)
            {
                monsterController.monster.ExitRecall();
                yield break;
            }

            monsterController.transform.LookAt(monsterController.monster.monsterBattleArea.transform);
            monsterController.transform.Translate(monsterController.transform.forward * Time.deltaTime);
            yield return null;
        }
    }
}

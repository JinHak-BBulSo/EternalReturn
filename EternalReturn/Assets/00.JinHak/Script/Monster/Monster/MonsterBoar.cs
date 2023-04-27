using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoar : Monster
{
    [SerializeField]
    GameObject skillRange = default;
    private float skillCoolTime = 15f;

    public override void Skill()
    {
        base.Skill();
        StartCoroutine(SkillReady());
    }
    private void Update()
    {
        Vector3 len = Camera.main.ScreenToWorldPoint(ExceptY.ExceptYPos(Input.mousePosition) - ExceptY.ExceptYPos(transform.position));
        float y = Mathf.Atan2(len.z, len.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }
    IEnumerator SkillReady()
    {
        StartCoroutine(SkillCoolTime(skillCoolTime));
        skillRange.SetActive(true);
        monsterController.monsterAni.SetBool("isSkillReady", true);
        monsterController.navMeshAgent.enabled = false;
        yield return new WaitForSeconds(2f);
        SkillAssult();
    }

    void SkillAssult()
    {
        monsterController.monsterAni.SetBool("isSkillReady", false);
        skillRange.SetActive(false);
        monsterController.navMeshAgent.enabled = true;
    }

    IEnumerator SkillCoolTime(float coolTime_)
    {
        yield return new WaitForSeconds(coolTime_);
        monsterController.isSkillAble = true;
    }
}

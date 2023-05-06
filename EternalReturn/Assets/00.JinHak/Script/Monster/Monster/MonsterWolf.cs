using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWolf : Monster
{
    [SerializeField]
    GameObject skillRange = default;

    private float skillCoolTime = 40f;
    private bool isAssult = false;
    public WolfSkill wolfSkillMesh = default;

    protected override void SetStatus()
    {
        base.SetStatus();
        monsterController.isSkillAble = true;
    }

    public override void Skill()
    {
        base.Skill();
        int r_ = Random.Range(4, 5 + 1);
        audioSource.clip = sounds[r_];
        monsterController.monsterAni.SetBool("isSkill", true);
        skillRange.SetActive(true);
        monsterController.navMeshAgent.enabled = false;
    }

    private void SkillAassult()
    {
        foreach (var wolf in wolfSkillMesh.targetWolfs)
        {
            wolf.monsterController.targetPlayer = monsterController.targetPlayer;
            wolf.HowlingCall();
        }
        skillRange.SetActive(false);
        StartCoroutine(SkillCoolTime(skillCoolTime));
        monsterController.isInSkillUse = false;
        monsterController.navMeshAgent.enabled = true;;
        monsterController.monsterAni.SetBool("isSkill", false);
    }

    IEnumerator SkillCoolTime(float coolTime_)
    {
        yield return new WaitForSeconds(coolTime_);
        monsterController.isSkillAble = true;
    }

    public void HowlingCall()
    {
        monsterController.isSkillAble = false;
        StartCoroutine(SkillCoolTime(skillCoolTime));
    }
}

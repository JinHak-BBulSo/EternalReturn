using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBear : Monster
{
    [SerializeField]
    GameObject skillRange = default;

    private float skillCoolTime = 20f;
    private bool isAssult = false;
    public BearSkill bearSkillMesh = default;

    protected override void SetStatus()
    {
        base.SetStatus();
        monsterController.isSkillAble = true;
    }

    public override void Skill()
    {
        base.Skill();
        monsterController.monsterAni.SetBool("isSkill", true);
        skillRange.SetActive(true);
    }

    private void SkillAassult()
    {
        foreach (var player in bearSkillMesh.targetPlayers)
        {
            DamageMessage dm = new DamageMessage(this.gameObject, 100, 3, 1.5f);
            player.TakeDamage(dm);
            player.Debuff(2, 1.5f);
        }
        skillRange.SetActive(false);
        StartCoroutine(SkillCoolTime(skillCoolTime));
        monsterController.isInSkillUse = false;
        monsterController.monsterAni.SetBool("isSkill", false);
    }

    IEnumerator SkillCoolTime(float coolTime_)
    {
        yield return new WaitForSeconds(coolTime_);
        monsterController.isSkillAble = true;
    }
}

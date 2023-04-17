using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{
    public override void Skill_Q()
    {
        base.Skill_Q();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 0);
        Skill_Q_Range.SetActive(true);
        StartCoroutine(SkillCooltime(skill_Q_Cooltime, 9f));
    }



    IEnumerator SkillCooltime(bool skill_, float cooltime_)
    {
        skill_ = true;
        yield return new WaitForSeconds(cooltime_);
        skill_ = false;
    }

}

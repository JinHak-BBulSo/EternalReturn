using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aya : PlayerBase
{

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        if (enemy.GetComponent<Monster>() != null)
        {
            DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
            enemy.GetComponent<Monster>().TakeDamage(dm);
        }
        else if (enemy.GetComponent<PlayerBase>() != null)
        {
            DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
            enemy.GetComponent<PlayerBase>().TakeDamage(dm);
        }
    }



    public override void Skill_Q()
    {
        base.Skill_Q();
        // if (clickTarget)
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 0);
        StartCoroutine(SkillCooltime(0, 9f));
    }


    public override void Skill_W()
    {
        base.Skill_W();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 1);
        StartCoroutine(SkillCooltime(1, 19f));
    }

    public override void Skill_E()
    {
        base.Skill_E();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 2);
        StartCoroutine(SkillCooltime(2, 20f));
    }

    public override void Skill_R()
    {
        base.Skill_R();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 3);
    }

    public override void Skill_D()
    {
        base.Skill_D();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 4);


    }

    IEnumerator SkillCooltime(int skillType_, float cooltime_)
    {
        skillCooltimes[skillType_] = true;
        yield return new WaitForSeconds(cooltime_);
        skillCooltimes[skillType_] = false;
    }
}

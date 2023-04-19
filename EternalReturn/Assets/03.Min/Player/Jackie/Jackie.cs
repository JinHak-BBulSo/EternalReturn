using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{
    public List<PlayerBase> enemyPlayer = new List<PlayerBase>();
    public List<Monster> enemyHunt = new List<Monster>();
    [SerializeField]
    private BoxCollider QBoxCol = default;
    protected override void Start()
    {
        base.Start();
        QBoxCol = Skill_Q_Range.transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
    }
    public override void Skill_Q()
    {
        base.Skill_Q();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 0);
        Skill_Q_Range.SetActive(true);
        StartCoroutine(SkillCooltime(0, 9f));
    }

    // (9f - ((skillLevel - 1) * 0.5) *  playerStat.coolDown)
    IEnumerator SkillCooltime(int skillType_, float cooltime_)
    {
        skillCooltimes[skillType_] = true;
        yield return new WaitForSeconds(cooltime_);
        skillCooltimes[skillType_] = false;
    }

    private void TriggerOn()
    {
        QBoxCol.isTrigger = true;
    }

    private void TriggerExit(int index_)
    {
        switch (index_)
        {
            case 1:
                FirstQDamage();
                break;
            case 2:
                SecondQDamage();
                break;
        }
        QBoxCol.isTrigger = false;
    }
    private void FirstQDamage()
    {
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            Debug.Log(25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            enemyPlayer[i].TakeDamage(dm);
            DamageMessage defdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0);
            enemyPlayer[i].ContinousDamage(defdm, 0, 6f);
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            Debug.Log(enemyHunt[i] + "나 맞음");
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            Debug.Log(25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            enemyHunt[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0);
            enemyHunt[i].ContinousDamage(debuffdm, 0, 6f);
        }
    }

    private void SecondQDamage()
    {
        Debug.Log(enemyHunt.Count);
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.7f) + (playerStat.skillPower * 0.6f));
            enemyPlayer[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyHunt.Count; i++)
        {
            Debug.Log(enemyHunt[i] + "나 맞음22");
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.7f) + (playerStat.skillPower * 0.6f));
            Debug.Log(25 + (playerStat.attackPower * 0.7f) + (playerStat.skillPower * 0.6f));
            enemyHunt[i].TakeDamage(dm);
        }
        enemyPlayer.Clear();
        enemyHunt.Clear();
    }
}

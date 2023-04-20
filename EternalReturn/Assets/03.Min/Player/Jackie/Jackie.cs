using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{
    public List<PlayerBase> enemyPlayer = new List<PlayerBase>();
    public List<Monster> enemyHunt = new List<Monster>();
    private BoxCollider QBoxCol = default;
    [SerializeField]
    private bool isWBuffOn = false;
    [SerializeField]
    private bool isRBuffOn = false;
    protected override void Start()
    {
        base.Start();
        QBoxCol = Skill_Q_Range.transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
        basicAttackCol = attackRange.GetComponent<SphereCollider>();
    }

    public override void Attack()
    {
        base.Attack();
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        if (isRBuffOn)
        {
            if (isWBuffOn)
            {
            }
            else
            {

            }
        }
        else
        {
            if (isWBuffOn)
            {

            }
            else
            {
                Debug.Log(enemy.name);
                if (enemy.GetComponent<Monster>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerStat.attackPower);
                    enemy.GetComponent<Monster>().TakeDamage(dm);
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerStat.attackPower);
                    enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                }
            }
        }
        enemy = null;
    }
    public override void Skill_Q()
    {
        base.Skill_Q();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 0);
        Skill_Q_Range.SetActive(true);
        StartCoroutine(SkillCooltime(0, 9f));
    }


    public override void Skill_W()
    {
        base.Skill_W();
        isWBuffOn = true;
        StartCoroutine(SkillCooltime(1, 19f));
        StartCoroutine(buffCool());
    }
    IEnumerator buffCool()
    {
        yield return new WaitForSeconds(5f);
        isWBuffOn = false;
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
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyPlayer[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            Debug.Log(25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            enemyHunt[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0, 6f);
            Debug.Log((30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f);
            StartCoroutine(enemyHunt[i].ContinousDamage(debuffdm, 0, 6f, 1f));
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
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.7f) + (playerStat.skillPower * 0.6f));
            Debug.Log(25 + (playerStat.attackPower * 0.7f) + (playerStat.skillPower * 0.6f));
            enemyHunt[i].TakeDamage(dm);
        }
        enemyPlayer.Clear();
        enemyHunt.Clear();
    }
}

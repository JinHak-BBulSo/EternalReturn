using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{
    public List<PlayerBase> enemyPlayer = new List<PlayerBase>();
    public List<Monster> enemyHunt = new List<Monster>();
    public BoxCollider QBoxCol = default;
    [SerializeField]
    private bool isWBuffOn = false;
    [SerializeField]
    private bool isRBuffOn = false;
    Vector3 targetPos;
    Vector3 exceptYTragetPos;
    Vector3 nowPos;
    Vector3 dir;
    float distance;
    protected override void Start()
    {
        base.Start();
        QBoxCol = SkillRange[0].transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
        // Debug.Log(QBoxCol);
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
        SkillRange[0].SetActive(true);
        StartCoroutine(SkillCooltime(0, 9f));
    }


    public override void Skill_W()
    {
        base.Skill_W();
        isWBuffOn = true;
        StartCoroutine(SkillCooltime(1, 19f));
        StartCoroutine(buffCool());
    }

    public override void Skill_E()
    {
        base.Skill_E();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 2);
        targetPos = nowMousePoint;
        exceptYTragetPos = ExceptY.ExceptYPos(nowMousePoint);
        nowPos = ExceptY.ExceptYPos(transform.position);
        distance = ExceptY.ExceptYDistance(transform.position, targetPos);
        Debug.Log(distance);
        if (distance >= 5.5f)
        {
            dir = Vector3.Normalize(exceptYTragetPos - nowPos);
            targetPos = nowPos + dir * 5.5f;
        }
        StartCoroutine(JackieJump());
        StartCoroutine(SkillCooltime(2, 1f));
    }

    IEnumerator JackieJump()
    {
        while (true)
        {
            transform.Translate(dir * Time.deltaTime * 10f);
            Debug.Log("?");
            if (ExceptY.ExceptYDistance(transform.position, targetPos) <= 0.1f || ExceptY.ExceptYDistance(transform.position, targetPos) >= 0.1f)
            {
                break;
            }
            yield return null;
        }
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

    private void EDamage()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 1.0f, Vector3.up, 0f);
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<Monster>() != null)
            {
                enemyHunt.Add(hit.transform.gameObject.GetComponent<Monster>());
            }
            else if (hit.transform.gameObject.GetComponent<PlayerBase>() != null)
            {
                enemyPlayer.Add(hit.transform.gameObject.GetComponent<PlayerBase>());
            }
        }

        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 50 + (playerStat.attackPower * 0.42f) + (playerStat.skillPower * 0.60f));
            enemyPlayer[i].TakeDamage(dm);
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 50 + (playerStat.attackPower * 0.42f) + (playerStat.skillPower * 0.60f));
            enemyHunt[i].TakeDamage(dm);
        }
    }
    private void FirstQDamage()
    {
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            enemyPlayer[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyPlayer[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerStat.attackPower * 0.45f) + (playerStat.skillPower * 0.40f));
            enemyHunt[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerStat.attackPower * 0.5f) + (playerStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyHunt[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }
    }

    private void SecondQDamage()
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{

    public BoxCollider QBoxCol = default;
    [SerializeField]
    private bool isWBuffOn = false;
    [SerializeField]
    private bool isRBuffOn = false;
    private Vector3 targetPos;
    private Vector3 exceptYTragetPos;
    private Vector3 nowPos;
    private Vector3 dir;
    private float distance;
    private bool isWeaponPassiveOn = true;
    public int weaponStack = 0;

    protected override void Start()
    {
        base.Start();
        QBoxCol = SkillRange[0].transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>();
        skillCooltimes[4] = true;
        // Debug.Log(QBoxCol);
    }

    public override void Attack()
    {
        base.Attack();
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        if (isWeaponPassiveOn)
        {
            weaponStack++;
            if (weaponStack >= 4)
            {
                skillCooltimes[4] = false;
                weaponStack = 4;
            }
        }
        if (isRBuffOn)
        {
            if (isWBuffOn)
            {
                if (enemy.GetComponent<Monster>() != null)
                {
                    if (enemy.GetComponent<Monster>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * 0.09f);
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    if (enemy.GetComponent<PlayerBase>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * 0.09f);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<PlayerBase>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<PlayerBase>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                }
                playerStat.nowHp += 10f + playerTotalStat.attackPower * 0.08f + playerTotalStat.skillPower * 0.05f;
            }
            else
            {
                if (enemy.GetComponent<Monster>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                    enemy.GetComponent<Monster>().TakeDamage(dm);
                    DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                    StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                    enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                    StartCoroutine(enemy.GetComponent<PlayerBase>().ContinousDamage(debuffdm, 0, 6f, 1f));
                }
            }
        }
        else
        {
            if (isWBuffOn)
            {
                if (enemy.GetComponent<Monster>() != null)
                {
                    if (enemy.GetComponent<Monster>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * 0.09f);
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                    }
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    if (enemy.GetComponent<PlayerBase>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * 0.09f);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    }
                }
                playerStat.nowHp += 10f + playerTotalStat.attackPower * 0.08f + playerTotalStat.skillPower * 0.05f;
            }
            else
            {
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
        }
        enemy = null;
    }
    public override void Skill_Q()
    {
        base.Skill_Q();
        transform.LookAt(nowMousePoint);
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
        transform.LookAt(nowMousePoint);
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 2);
        targetPos = nowMousePoint;
        exceptYTragetPos = ExceptY.ExceptYPos(nowMousePoint);
        nowPos = ExceptY.ExceptYPos(transform.position);
        distance = ExceptY.ExceptYDistance(transform.position, targetPos);
        dir = Vector3.Normalize(exceptYTragetPos - nowPos);
        if (distance >= 5.5f)
        {
            distance = 5.5f;
            targetPos = transform.position + dir * distance;
        }
        isMove = false;
        StartCoroutine(JackieJump());
        StartCoroutine(SkillCooltime(2, 20f));
    }

    public override void Skill_R()
    {
        base.Skill_R();
        isRBuffOn = true;
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 3);
        weapon.SetActive(false);
    }

    public override void Skill_D()
    {
        base.Skill_D();
        if (weaponStack >= 4)
        {
            transform.LookAt(nowMousePoint);
            playerAni.SetBool("isSkill", true);
            playerAni.SetFloat("SkillType", 4);
            weaponStack = 0;
            skillCooltimes[4] = true;
        }



    }
    IEnumerator JackieJump()
    {
        float time = 0f;
        while (true)
        {
            transform.position += (dir * distance) * Time.deltaTime;
            if (time >= 0.7f)
            {
                yield break;
            }
            yield return null;
            time += Time.deltaTime;
        }
    }
    IEnumerator buffCool()
    {
        yield return new WaitForSeconds(5f);
        isWBuffOn = false;
    }

    IEnumerator RbuffCool()
    {
        playerController.ChangeState(new PlayerIdle());
        playerAni.SetBool("isRbuff", true);
        playerAni.SetBool("isSkill", false);
        yield return new WaitForSeconds(15f);
        if (isRBuffOn)
        {
            RBuffOff();
        }
    }


    // (9f - ((skillLevel - 1) * 0.5) *  playerStat.coolDown)
    IEnumerator SkillCooltime(int skillType_, float cooltime_)
    {
        skillCooltimes[skillType_] = true;
        yield return new WaitForSeconds(cooltime_);
        skillCooltimes[skillType_] = false;
    }

    private void RBuffOff()
    {
        isRBuffOn = false;
        playerAni.SetBool("isRbuff", false);
        playerAni.SetBool("isSkill", false);
        weapon.SetActive(true);
        playerController.ChangeState(new PlayerIdle());
        StartCoroutine(SkillCooltime(3, 70f));
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
        enemyPlayer.Clear();
        enemyHunt.Clear();
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
            DamageMessage dm = new DamageMessage(gameObject, 50 + (playerTotalStat.attackPower * 0.42f) + (playerTotalStat.skillPower * 0.60f));
            enemyPlayer[i].TakeDamage(dm);
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 50 + (playerTotalStat.attackPower * 0.42f) + (playerTotalStat.skillPower * 0.60f));
            enemyHunt[i].TakeDamage(dm);
        }
    }
    private void FirstQDamage()
    {
        enemyPlayer.Clear();
        enemyHunt.Clear();
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerTotalStat.attackPower * 0.45f) + (playerTotalStat.skillPower * 0.40f));
            enemyPlayer[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerTotalStat.attackPower * 0.5f) + (playerTotalStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyPlayer[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerTotalStat.attackPower * 0.45f) + (playerTotalStat.skillPower * 0.40f));
            enemyHunt[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (playerTotalStat.attackPower * 0.5f) + (playerTotalStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyHunt[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }
    }

    private void SecondQDamage()
    {
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerTotalStat.attackPower * 0.7f) + (playerTotalStat.skillPower * 0.6f));
            enemyPlayer[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (playerTotalStat.attackPower * 0.7f) + (playerTotalStat.skillPower * 0.6f));
            Debug.Log(25 + (playerTotalStat.attackPower * 0.7f) + (playerTotalStat.skillPower * 0.6f));
            enemyHunt[i].TakeDamage(dm);
        }
        enemyPlayer.Clear();
        enemyHunt.Clear();
    }

    IEnumerator WeaponSkillMove()
    {
        float time = 0f;
        while (true)
        {
            transform.position += transform.forward * 20f * Time.deltaTime;
            if (time >= 0.1f)
            {
                yield break;
            }
            yield return null;
            time += Time.deltaTime;
        }
    }
    private void DDamage()
    {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position, new Vector3(1f, 1f, 1f) * 0.5f, transform.forward, Quaternion.identity, 2f);
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 10f);

        Debug.Log($"endpoint : {transform.position + transform.forward.normalized * 2f}");
        StartCoroutine(WeaponSkillMove());
        // transform.position = transform.position + transform.forward.normalized * 2f;

        enemyPlayer.Clear();
        enemyHunt.Clear();
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.GetComponent<PlayerBase>() != null)
            {
                enemyPlayer.Add(hits[i].transform.GetComponent<PlayerBase>());
            }
            else if (hits[i].transform.GetComponent<Monster>() != null)
            {
                enemyHunt.Add(hits[i].transform.GetComponent<Monster>());
            }
        }

        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            float damage = playerTotalStat.attackPower * 1.2f + playerTotalStat.skillPower * 0.45f + enemyPlayer[i].playerTotalStat.maxHp * 0.07f;
            DamageMessage dm = new DamageMessage(gameObject, damage);
            enemyPlayer[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyHunt.Count; i++)
        {
            float damage = playerTotalStat.attackPower * 1.2f + playerTotalStat.skillPower * 0.45f + enemyHunt[i].monsterStatus.maxHp * 0.07f;
            DamageMessage dm = new DamageMessage(gameObject, damage);
            enemyHunt[i].TakeDamage(dm);
        }
    }
}
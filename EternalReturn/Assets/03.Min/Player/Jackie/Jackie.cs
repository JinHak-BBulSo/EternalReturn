using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class Jackie : PlayerBase
{
    public int weaponStack = 0;
    private bool isWBuffOn = false;
    private bool isRBuffOn = false;
    [SerializeField]
    private GameObject skillQ = default;
    [SerializeField]
    private GameObject skillE = default;
    private Vector3 targetPos;
    private Vector3 exceptYTragetPos;
    private Vector3 nowPos;
    private Vector3 dir;
    private float distance;
    private bool isWeaponPassiveOn = false;
    private bool hasBuff = false;
    private bool watchDebuff = false;
    private float increaseMoveSpeedBuff = 0f;
    private int maxStack = 4;
    private float increaseAttackPower = 0f;
    private bool isPassiveOn = false;
    private int prevPlayerKill = 0;
    private int prevHuntKill = 0;


    protected override void Start()
    {
        weaponType = 19;
        base.Start();
        skillCooltimes[4] = true;
        if (photonView.IsMine)
        {
            ItemManager.Instance.SetDefault(weaponType);
            ItemManager.Instance.GetItem(ItemManager.Instance.itemList[227]);
            item[0] = 277;
        }
        AddTotalStat();
    }
    public override void Attack()
    {
        base.Attack();
    }

    protected override void InitStat()
    {
        base.InitStat();
        skillSystem.skillInfos[0].cooltime = 9f;
        skillSystem.skillInfos[0].reduceCooltime = 0.5f;
        skillSystem.skillInfos[1].cooltime = 19f;
        skillSystem.skillInfos[1].reduceCooltime = 1.5f;
        skillSystem.skillInfos[2].cooltime = 24f;
        skillSystem.skillInfos[2].reduceCooltime = 2f;
        skillSystem.skillInfos[3].cooltime = 70f;
        skillSystem.skillInfos[3].reduceCooltime = 10f;
        skillSystem.skillInfos[4].cooltime = 0f;
        skillSystem.skillInfos[4].reduceCooltime = 0f;
        skillSystem.skillInfos[5].cooltime = 0f;
        skillSystem.skillInfos[5].reduceCooltime = 0f;
    }
    protected override void Update()
    {
        base.Update();
        if (!isWeaponPassiveOn)
        {
            if (skillSystem != null && skillSystem.skillInfos[4].CurrentLevel >= 1)
            {
                isWeaponPassiveOn = true;
            }
        }
        if (isWBuffOn)
        {
            watchDebuff = false;
            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, 5.5f);

            foreach (var hit in hits)
            {
                Vector3 direction = hit.transform.position - transform.position;
                float angleBetween = Vector3.Angle(transform.forward, direction);
                if (angleBetween < 120f * 0.5f)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        if (hit.GetComponent<Monster>().applyDebuffCheck[0])
                        {
                            if (!hasBuff)
                            {
                                playerTotalStat.moveSpeed += increaseMoveSpeedBuff;
                                hasBuff = true;
                            }
                            watchDebuff = true;
                            break;
                        }
                    }
                    else if (hit.CompareTag("Player"))
                    {
                        if (hit.GetComponent<PlayerBase>().applyDebuffCheck[0])
                        {
                            if (!hasBuff)
                            {
                                playerTotalStat.moveSpeed += increaseMoveSpeedBuff;
                                hasBuff = true;
                            }
                            watchDebuff = true;
                            break;
                        }
                    }

                }
            }
            if (!watchDebuff && hasBuff)
            {
                playerTotalStat.moveSpeed -= increaseMoveSpeedBuff;
                hasBuff = false;
            }
            // hits에 applyDebuffCheck[0]이 없는 경우 이동속도를 줄여라
        }
        else if (hasBuff)
        {
            playerTotalStat.moveSpeed -= increaseMoveSpeedBuff;
            hasBuff = false;
        }
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        if (!photonView.IsMine || enemy == default)
        {
            return;
        }
        PlayAudio(PlayerSound.ATTACK);
        if (isWeaponPassiveOn)
        {
            weaponStack++;
            if (weaponStack >= maxStack)
            {
                skillCooltimes[4] = false;
                weaponStack = maxStack;
            }
        }

        if (skillSystem.skillInfos[4].CurrentLevel == 2)
        {
            maxStack = 3;
        }

        if (isRBuffOn)
        {
            if (isWBuffOn)
            {
                if (enemy.GetComponent<Monster>() != null)
                {
                    if (enemy.GetComponent<Monster>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * (0.09f + 0.03f * (skillSystem.skillInfos[1].CurrentLevel - 1)));
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<Monster>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    if (enemy.GetComponent<PlayerBase>().applyDebuffCheck[0])
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * (0.09f + 0.03f * (skillSystem.skillInfos[1].CurrentLevel - 1)));
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<PlayerBase>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                        DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                        StartCoroutine(enemy.GetComponent<PlayerBase>().ContinousDamage(debuffdm, 0, 6f, 1f));
                    }
                }
                playerStat.nowHp += 10f + (0.09f * (skillSystem.skillInfos[1].CurrentLevel - 1)) + playerTotalStat.attackPower * 0.08f + playerTotalStat.skillPower * 0.05f;
            }
            else
            {
                if (enemy.GetComponent<Monster>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                    enemy.GetComponent<Monster>().TakeDamage(dm);
                    DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
                    StartCoroutine(enemy.GetComponent<Monster>().ContinousDamage(debuffdm, 0, 6f, 1f));
                }
                else if (enemy.GetComponent<PlayerBase>() != null)
                {
                    DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                    enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    DamageMessage debuffdm = new DamageMessage(gameObject, (10 + (5 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.03f) + (playerTotalStat.skillPower * 0.03f)) / 6f, 0, 6f);
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
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * (0.09f + 0.03f * (skillSystem.skillInfos[1].CurrentLevel - 1)));
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
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + playerTotalStat.attackPower * (0.09f + 0.03f * (skillSystem.skillInfos[1].CurrentLevel - 1)));
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    }
                    else
                    {
                        DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower);
                        enemy.GetComponent<PlayerBase>().TakeDamage(dm);
                    }
                }
                playerStat.nowHp += 10f + (0.09f * (skillSystem.skillInfos[1].CurrentLevel - 1)) + playerTotalStat.attackPower * 0.08f + playerTotalStat.skillPower * 0.05f;
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
        //enemy = null;
    }

    public override void Skill_Q()
    {
        base.Skill_Q();
        transform.LookAt(nowMousePoint);
        skillQ.SetActive(true);
        photonView.RPC("ShowRangeJackieQ", RpcTarget.All, true);
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 0);
        StartCoroutine(SkillCooltime(0, skillSystem.skillInfos[0].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
    }
    [PunRPC]
    public void ShowRangeJackieQ(bool flag)
    {
        skillQ.SetActive(flag);
    }

    public override void Skill_W()
    {
        base.Skill_W();
        isWBuffOn = true;
        playerController.ChangeState(new PlayerIdle());
        increaseMoveSpeedBuff = playerTotalStat.moveSpeed * 0.05f + (0.04f * (skillSystem.skillInfos[1].CurrentLevel - 1));
        StartCoroutine(SkillCooltime(1, skillSystem.skillInfos[1].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
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
        StartCoroutine(SkillCooltime(2, skillSystem.skillInfos[2].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
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

    public override void ExtraRange()
    {
        base.ExtraRange();
        skillQ.SetActive(false);
        photonView.RPC("ShowRangeJackieQ", RpcTarget.All, false);
        skillE.SetActive(false);
        photonView.RPC("ShowRangeJackieE", RpcTarget.All, false);
    }
    IEnumerator JackieJump()
    {
        float time = 0f;
        while (true)
        {
            if (time >= 0.15f)
            {
                transform.position = Vector3.Lerp(nowPos, targetPos, time * 1.5f);
                // transform.position += (dir * distance) * Time.deltaTime;
            }
            if (time >= 0.7f)
            {
                photonView.RPC("ShowRangeJackieE", RpcTarget.All, true);
                skillE.SetActive(true);
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
        float time_ = 0f;
        playerController.ChangeState(new PlayerIdle());
        playerAni.SetBool("isRbuff", true);
        playerAni.SetBool("isSkill", false);
        while (true)
        {
            if (time_ >= 15f && (playerController.playerState == PlayerController.PlayerState.IDLE || playerController.playerState == PlayerController.PlayerState.MOVE
            || playerController.playerState == PlayerController.PlayerState.ATTACKMOVE))
            {
                RBuffOff(time_);
                yield break;
            }
            time_ += Time.deltaTime;
            yield return null;
        }
    }

    private void RDamage(float time_)
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 4.5f, Vector3.up, 0f);
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<Monster>() != null)
            {
                enemyHunt.Add(hit.transform.gameObject.GetComponent<Monster>());
            }
            else if (hit.transform.gameObject.GetComponent<PlayerBase>() != null)
            {
                if (hit.transform.GetComponent<PlayerBase>() == this)
                {

                }
                else
                {
                    enemyPlayer.Add(hit.transform.gameObject.GetComponent<PlayerBase>());
                }
            }
        }
        float damage = 0f;
        float maxDamage = (250 + (125 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + 1.2f * playerTotalStat.attackPower + playerTotalStat.skillPower);
        float minDamage = (100 + (125 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + 0.9f * playerTotalStat.attackPower + 0.8f * playerTotalStat.skillPower);
        damage = maxDamage - minDamage * (time_ / 1.5f) + minDamage;
        if (damage >= maxDamage)
        {
            damage = maxDamage;
        }
        // -(200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower) * (time / 1.5f)
        // + (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower);
        DamageMessage dm = new DamageMessage(gameObject, damage);

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            enemyHunt[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            enemyPlayer[i].TakeDamage(dm);
        }
    }
    IEnumerator SkillCooltime(int skillType_, float cooltime_)
    {
        skillCooltimes[skillType_] = true;
        skillSystem.skillInfos[skillType_].currentCooltime = skillSystem.skillInfos[skillType_].cooltime * ((100 - playerTotalStat.coolDown) / 100);
        while (true)
        {
            if (skillSystem.skillInfos[skillType_].currentCooltime <= 0f)
            {
                skillCooltimes[skillType_] = false;
                yield break;
            }
            skillSystem.skillInfos[skillType_].currentCooltime -= Time.deltaTime;
            yield return null;
        }
    }

    private void RBuffOff(float time_)
    {
        RDamage(time_);
        isRBuffOn = false;
        playerAni.SetBool("isRbuff", false);
        playerAni.SetBool("isSkill", false);
        weapon.SetActive(true);
        playerController.ChangeState(new PlayerIdle());
        StartCoroutine(SkillCooltime(3, skillSystem.skillInfos[3].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
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
            else if (hit.transform.gameObject.GetComponent<PlayerBase>() != null && hit.transform.gameObject != this.gameObject)
            {
                enemyPlayer.Add(hit.transform.gameObject.GetComponent<PlayerBase>());
            }
        }

        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 50 + (50 * (skillSystem.skillInfos[2].CurrentLevel - 1)) + (playerTotalStat.attackPower * (0.42f + 0.12f * (skillSystem.skillInfos[2].CurrentLevel - 1))) + (playerTotalStat.skillPower * 0.60f));
            enemyPlayer[i].TakeDamage(dm);
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 50 + (50 * (skillSystem.skillInfos[2].CurrentLevel - 1)) + (playerTotalStat.attackPower * (0.42f + 0.12f * (skillSystem.skillInfos[2].CurrentLevel - 1))) + (playerTotalStat.skillPower * 0.60f));
            enemyHunt[i].TakeDamage(dm);
        }
    }

    [PunRPC]
    public void ShowRangeJackieE(bool flag)
    {
        skillE.SetActive(flag);
    }
    private void FirstQDamage()
    {
        enemyPlayer.Clear();
        enemyHunt.Clear();

        Collider[] hits;
        hits = Physics.OverlapSphere(transform.position, 2.5f);

        foreach (var hit in hits)
        {
            Vector3 direction = hit.transform.position - transform.position;
            float angleBetween = Vector3.Angle(transform.forward, direction);
            if (angleBetween < 150f * 0.5f)
            {
                if (hit.transform.gameObject.GetComponent<Monster>() != null)
                {
                    enemyHunt.Add(hit.transform.gameObject.GetComponent<Monster>());
                }
                else if (hit.transform.gameObject.GetComponent<PlayerBase>() != null)
                {
                    if (hit.transform.GetComponent<PlayerBase>() == this)
                    {

                    }
                    else
                    {
                        enemyPlayer.Add(hit.transform.gameObject.GetComponent<PlayerBase>());
                    }
                }
            }
        }
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.45f) + (playerTotalStat.skillPower * 0.40f));
            enemyPlayer[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.5f) + (playerTotalStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyPlayer[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }

        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.45f) + (playerTotalStat.skillPower * 0.40f));
            enemyHunt[i].TakeDamage(dm);
            DamageMessage debuffdm = new DamageMessage(gameObject, (30 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.5f) + (playerTotalStat.skillPower * 0.05f)) / 6f, 0, 6f);
            StartCoroutine(enemyHunt[i].ContinousDamage(debuffdm, 0, 6f, 1f));
        }
    }

    private void SecondQDamage()

    {
        enemyPlayer.Clear();
        enemyHunt.Clear();
        Collider[] hits;

        hits = Physics.OverlapSphere(transform.position, 2.5f);

        foreach (var hit in hits)
        {
            Vector3 direction = hit.transform.position - transform.position;
            float angleBetween = Vector3.Angle(transform.forward, direction);
            if (angleBetween < 150f * 0.5f)
            {
                if (hit.transform.gameObject.GetComponent<Monster>() != null)
                {
                    enemyHunt.Add(hit.transform.gameObject.GetComponent<Monster>());
                }
                else if (hit.transform.gameObject.GetComponent<PlayerBase>() != null)
                {
                    if (hit.transform.GetComponent<PlayerBase>() == this)
                    {

                    }
                    else
                    {
                        enemyPlayer.Add(hit.transform.gameObject.GetComponent<PlayerBase>());
                    }
                }
            }
        }
        for (int i = 0; i < enemyPlayer.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.7f) + (playerTotalStat.skillPower * 0.6f));
            enemyPlayer[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyHunt.Count; i++)
        {
            DamageMessage dm = new DamageMessage(gameObject, 25 + (25 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.7f) + (playerTotalStat.skillPower * 0.6f));
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
        StartCoroutine(WeaponSkillMove());
        // transform.position = transform.position + transform.forward.normalized * 2f;

        enemyPlayer.Clear();
        enemyHunt.Clear();
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.GetComponent<PlayerBase>() != null && hits[i].transform.gameObject != this.gameObject)
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
            float damage = playerTotalStat.attackPower * (1.2f + (0.5f * (skillSystem.skillInfos[4].CurrentLevel - 1))) + playerTotalStat.skillPower * 0.45f + enemyPlayer[i].playerTotalStat.maxHp * 0.07f;
            DamageMessage dm = new DamageMessage(gameObject, damage);
            enemyPlayer[i].TakeDamage(dm);
        }
        for (int i = 0; i < enemyHunt.Count; i++)
        {
            float damage = playerTotalStat.attackPower * (1.2f + (0.5f * (skillSystem.skillInfos[4].CurrentLevel - 1))) + playerTotalStat.skillPower * 0.45f + enemyHunt[i].monsterStatus.maxHp * 0.07f;
            DamageMessage dm = new DamageMessage(gameObject, damage);
            enemyHunt[i].TakeDamage(dm);
        }
    }

    public override void Skill_T()
    {
        base.Skill_T();
        if (prevHuntKill != huntKill)
        {
            if (isPassiveOn)
            {
                // 코루틴 멈추기
                StopCoroutine("PassiveBuff");
                playerTotalStat.attackPower -= increaseAttackPower;
                isPassiveOn = false;
                StartCoroutine(PassiveBuff(1));
            }
            else
            {
                StartCoroutine(PassiveBuff(1));
            }
            prevHuntKill = huntKill;
        }
        else if (prevPlayerKill != playerKill)
        {
            if (isPassiveOn)
            {
                StopCoroutine("PassiveBuff");
                playerTotalStat.attackPower -= increaseAttackPower;
                isPassiveOn = false;
                StartCoroutine(PassiveBuff(0));
            }
            else
            {
                StartCoroutine(PassiveBuff(0));
            }
            prevPlayerKill = playerKill;
        }
    }
    IEnumerator PassiveBuff(int killType_)
    {
        switch (killType_)
        {
            case 0:
                increaseAttackPower = playerTotalStat.attackPower * (0.14f + (0.09f * (skillSystem.skillInfos[0].CurrentLevel - 1)));
                playerTotalStat.attackPower += increaseAttackPower;
                isPassiveOn = true;
                break;
            case 1:
                increaseAttackPower = playerTotalStat.attackPower * (0.04f + (0.04f * (skillSystem.skillInfos[0].CurrentLevel - 1)));
                playerTotalStat.attackPower += increaseAttackPower;
                isPassiveOn = true;
                break;
        }
        yield return new WaitForSeconds(20f);
        playerTotalStat.attackPower -= increaseAttackPower;
        isPassiveOn = false;
    }
}
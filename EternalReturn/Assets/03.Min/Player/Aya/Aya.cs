using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Realtime;
using Photon.Pun;

public class Aya : PlayerBase
{
    private Vector3 targetPos;
    private Vector3 exceptYTragetPos;
    private Vector3 nowPos;
    private Vector3 dir;
    private float distance;
    public bool isWon = false;
    [SerializeField]
    private GameObject RRange = default;
    [SerializeField]
    private GameObject Bullet = default;
    protected override void Start()
    {
        weaponType = 10;
        base.Start();
        if (photonView.IsMine)
        {
            ItemManager.Instance.SetDefault(weaponType);
            ItemManager.Instance.GetItem(ItemManager.Instance.itemList[14]);


        }
        item[0] = 15;
        AddTotalStat();
        ItemChang();
    }

    protected override void InitStat()
    {
        base.InitStat();
        skillSystem.skillInfos[0].cooltime = 6.5f;
        skillSystem.skillInfos[0].reduceCooltime = 0.5f;
        skillSystem.skillInfos[1].cooltime = 17f;
        skillSystem.skillInfos[1].reduceCooltime = 2f;
        skillSystem.skillInfos[2].cooltime = 19f;
        skillSystem.skillInfos[2].reduceCooltime = 2f;
        skillSystem.skillInfos[3].cooltime = 80f;
        skillSystem.skillInfos[3].reduceCooltime = 20f;
        skillSystem.skillInfos[4].cooltime = 40f;
        skillSystem.skillInfos[4].reduceCooltime = 15f;
        skillSystem.skillInfos[5].cooltime = 0f;
        skillSystem.skillInfos[5].reduceCooltime = 0f;
    }

    protected override void Update()
    {
        if (!isWon)
        {
            base.Update();
        }
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
        SkillRange[0].SetActive(false);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.GetComponent<Monster>() || hit.transform.GetComponent<PlayerBase>())
                {
                    if (hit.transform.GetComponent<PlayerBase>() == this)
                    {

                    }
                    else
                    {
                        enemy = hit.transform.gameObject;
                    }
                }
            }
        }
        if (enemy != null)
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(enemy.transform.position, out navHit, 5.0f, NavMesh.AllAreas))
            {
                SetDestination(new Vector3(navHit.position.x, navHit.position.y, navHit.position.z));

                path = new NavMeshPath();
                playerNav.CalculatePath(destination, path);
                corners.Clear();
                for (int i = 0; i < path.corners.Length; i++)
                {
                    corners.Add(path.corners[i]);
                }
                currentCorner = 0;
            }
            StartCoroutine(MoveToEnemy());
        }
        else
        {
            playerController.ChangeState(new PlayerIdle());
        }
    }
    IEnumerator MoveToEnemy()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                playerController.ChangeState(new PlayerIdle());
                yield break;
            }
            if (ExceptY.ExceptYDistance(enemy.transform.position, transform.position) <= 4.6f)
            {
                playerAni.SetBool("isMove", false);
                transform.LookAt(enemy.transform);
                playerAni.SetBool("isSkill", true);
                playerAni.SetFloat("SkillType", 0);
                StartCoroutine(SkillCooltime(0, skillSystem.skillInfos[0].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
                break;
            }
            SkillMove();
            yield return null;
        }
    }


    public void SkillMove()
    {
        playerAni.SetBool("isMove", true);
        if (corners.Count > 0 && currentCorner < corners.Count)
        {
            if (Vector3.Distance(corners[currentCorner], transform.position) <= 0.2f)
            {
                currentCorner++;
            }
            if (currentCorner < corners.Count)
            {
                var dir = corners[currentCorner] - transform.position;
                Quaternion viewroate = Quaternion.LookRotation(dir);
                viewroate = Quaternion.Euler(transform.rotation.x, viewroate.eulerAngles.y, transform.rotation.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, viewroate, 6f * Time.deltaTime);
                transform.position += dir.normalized * Time.deltaTime * playerTotalStat.moveSpeed;
            }
            else
            {
                playerAni.SetBool("isMove", false);
            }
        }
    }

    private void FirstQDamage()
    {
        if (photonView.IsMine)
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

    private void SecondQDamage()
    {
        if (photonView.IsMine)
        {
            if (enemy.GetComponent<Monster>() != null)
            {
                DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + 60 + (55 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.7f));
                enemy.GetComponent<Monster>().TakeDamage(dm);
            }
            else if (enemy.GetComponent<PlayerBase>() != null)
            {
                DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + 60 + (55 * (skillSystem.skillInfos[0].CurrentLevel - 1)) + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.7f));
                enemy.GetComponent<PlayerBase>().TakeDamage(dm);
            }
        }
    }


    public override void Skill_W()
    {
        base.Skill_W();
        SkillRange[0].SetActive(false);
        transform.LookAt(nowMousePoint);
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 1);
        isWon = true;
        StartCoroutine(SkillCooltime(1, skillSystem.skillInfos[1].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
        StartCoroutine(WSkill());
    }


    private void Shot()
    {
        AyaBullet bullet = Instantiate(Bullet).GetComponent<AyaBullet>();
        bullet.transform.position = weapon.transform.position;
        bullet.damage = 30 + (25 * (skillSystem.skillInfos[1].CurrentLevel - 1))
        + (playerTotalStat.attackPower * (0.2f + (0.5f * (skillSystem.skillInfos[1].CurrentLevel - 1))))
        + (playerTotalStat.skillPower * (0.25f + (0.5f * (skillSystem.skillInfos[1].CurrentLevel - 1))));
        bullet.shootPlayer = this;
        // if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        // {
        //     photonView.RPC("CallShot", RpcTarget.MasterClient);
        // }
        // else if (PhotonNetwork.IsMasterClient && photonView.IsMine)
        // {
        //     photonView.RPC("CallShot", RpcTarget.Others);
        // }
    }

    [PunRPC]
    private void CallShot()
    {
        AyaBullet bullet = Instantiate(Bullet).GetComponent<AyaBullet>();
        bullet.transform.position = weapon.transform.position;
        bullet.damage = 30 + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.25f);
        bullet.shootPlayer = this;
    }

    IEnumerator WSkill()
    {
        float time = 0f;
        while (true)
        {
            if (time >= 3.3f)
            {
                isWon = false;
                playerAni.SetBool("isSkill", false);
                playerController.ChangeState(new PlayerIdle());
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWon = false;
                playerAni.SetBool("isSkill", false);
                playerController.ChangeState(new PlayerIdle());
                yield break;
            }
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(hit.point, out navHit, 5.0f, NavMesh.AllAreas))
                    {
                        SetDestination(new Vector3(navHit.position.x, navHit.position.y, navHit.position.z));
                        isMove = false;
                        path = new NavMeshPath();
                        playerNav.CalculatePath(destination, path);
                        corners.Clear();
                        for (int i = 0; i < path.corners.Length; i++)
                        {
                            corners.Add(path.corners[i]);
                        }
                        currentCorner = 0;
                    }
                }
            }
            MoveWithoutRotate();
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void MoveWithoutRotate()
    {
        if (corners.Count > 0 && currentCorner < corners.Count)
        {
            if (Vector3.Distance(corners[currentCorner], transform.position) <= 0.2f)
            {
                currentCorner++;
            }
            if (currentCorner < corners.Count)
            {
                var dir = corners[currentCorner] - transform.position;
                transform.position += dir.normalized * Time.deltaTime * playerTotalStat.moveSpeed;
            }
        }
    }

    public override void Skill_E()
    {
        base.Skill_E();
        SkillRange[2].SetActive(false);
        transform.LookAt(nowMousePoint);
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 2);
        corners.Clear();
        isMove = false;
        StartCoroutine(AyaDash());
        StartCoroutine(SkillCooltime(2, skillSystem.skillInfos[2].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
    }

    IEnumerator AyaDash()
    {
        targetPos = nowMousePoint;
        exceptYTragetPos = ExceptY.ExceptYPos(nowMousePoint);
        nowPos = ExceptY.ExceptYPos(transform.position);
        distance = ExceptY.ExceptYDistance(transform.position, targetPos);
        dir = Vector3.Normalize(exceptYTragetPos - nowPos);
        distance = 4f;
        float time = 0f;
        while (true)
        {
            transform.position += (dir * distance) * Time.deltaTime / 0.7f;
            if (time >= 0.7f)
            {
                yield break;
            }
            yield return null;
            time += Time.deltaTime;
        }
    }

    [PunRPC]
    public void ShowRangeAyaR(bool flag)
    {
        RRange.SetActive(flag);
    }
    public override void Skill_R()
    {
        base.Skill_R();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 3);
        RRange.SetActive(true);
        photonView.RPC("ShowRangeAyaR", RpcTarget.All, true);
        StartCoroutine(RDamage());
    }

    IEnumerator RDamage()
    {
        enemyPlayer.Clear();
        enemyHunt.Clear();
        float time = 0f;
        while (true)
        {
            if (time >= 0.5f && Input.GetKeyDown(KeyCode.R))
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
                float maxDamage = (400 + (200 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + 0.8f * playerTotalStat.attackPower + 1.2f * playerTotalStat.skillPower);
                float minDamage = (200 + (100 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower);
                damage = maxDamage - minDamage * (time / 1.5f) + minDamage;
                // -(200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower) * (time / 1.5f)
                // + (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower);
                DamageMessage dm = new DamageMessage(gameObject, damage);

                for (int i = 0; i < enemyHunt.Count; i++)
                {
                    enemyHunt[i].TakeDamage(dm);
                    enemyHunt[i].Debuff(dm, 3, time);
                }
                for (int i = 0; i < enemyPlayer.Count; i++)
                {
                    enemyPlayer[i].TakeDamage(dm);
                    enemyPlayer[i].Debuff(3, time);
                }
                RRange.SetActive(false);
                photonView.RPC("ShowRangeAyaR", RpcTarget.All, false);
                playerAni.SetBool("isREnd", true);
                StartCoroutine(SkillCooltime(3, skillSystem.skillInfos[3].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
                yield break;
            }
            if (time >= 1.5f)
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
                damage = (400 + (200 * (skillSystem.skillInfos[3].CurrentLevel - 1)) + 0.8f * playerTotalStat.attackPower + 1.2f * playerTotalStat.skillPower);
                Debug.Log(damage);
                DamageMessage dm = new DamageMessage(gameObject, damage);
                for (int i = 0; i < enemyHunt.Count; i++)
                {
                    enemyHunt[i].TakeDamage(dm);
                }
                for (int i = 0; i < enemyPlayer.Count; i++)
                {
                    enemyPlayer[i].TakeDamage(dm);
                }
                RRange.SetActive(false);
                photonView.RPC("ShowRangeAyaR", RpcTarget.All, false);
                StartCoroutine(SkillCooltime(3, skillSystem.skillInfos[3].cooltime * ((100 - playerTotalStat.coolDown) / 100)));
                playerAni.SetBool("isREnd", true);
                yield break;
            }

            yield return null;
            time += Time.deltaTime;
        }
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

    public override void MotionEnd()
    {
        base.MotionEnd();
        playerAni.SetBool("isREnd", false);
    }

    public override void ExtraAni()
    {
        base.ExtraAni();
        playerAni.SetBool("isREnd", false);
    }
}

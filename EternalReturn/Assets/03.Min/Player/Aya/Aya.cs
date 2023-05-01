using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        base.Start();
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
                    enemy = hit.transform.gameObject;
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
                StartCoroutine(SkillCooltime(0, 6.5f));
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

    private void SecondQDamage()
    {
        if (enemy.GetComponent<Monster>() != null)
        {
            DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + 60 + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.7f));
            enemy.GetComponent<Monster>().TakeDamage(dm);
        }
        else if (enemy.GetComponent<PlayerBase>() != null)
        {
            DamageMessage dm = new DamageMessage(gameObject, playerTotalStat.attackPower + 60 + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.7f));
            enemy.GetComponent<PlayerBase>().TakeDamage(dm);
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
        StartCoroutine(WSkill());
    }

    private void Shot()
    {
        AyaBullet bullet = Instantiate(Bullet, weapon.transform).GetComponent<AyaBullet>();
        bullet.damage = 30 + (playerTotalStat.attackPower * 0.2f) + (playerTotalStat.skillPower * 0.25f);
        bullet.shootPlayer = this;
    }
    IEnumerator WSkill()
    {
        float time = 0f;
        while (true)
        {
            if (time >= 3f)
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
        StartCoroutine(SkillCooltime(2, 19f));
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

    public override void Skill_R()
    {
        base.Skill_R();
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 3);
        RRange.SetActive(true);
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
                float damage = 0f;
                damage = (400 + 0.8f * playerTotalStat.attackPower + 1.2f * playerTotalStat.skillPower)
                - (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower) * (time / 1.5f)
                + (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower);
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
                playerAni.SetBool("isREnd", true);
                StartCoroutine(SkillCooltime(3, 80f));
                yield break;
            }
            if (time >= 1.5f)
            {
                RaycastHit[] hits;
                hits = Physics.SphereCastAll(transform.position, 1.0f, Vector3.up, 0f);

                float damage = 0f;
                damage = (400 + 0.8f * playerTotalStat.attackPower + 1.2f * playerTotalStat.skillPower)
                - (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower) * (time / 1.5f)
                + (200 + 0.4f * playerTotalStat.attackPower + 0.6f * playerTotalStat.skillPower);
                DamageMessage dm = new DamageMessage(gameObject, damage);

                for (int i = 0; i < enemyHunt.Count; i++)
                {
                    enemyHunt[i].TakeDamage(dm);
                }
                for (int i = 0; i < enemyPlayer.Count; i++)
                {
                    enemyPlayer[i].TakeDamage(dm);
                }
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
                RRange.SetActive(false);
                StartCoroutine(SkillCooltime(3, 80f));
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
        yield return new WaitForSeconds(cooltime_);
        skillCooltimes[skillType_] = false;
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

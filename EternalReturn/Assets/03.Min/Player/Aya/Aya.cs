using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            // if (ExceptY.ExceptYDistance(enemy.transform.position, transform.position) >= 4.6f)
            // {

            //     // playerController.ChangeState(new PlayerIdle());
            // }
            // else
            // {
            //     transform.LookAt(enemy.transform);
            //     playerAni.SetBool("isSkill", true);
            //     playerAni.SetFloat("SkillType", 0);
            //     StartCoroutine(SkillCooltime(0, 6.5f));
            // }
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
        playerAni.SetBool("isSkill", true);
        playerAni.SetFloat("SkillType", 1);
        // StartCoroutine(SkillCooltime(1, 19f));
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

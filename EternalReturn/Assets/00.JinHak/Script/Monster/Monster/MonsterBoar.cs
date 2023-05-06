using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoar : Monster
{
    [SerializeField]
    GameObject skillRange = default;

    private List<PlayerBase> collisionTarget = new List<PlayerBase>();
    private float skillCoolTime = 15f;
    private bool isAssult = false;

    protected override void SetStatus()
    {
        base.SetStatus();
        monsterController.isSkillAble = true;
    }

    public override void Skill()
    {
        base.Skill();
        StartCoroutine(SkillReady());
    }

    IEnumerator SkillReady()
    {
        audioSource.clip = sounds[4];
        transform.LookAt(monsterController.monster.firstAttackPlayer.transform);
        StartCoroutine(SkillCoolTime(skillCoolTime));
        skillRange.SetActive(true);
        monsterController.monsterAni.SetBool("isSkillReady", true);
        monsterController.navMeshAgent.enabled = false;
        yield return new WaitForSeconds(3f);
        audioSource.clip = sounds[5];
        SkillAssult();
    }

    void SkillAssult()
    {
        isAssult = true;
        monsterController.monsterRigid.velocity = transform.forward * 10f;
        monsterController.monsterAni.SetBool("isSkillReady", false);
        monsterController.monsterAni.SetBool("isSkill", true);
        skillRange.SetActive(false);
    }

    public void SkillEnd()
    {
        isAssult = false;
        monsterController.monsterRigid.velocity = Vector3.zero;
        monsterController.navMeshAgent.enabled = true;
        monsterController.isInSkillUse = false;
        monsterController.monsterAni.SetBool("isSkill", false);
        collisionTarget.Clear();
    }

    IEnumerator SkillCoolTime(float coolTime_)
    {
        yield return new WaitForSeconds(coolTime_);
        monsterController.isSkillAble = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(isAssult && other.tag == "Player")
        {
            PlayerBase nowTargetPlayer_ = other.GetComponent<PlayerBase>();
            if (!collisionTarget.Contains(other.GetComponent<PlayerBase>()))
            {
                DamageMessage dm = new DamageMessage(this.gameObject, 100);
                nowTargetPlayer_.TakeDamage(dm);

                collisionTarget.Add(other.GetComponent<PlayerBase>());
                nowTargetPlayer_.playerRigid.AddForce(transform.forward * 45, ForceMode.Impulse);
                StartCoroutine(NuckBackEnd(nowTargetPlayer_.playerRigid));
            }
        }
    }

    IEnumerator NuckBackEnd(Rigidbody targetRigid_)
    {
        yield return new WaitForSeconds(0.1f);
        targetRigid_.velocity = Vector3.zero;
        targetRigid_.GetComponent<PlayerBase>().Debuff(4, 1.2f);
    }
}

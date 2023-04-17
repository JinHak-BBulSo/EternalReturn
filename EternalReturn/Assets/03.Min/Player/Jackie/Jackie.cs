using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackie : PlayerBase
{
    private List<PlayerBase> enemyPlayers = new List<PlayerBase>();
    [SerializeField]
    private MeshCollider QMeshCol = default;
    protected override void Start()
    {
        base.Start();
        QMeshCol = Skill_Q_Range.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>();
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
}

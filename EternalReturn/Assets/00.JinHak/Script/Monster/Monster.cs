using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterStatus monsterStatus = default;
    private MonsterController monsterController;

    public string monsterName = default;
    
    public bool isSkillAble = false;
    public bool isAttackAble = false;

    // 영역 내 플레이어 캐싱
     

    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        SetStatus();
    }

    void Update()
    {

    }
    
    protected virtual void SetStatus()
    {
        

        isSkillAble = true;
    }
    public virtual void LevelUp()
    {
        /* each monster override using */
    }

    public virtual void Attack()
    {
        monsterController.monsterAni.SetBool("isAttack", true);
    }
    
    public virtual void Move()
    {

    }

    public virtual void Recall()
    {

    }

    public virtual void Beware()
    {

    }

    public virtual void Skill()
    {
        /* each monster override using */
    }

    public void Die()
    {

    }

    public virtual void ExitAttack()
    {
        monsterController.monsterAni.SetBool("isAttack", false);
    }

    public virtual void ExitSkill()
    {
    }
}

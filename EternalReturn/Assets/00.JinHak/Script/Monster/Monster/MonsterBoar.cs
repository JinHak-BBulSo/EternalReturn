using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoar : Monster
{
    public override void Skill()
    {
        base.Skill();

    }

    IEnumerator SkillReady()
    {
        yield return new WaitForSeconds(2f);
    }

    void SkillAssult()
    {
        
    }
}

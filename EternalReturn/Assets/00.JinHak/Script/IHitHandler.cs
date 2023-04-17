using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessage
{
    public GameObject causer;
    public float damageAmount;
    public int debuffIndex;
    public Debuff debuff;
    public DamageMessage(GameObject causer, float damageAmount, Debuff debuff_ = default, int debuffIndex = -1)
    {
        this.causer = causer;
        this.damageAmount = damageAmount;
        this.debuff = debuff_;
        this.debuffIndex = debuffIndex;
    }
}
public interface IHitHandler
{
    public void TakeDamage(DamageMessage message);
    public void TakeSolidDamage(DamageMessage message);

    IEnumerator ContinousDamage(DamageMessage message,Debuff debuff_, int debuffIndex_);

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessage
{
    public GameObject causer;
    public float damageAmount;
    public int debuffIndex;
    public DamageMessage(GameObject causer, float damageAmount, int debuffIndex = -1)
    {
        this.causer = causer;
        this.damageAmount = damageAmount;
        this.debuffIndex = debuffIndex;
    }
}
public interface IHitHandler
{
    public void TakeDamage(DamageMessage message);
    // public void TakeDamage(DamageMessage message, float damageAmount);
    //public void TakeSolidDamage(DamageMessage message);
    //public void TakeSolidDamage(DamageMessage message, float damageAmount);
    //IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_);
}
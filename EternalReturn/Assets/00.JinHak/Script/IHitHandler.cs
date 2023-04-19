using System.Collections;
using UnityEngine;

public class DamageMessage
{
    public GameObject causer;
    public float damageAmount;
    public int debuffIndex;
    public float continousTime;
    public DamageMessage(GameObject causer, float damageAmount, int debuffIndex = -1, float continousTime = 0)
    {
        this.causer = causer;
        this.damageAmount = damageAmount;
        this.debuffIndex = debuffIndex;
        this.continousTime = continousTime;
    }
}
public interface IHitHandler
{
    public void TakeDamage(DamageMessage message);
    public void TakeDamage(DamageMessage message, float damageAmount);
    public void TakeSolidDamage(DamageMessage message);
    public void TakeSolidDamage(DamageMessage message, float damageAmount);
    IEnumerator ContinousDamage(DamageMessage message, int debuffIndex_, float continousTime_, float tickTime_);
}
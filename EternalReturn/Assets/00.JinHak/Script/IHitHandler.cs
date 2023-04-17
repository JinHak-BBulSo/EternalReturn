using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessage
{
    public GameObject causer;
    public float damageAmount;
    public DamageMessage(GameObject causer, float damageAmount)
    {
        this.causer = causer;
        this.damageAmount = damageAmount;
    }
}
public interface IHitHandler
{
    public void TakeDamage(DamageMessage message)
    {
        /* virtual method */
    }
}
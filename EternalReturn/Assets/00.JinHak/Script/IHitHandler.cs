using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitHandler
{
    public void HitDamage(float targetHp_, float power_);
}

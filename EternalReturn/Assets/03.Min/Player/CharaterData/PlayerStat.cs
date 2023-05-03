using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat : Stat
{
    public float nowHp;
    public float maxHp;
    public float maxStamina;
    public float nowStamina;
    public PlayerExp playerExp = new PlayerExp(920f, 500f);
    public PlayerExp weaponExp = new PlayerExp(198f, 162f);
    public PlayerExp craftExp = new PlayerExp(430f, 120f);
    public PlayerExp searchExp = new PlayerExp(350f, 60f);
    public PlayerExp moveExp = new PlayerExp(300f, 20f);
    public PlayerExp healthExp = new PlayerExp(180f, 110f);
    public PlayerExp defExp = new PlayerExp(230f, 270);

    public enum PlayerExpType
    {
        None = -1,
        WEAPON,
        CRAFT,
        SEARCH,
        MOVE,
        HEALTH,
        DEF
    }
}

public class PlayerExp
{
    public int level = 1;
    public int maxLevel = 20;
    public float nowExp;
    public float maxExp;
    public float expDelta;

    public PlayerExp(float maxExp_, float expDelta_)
    {
        this.maxExp = maxExp_;
        this.expDelta = expDelta_;
    }
}
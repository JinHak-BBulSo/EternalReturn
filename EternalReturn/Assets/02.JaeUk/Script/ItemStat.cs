using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStat : Stat
{
    public int id;
    public int type;
    public int rare;
    public string name;
    public int count;
    public float weaponAttackSpeedPercent;
    public float weaponAttackRangePercent;
    public float foodIncrease;
    public int maxCount;
    public float craftTime = 0;
    public bool isInventory = false;
    public bool isEquipment = false;
    public bool isItemWishList = false;
    public ItemStat()
    {

    }
    public ItemStat(float attackPower_, float skillPower_, float basicAttackPower_, float defense_, float attackSpeed_, float coolDown_, float criticalPercent_, float criticalDamage_,
     float moveSpeed_, float visionRange_, float attackRange_, float damageReduce_, float tenacity_, float armorReduce_, float lifeSteel_, float extraHp_,
      float extraStamina_, float hpRegen_, float staminaRegen_, float weaponAttackSpeedPercent_, float weaponAttackRangePercent_)
    {
        attackPower = attackPower_;
        skillPower = skillPower_;
        basicAttackPower = basicAttackPower_;
        attackSpeed = attackSpeed_;
        coolDown = coolDown_;
        criticalPercent = criticalPercent_;
        criticalDamage = criticalDamage_;
        moveSpeed = moveSpeed_;
        visionRange = visionRange_;
        attackRange = attackRange_;
        damageReduce = damageReduce_;
        tenacity = tenacity_;
        armorReduce = armorReduce_;
        lifeSteel = lifeSteel_;
        extraHp = extraHp_;
        extraStamina = extraStamina_;
        hpRegen = hpRegen_;
        staminaRegen = staminaRegen_;
        weaponAttackSpeedPercent = weaponAttackSpeedPercent_;
        weaponAttackRangePercent = weaponAttackRangePercent_;
    }

    public ItemStat(ItemStat itemClone)
    {
        attackPower = itemClone.attackPower;
        skillPower = itemClone.skillPower;
        basicAttackPower = itemClone.basicAttackPower;
        attackSpeed = itemClone.attackSpeed;
        coolDown = itemClone.coolDown;
        criticalPercent = itemClone.criticalPercent;
        criticalDamage = itemClone.criticalDamage;
        moveSpeed = itemClone.moveSpeed;
        visionRange = itemClone.visionRange;
        attackRange = itemClone.attackRange;
        damageReduce = itemClone.damageReduce;
        tenacity = itemClone.tenacity;
        armorReduce = itemClone.armorReduce;
        lifeSteel = itemClone.lifeSteel;
        extraHp = itemClone.extraHp;
        extraStamina = itemClone.extraStamina;
        hpRegen = itemClone.hpRegen;
        staminaRegen = itemClone.staminaRegen;
        id = itemClone.id;
        type = itemClone.type;
        rare = itemClone.rare;
        name = itemClone.name;
        count = itemClone.count;
        foodIncrease = itemClone.foodIncrease;
        maxCount = itemClone.maxCount;
        switch (itemClone.rare)
        {
            case 1:
                craftTime = 1f;
                break;
            case 2:
                craftTime = 2f;
                break;
            case 3:
                craftTime = 4f;
                break;
        }
        switch (itemClone.type)
        {
            case 1:
                weaponAttackSpeedPercent = 0.58f;
                weaponAttackRangePercent = 1.05f;
                break;
            case 2:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.65f;
                break;
            case 3:
                weaponAttackSpeedPercent = 0.485f;
                weaponAttackRangePercent = 1.45f;
                break;
            case 4:
                weaponAttackSpeedPercent = 0.445f;
                weaponAttackRangePercent = 2.2f;
                break;
            case 5:
                weaponAttackSpeedPercent = 0.5f;
                weaponAttackRangePercent = 1.2f;
                break;
            case 6:
                weaponAttackSpeedPercent = 0.63f;
                weaponAttackRangePercent = 0.95f;
                break;
            case 7:
                weaponAttackSpeedPercent = 0.51f;
                weaponAttackRangePercent = 5.5f;
                break;
            case 8:
                weaponAttackSpeedPercent = 0.55f;
                weaponAttackRangePercent = 4.7f;
                break;
            case 9:
                weaponAttackSpeedPercent = 0.65f;
                weaponAttackRangePercent = 5.85f;
                break;
            case 10:
                weaponAttackSpeedPercent = 0.505f;
                weaponAttackRangePercent = 4.2f;
                break;
            case 11:
                weaponAttackSpeedPercent = 0.43f;
                weaponAttackRangePercent = 5.3f;
                break;
            case 12:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.55f;
                break;
            case 13:
                weaponAttackSpeedPercent = 0.61f;
                weaponAttackRangePercent = 1.25f;
                break;
            case 19:
                weaponAttackSpeedPercent = 0.485f;
                weaponAttackRangePercent = 1.45f;
                break;
            default:
                weaponAttackSpeedPercent = 0;
                break;
        }
    }

    public ItemStat(float attackPower_, float skillPower_, float basicAttackPower_, float defense_, float attackSpeed_, float coolDown_, float criticalPercent_, float criticalDamage_
    , float moveSpeed_, float visionRange_, float attackRange_, float damageReduce_, float tenacity_, float armorReduce_, float lifeSteel_, float extraHp_, float extraStamina_
    , float hpRegen_, float staminaRegen_, int id_, int type_, int rare_, string name_, int count_, int foodIncrease_, int maxCount_)
    {
        attackPower = attackPower_;
        skillPower = skillPower_;
        basicAttackPower = basicAttackPower_;
        attackSpeed = attackSpeed_;
        coolDown = coolDown_;
        criticalPercent = criticalPercent_;
        criticalDamage = criticalDamage_;
        moveSpeed = moveSpeed_;
        visionRange = visionRange_;
        attackRange = attackRange_;
        damageReduce = damageReduce_;
        tenacity = tenacity_;
        armorReduce = armorReduce_;
        lifeSteel = lifeSteel_;
        extraHp = extraHp_;
        extraStamina = extraStamina_;
        hpRegen = hpRegen_;
        staminaRegen = staminaRegen_;
        id = id_;
        type = type_;
        rare = rare_;
        name = name_;
        count = count_;
        foodIncrease = foodIncrease_;
        maxCount = maxCount_;
        switch (rare_)
        {
            case 1:
                craftTime = 1f;
                break;
            case 2:
                craftTime = 2f;
                break;
            case 3:
                craftTime = 4f;
                break;
        }
        switch (type_)
        {
            case 1:
                weaponAttackSpeedPercent = 0.58f;
                weaponAttackRangePercent = 1.05f;
                break;
            case 2:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.65f;
                break;
            case 3:
                weaponAttackSpeedPercent = 0.485f;
                weaponAttackRangePercent = 1.45f;
                break;
            case 4:
                weaponAttackSpeedPercent = 0.445f;
                weaponAttackRangePercent = 2.2f;
                break;
            case 5:
                weaponAttackSpeedPercent = 0.5f;
                weaponAttackRangePercent = 1.2f;
                break;
            case 6:
                weaponAttackSpeedPercent = 0.63f;
                weaponAttackRangePercent = 0.95f;
                break;
            case 7:
                weaponAttackSpeedPercent = 0.51f;
                weaponAttackRangePercent = 5.5f;
                break;
            case 8:
                weaponAttackSpeedPercent = 0.55f;
                weaponAttackRangePercent = 4.7f;
                break;
            case 9:
                weaponAttackSpeedPercent = 0.65f;
                weaponAttackRangePercent = 5.85f;
                break;
            case 10:
                weaponAttackSpeedPercent = 0.505f;
                weaponAttackRangePercent = 4.2f;
                break;
            case 11:
                weaponAttackSpeedPercent = 0.43f;
                weaponAttackRangePercent = 5.3f;
                break;
            case 12:
                weaponAttackSpeedPercent = 0.56f;
                weaponAttackRangePercent = 1.55f;
                break;
            case 13:
                weaponAttackSpeedPercent = 0.61f;
                weaponAttackRangePercent = 1.25f;
                break;
            case 19:
                weaponAttackSpeedPercent = 0.485f;
                weaponAttackRangePercent = 1.45f;
                break;
            default:
                weaponAttackSpeedPercent = 0;
                break;

        }
    }
}
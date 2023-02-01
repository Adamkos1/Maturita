using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    public enum WeaponType
    {
        SpellCaster,
        FaithCaster,
        PyroCaster,
        StraightSword,
        Axe,
        Dagger,
        Shield,
        SmallShield,
        Unarmed,
        Bow,
    }

    public enum AmmoType
    {
        Arrow,
        Bolt
    }

    public enum AttackType
    {
        Light,
        Heavy,
        Parry
    }

    public enum AICombatStyle
    {
        swordAndShield,
        archer
    }

    public enum AIAttackActionType
    {
        melleeAttackAction,
        magicAttackAction,
        rangedAttackAattack
    }

    public class Enums : MonoBehaviour
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int manaCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats,
                                                PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            Debug.Log("skusil si spell");
        }

        public virtual void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, 
                                                    PlayerWeaponSlotManager weaponSlotManager, CameraHandler cameraHandler, bool isLeftHanded)
        {
            Debug.Log("dal si spell");
            playerStats.SpendMana(manaCost);
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            Debug.Log("skusil si spell");
            Destroy(instantiatedWarmUpSpellFX, 5f);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, CameraHandler cameraHandler, bool isLeftHanded)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager, cameraHandler, isLeftHanded);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("dal si spell");
            Destroy(instantiatedSpellFX, 1.5f);
        }

    }

}
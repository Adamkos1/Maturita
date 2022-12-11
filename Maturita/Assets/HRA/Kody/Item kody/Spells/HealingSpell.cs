using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("skusil si spell");
            Destroy(instantiatedWarmUpSpellFX, 5f);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager, CameraHandler cameraHandler)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager, cameraHandler);
            //GameObject instantiatedSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("dal si spell");
        }

    }

}
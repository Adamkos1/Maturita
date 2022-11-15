using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            //GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("skusil si spell");
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            GameObject instantiatedSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("dal si spell");
        }

    }

}
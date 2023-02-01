using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            Debug.Log("skusil si spell");
            Destroy(instantiatedWarmUpSpellFX, 5f);
        }

        public override void SuccessfullyCastSpell(CharacterManager character)
        {
            base.SuccessfullyCastSpell(character);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, character.transform);
            character.characterStatsManager.HealCharacter(healAmount);
            Debug.Log("dal si spell");
            Destroy(instantiatedSpellFX, 1.5f);
        }

    }

}
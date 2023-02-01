using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Miricle Spell Action")]

    public class MiricleSpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isFaithSpell)
            {
                if (character.characterStatsManager.currentMana >= character.characterInventoryManager.currentSpell.manaCost)
                {
                    character.characterInventoryManager.currentSpell.AttemptToCastSpell(character);
                }
                else
                {
                    character.characterAnimatorManager.EraseHandIKForWeapon();
                    character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }
}
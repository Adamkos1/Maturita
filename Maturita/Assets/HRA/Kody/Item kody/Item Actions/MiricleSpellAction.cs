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

            if (character.characterInventoryManager.healSpell != null && character.characterInventoryManager.healSpell.isFaithSpell)
            {
                if (character.characterStatsManager.currentMana >= character.characterInventoryManager.healSpell.manaCost)
                {
                    character.characterInventoryManager.healSpell.AttemptToCastSpell(character);
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
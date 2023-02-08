using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Magic Spell Action")]

    public class MagicSpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.characterInventoryManager.pyroSpell != null && character.characterInventoryManager.pyroSpell.isMagicSpell)
            {
                if (character.characterStatsManager.currentMana >= character.characterInventoryManager.pyroSpell.manaCost)
                {
                    character.characterInventoryManager.pyroSpell.AttemptToCastSpell(character);
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
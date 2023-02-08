using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Pyromancy Spell Action")]

    public class PyromancySpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.characterInventoryManager.pyroSpell != null && character.characterInventoryManager.pyroSpell.isPyroSpell)
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
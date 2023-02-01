using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Block Action")]

    public class BlockingAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.isBlocking)
                return;

            if (character.isTwoHandingWeapon)
                return;

            if (character.isHoldingArrow)
                return;

            character.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();

            character.characterAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            character.isBlocking = true;
        }
    }

}

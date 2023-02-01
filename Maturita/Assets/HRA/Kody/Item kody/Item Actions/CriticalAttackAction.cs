using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Critica lAttack Action")]

    public class CriticalAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            character.characterCombatManager.AttemptBackStabOrRiposte();
        }
    }
}
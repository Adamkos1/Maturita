using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void GrantWeaponAttackPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.totalPoiseDefence + characterManager.characterStatsManager.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.armorPoiseBonus;
        }

        public void DrainStaminaLightAttack()
        {
            
        }

        public void DrainStaminaHeavyAttack()
        {
            
        }
    }

}

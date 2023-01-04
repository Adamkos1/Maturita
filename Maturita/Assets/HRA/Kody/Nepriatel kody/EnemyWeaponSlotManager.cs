using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void GrantWeaponAttackPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.totalPoiseDefence + characterStatsManager.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
        }

        public void DrainStaminaLightAttack()
        {
            
        }

        public void DrainStaminaHeavyAttack()
        {
            
        }
    }

}

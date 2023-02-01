using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager playerManager;


        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        public override void DrainStaminaBasedOnAttack()
        {
            if(playerManager.isUsingRightHand)
            {
                if(currentAttackType == AttackType.Light)
                {
                    playerManager.playerStatsManager.DrainStamina(playerManager.playerInventoryManager.rightWeapon.baseStaminaCost * playerManager.playerInventoryManager.rightWeapon.lightAttackDamageModifier);
                }
                else if(currentAttackType == AttackType.Heavy)
                {
                    playerManager.playerStatsManager.DrainStamina(playerManager.playerInventoryManager.rightWeapon.baseStaminaCost * playerManager.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
            }
            else if (playerManager.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.Light)
                {
                    playerManager.playerStatsManager.DrainStamina(playerManager.playerInventoryManager.leftWeapon.baseStaminaCost * playerManager.playerInventoryManager.leftWeapon.lightAttackDamageModifier);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    playerManager.playerStatsManager.DrainStamina(playerManager.playerInventoryManager.leftWeapon.baseStaminaCost * playerManager.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.Parry)
                {
                    playerManager.playerStatsManager.DrainStamina(playerManager.playerInventoryManager.leftWeapon.baseStaminaCost * playerManager.playerInventoryManager.leftWeapon.lightAttackDamageModifier);
                }
            }
        }

        public override void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, string blockAnimation)
        {
            base.AttemptBlock(attackingWeapon, physicalDamage, blockAnimation);
            playerManager.playerStatsManager.staminaBar.SetCurrentStamina(Mathf.RoundToInt(playerManager.playerStatsManager.currentStamina));
        }
    }

}

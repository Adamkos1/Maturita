using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager characterManager;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if(characterManager.isUsingRightHand)
            {
                characterManager.characterStatsManager.blockingPhysicalDamageAbsorption = characterManager.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                characterManager.characterStatsManager.blockingStabilityRating = characterManager.characterInventoryManager.rightWeapon.stability;
            }
            else if(characterManager.isUsingLeftHand)
            {
                characterManager.characterStatsManager.blockingPhysicalDamageAbsorption = characterManager.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                characterManager.characterStatsManager.blockingStabilityRating = characterManager.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {

        }

        public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, string blockAnimation)
        {
            float staminaDamageAbsorption = (physicalDamage * attackingWeapon.guardBreakModifier) * (characterManager.characterStatsManager.blockingStabilityRating / 100);

            float staminaDamage = (physicalDamage * attackingWeapon.guardBreakModifier) - staminaDamageAbsorption;

            characterManager.characterStatsManager.currentStamina = characterManager.characterStatsManager.currentStamina - staminaDamage;

            if(characterManager.characterStatsManager.currentStamina <= 0)
            {
                characterManager.isBlocking = false;
                characterManager.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
            }
            else
            {
                characterManager.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }
        }
    }

}

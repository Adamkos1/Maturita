using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;

        LayerMask backStabLayer = 1 << 11;
        LayerMask riposteLayer = 1 << 12;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Attack Animations")]
        public string oh_Light_Attack_01 = "Oh_Light_Attack_01";
        public string oh_Light_Attack_02 = "Oh_Light_Attack_02";
        public string oh_Heavy_Attack_01 = "Oh_Heavy_Attack_01";
        public string oh_Runing_Attack_01 = "Oh_Runing_Attack_01";
        public string oh_Jumping_Attack_01 = "Oh_Jumping_Attack_01";
        public string oh_Charge_Attack_01 = "Oh_Charging_Attack_Charge_01";

        public string th_Runing_Attack_01 = "Th_Runing_Attack_01";
        public string th_Jumping_Attack_01 = "Th_Jumping_Attack_01";
        public string th_Light_Attack_01 = "Th_Light_Attack_01";
        public string th_Light_Attack_02 = "Th_Light_Attack_02";
        public string th_Heavy_Attack_01 = "Th_Heavy_Attack_01";
        public string th_Charge_Attack_01 = "Th_Charging_Attack_Charge_01";

        public string weaponArt = "Weapon Art";

        public string lastAttack;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if(character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
            }
            else if(character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {

        }

        public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, string blockAnimation)
        {
            float staminaDamageAbsorption = (physicalDamage * attackingWeapon.guardBreakModifier) * (character.characterStatsManager.blockingStabilityRating / 100);

            float staminaDamage = (physicalDamage * attackingWeapon.guardBreakModifier) - staminaDamageAbsorption;

            character.characterStatsManager.currentStamina = character.characterStatsManager.currentStamina - staminaDamage;

            if(character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
            character.animator.SetBool("isFiringSpell", true);
        }


        public void AttemptBackStabOrRiposte()
        {
            if (character.characterStatsManager.currentStamina <= 0)
                return;


            RaycastHit hit;

            if (Physics.Raycast(character.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.4f, backStabLayer))
            {
                character.characterAnimatorManager.EraseHandIKForWeapon();
                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

                if (enemyChracterManager != null)
                {
                    character.transform.position = enemyChracterManager.backStabCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = character.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - character.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                    character.transform.rotation = targetRotation;

                    int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                }
            }

            else if (Physics.Raycast(character.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                character.characterAnimatorManager.EraseHandIKForWeapon();
                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

                if (enemyChracterManager != null && enemyChracterManager.canBeRiposted)
                {
                    character.transform.position = enemyChracterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = character.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - character.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                    character.transform.rotation = targetRotation;

                    int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

    }

}

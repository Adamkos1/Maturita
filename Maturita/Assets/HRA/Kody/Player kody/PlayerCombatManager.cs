using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager playerManager;

        [Header("Attack Animations")]
        public string oh_Light_Attack_01 = "Oh_Light_Attack_01";
        public string oh_Light_Attack_02 = "Oh_Light_Attack_02";
        public string oh_Heavy_Attack_01 = "Oh_Heavy_Attack_01";
        public string oh_Runing_Attack_01 = "Oh_Runing_Attack_01";
        public string oh_Jumping_Attack_01 = "Oh_Jumping_Attack_01";
        public string oh_Charge_Attack_01 = "Oh_Charging_Attack_Charge_01";

        public string th_Runing_Attack_01 = "Th_Runing_Attack_01";
        public string th_Jumping_Attack_01 = "Th_Jumping_Attack_01";
        public  string th_Light_Attack_01 = "Th_Light_Attack_01";
        public string th_Light_Attack_02 = "Th_Light_Attack_02";
        public string th_Heavy_Attack_01 = "Th_Heavy_Attack_01";
        public string th_Charge_Attack_01 = "Th_Charging_Attack_Charge_01";

        public string weaponArt = "Weapon Art";

        public string lastAttack;

        LayerMask backStabLayer = 1 << 11;
        LayerMask riposteLayer = 1 << 12;


        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void SuccessfullyCastSpell()
        {
            playerManager.playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerManager.playerAnimatorManager, playerManager.playerStatsManager, playerManager.playerWeaponSlotManager, playerManager.cameraHandler, playerManager.isUsingLeftHand);
            playerManager.animator.SetBool("isFiringSpell", true);
        }

        public void AttemptBackStabOrRiposte()
        {
            if (playerManager.playerStatsManager.currentStamina <= 0)
                return;

                
            RaycastHit hit;

            if(Physics.Raycast(playerManager.inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.4f, backStabLayer))
            {
                playerManager.playerAnimatorManager.EraseHandIKForWeapon();
                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerManager.playerWeaponSlotManager.rightHandDamageCollider;

                if (enemyChracterManager != null)
                {
                    playerManager.transform.position = enemyChracterManager.backStabCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerManager.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    playerManager.playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                }
            }

            else if (Physics.Raycast(playerManager.inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                playerManager.playerAnimatorManager.EraseHandIKForWeapon();
                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerManager.playerWeaponSlotManager.rightHandDamageCollider;

                if (enemyChracterManager != null && enemyChracterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyChracterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerManager.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    playerManager.playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }

}

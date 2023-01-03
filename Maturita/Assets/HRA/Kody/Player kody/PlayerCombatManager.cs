using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerAnimatorManager playerAnimatorHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;
        InputHandler inputHandler;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        CameraHandler cameraHandler;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attack Animations")]
        string oh_Light_Attack_01 = "Oh_Light_Attack_01";
        string oh_Light_Attack_02 = "Oh_Light_Attack_02";
        string oh_Heavy_Attack_01 = "Oh_Heavy_Attack_01";

        string th_Light_Attack_01 = "Th_Light_Attack_01";
        string th_Light_Attack_02 = "Th_Light_Attack_02";
        string th_Heavy_Attack_01 = "Th_Heavy_Attack_01";

        string weaponArt = "Weapon Art";

        public string lastAttack;

        LayerMask backStabLayer = 1 << 11;
        LayerMask riposteLayer = 1 << 12;


        private void Awake()
        {
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerManager = GetComponent<PlayerManager>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimatorHandler.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_Light_Attack_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_Light_Attack_02, true);
                }

                else if(lastAttack == th_Light_Attack_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(th_Light_Attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorHandler.PlayTargetAnimation(th_Light_Attack_01, true);
                lastAttack = th_Light_Attack_01;
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation(oh_Light_Attack_01, true);
                lastAttack = oh_Light_Attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;
            playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorHandler.PlayTargetAnimation(th_Heavy_Attack_01, true);
                lastAttack = th_Heavy_Attack_01;
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation(oh_Heavy_Attack_01, true);
                lastAttack = oh_Heavy_Attack_01;
            }
        }

        #region Input Actions

        public void HandleRBAction()
        {
            if(playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }

        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {

            }
        }

        public void HandleLBAction()
        {
            PerFormLBBlockingAction();
        }


        #endregion

        #region Attack Action

        private void PerformRBMeleeAction()
        {

            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);

            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;

            if(weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }    
            }

            else if(weapon.weaponType == WeaponType.PyroCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if(isTwoHanding)
            {

            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation(weaponArt, true);
            }

        }

        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManager, cameraHandler);
            playerAnimatorHandler.animator.SetBool("isFiringSpell", true);
        }

        #endregion

        #region Defense Actions

        private void PerFormLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        #endregion

        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, backStabLayer))
            {
                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyChracterManager != null)
                {
                    playerManager.transform.position = enemyChracterManager.backStabCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyChracterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                }
            }

            else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {

                CharacterManager enemyChracterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyChracterManager != null && enemyChracterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyChracterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyChracterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyChracterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerAnimatorManager playerAnimatorManager;
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
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void HandleHoldRBAction()
        {
            if(playerManager.isTwoHandingWeapon)
            {
                PerformRBRangedAction();
            }
            else
            {

            }
        }

        public void HandleRBAction()
        {
            //playerAnimatorManager.EraseHandIKForWeapon();

            if(playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                PerformMagicAction(playerInventoryManager.rightWeapon, false);
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
            if(playerManager.isTwoHandingWeapon)
            {
                if(playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if(playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield && !playerManager.isTwoHandingWeapon)
                {
                    PerFormLBBlockingAction();
                }
                else if(playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
                        playerInventoryManager.leftWeapon.weaponType == WeaponType.PyroCaster)
                {
                    PerformMagicAction(playerInventoryManager.leftWeapon, true);
                    playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
                }
            }
        }


        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_Light_Attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(oh_Light_Attack_02, true);
                }

                else if (lastAttack == th_Light_Attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(th_Light_Attack_02, true);
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
                playerAnimatorManager.PlayTargetAnimation(th_Light_Attack_01, true);
                lastAttack = th_Light_Attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_Light_Attack_01, true);
                lastAttack = oh_Light_Attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_Heavy_Attack_01, true);
                lastAttack = th_Heavy_Attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_Heavy_Attack_01, true);
                lastAttack = oh_Heavy_Attack_01;
            }
        }

        private void DrawArrowAction()
        {
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01_R", true);
            GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_ONLY_Draw_01");
            playerEffectsManager.currentRangedFX = loadedArrow;
        }

        public void FireArrowAction()
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_ONLY_Fire_01");
            Destroy(playerEffectsManager.currentRangedFX);

            //resetovat hracov live arrow
            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
            playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

            //vytvorit a vystrelit live arrow
            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageColider damageColider = liveArrow.GetComponentInChildren<RangedProjectileDamageColider>();

            //dat naboju rychlost
            if(cameraHandler.currentLockOnTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(transform.forward);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            }

            rigidbody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardCelocity);
            rigidbody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);
            rigidbody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidbody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            //dat live arrowu damage
            damageColider.characterManager = playerManager;
            damageColider.ammoItem = playerInventoryManager.currentAmmo;
            damageColider.currentWeaponDamage = playerInventoryManager.currentAmmo.physicalDamage;

        }


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

                playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);

            }
        }

        private void PerformRBRangedAction()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerAnimatorManager.EraseHandIKForWeapon();

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            if(!playerManager.isHoldingArrow)
            {
                if(playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }

        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting)
                return;

            if (weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorManager.EraseHandIKForWeapon();
                        playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                    }
                }    
            }

            else if(weapon.weaponType == WeaponType.PyroCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorManager.EraseHandIKForWeapon();
                        playerAnimatorManager.PlayTargetAnimation("Shrug", true);
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
                playerAnimatorManager.PlayTargetAnimation(weaponArt, true);
            }

        }

        private void PerFormLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            if (playerManager.isTwoHandingWeapon)
                return;

            if (playerManager.isHoldingArrow)
                return;

            playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void PerformLBAimingAction()
        {
            playerAnimatorManager.EraseHandIKForWeapon();
            //playerAnimatorManager.animator.SetBool("isAiming", true);
        }


        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, cameraHandler, playerManager.isUsingLeftHand);
            playerAnimatorManager.animator.SetBool("isFiringSpell", true);
        }

        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerAnimatorManager.EraseHandIKForWeapon();

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

                    playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
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

                    playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyChracterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }

}

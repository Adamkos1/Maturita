using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;
        InputHandler inputHandler;
        QuickSlotsUI quickSlotsUI;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            inputHandler = GetComponent<InputHandler>();
        }

        public override void LoadWeaponOnSlot (WeaponItem weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        if (leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }
                        else if (leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                        {
                            backSlotForShield.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }

                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);

                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                        backSlotForShield.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if(isLeft)
                {
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        public void DrainStaminaLightAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

    }

}

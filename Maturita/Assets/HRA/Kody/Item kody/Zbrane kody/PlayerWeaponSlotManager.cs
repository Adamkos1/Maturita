using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager playerManager;
        QuickSlotsUI quickSlotsUI;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
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
                    playerManager.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (playerManager.inputHandler.twoHandFlag)
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
                        playerManager.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);

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
                    playerManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if(isLeft)
                {
                    playerManager.playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    playerManager.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerManager.playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                    playerManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        public void DrainStaminaLightAttack()
        {
            WeaponItem currentWeaponBeingUsed = playerManager.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            playerManager.playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(currentWeaponBeingUsed.baseStamina * currentWeaponBeingUsed.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            WeaponItem currentWeaponBeingUsed = playerManager.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            playerManager.playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(currentWeaponBeingUsed.baseStamina * currentWeaponBeingUsed.heavyAttackMultiplier));
        }

    }

}

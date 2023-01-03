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
        Animator animator;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attacking Weapon")]
        public WeaponItem attackingWeapon;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            inputHandler = GetComponent<InputHandler>();
            LoadWeaponHolderSlots();
        }

        private void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }

                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
                else if (weaponSlot.isShieldBackSlot)
                {
                    backSlotForShield = weaponSlot;
                }
            }
        }

        public void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
        }

        public void LoadWeaponOnSlot (WeaponItem weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        if (leftHandSlot.currentWeapon.isMeleeWeapon)
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }
                        else if (leftHandSlot.currentWeapon.isShieldWeapon)
                        {
                            backSlotForShield.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }

                        leftHandSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.two_hand_idle, 0.2f);

                    }
                    else
                    {
                        animator.CrossFade("Both Arms Empty", 0.2f);
                        backSlot.UnloadWeaponAndDestroy();
                        backSlotForShield.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if(isLeft)
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                }
            }

        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamgeCollider()
        {
            if (playerInventoryManager.leftWeapon.isUnarmed)
                return;

            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventoryManager.leftWeapon.baseDamage;

            leftHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
            playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        private void LoadRightWeaponDamgeCollider()
        {
            if (playerInventoryManager.rightWeapon.isUnarmed)
                return;

            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventoryManager.rightWeapon.baseDamage;

            rightHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
            playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

        }

        public void OpenDamageCollider()
        {
            if(playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
                playerEffectsManager.PlayWeaponFX(false);
            }
            else
            {
                leftHandDamageCollider.EnableDamageCollider();
                playerEffectsManager.PlayWeaponFX(true);
            }
        }

        public void CloseDamageCollider()
        { 
            if(rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DissableDamageCollider();
                playerEffectsManager.StopWeaponFX(false);
            }

            if(leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DissableDamageCollider();
                playerEffectsManager.StopWeaponFX(false);

            }
        }

        #endregion

        #region Handle Weapon Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        #region Handle Weapon's Poise Bonus

        public void GrantWeaponAttackPoiseBonus()
        {
            playerStatsManager.totalPoiseDefence = playerStatsManager.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStatsManager.totalPoiseDefence = playerStatsManager.armorPoiseBonus;
        }

        #endregion
    }

}

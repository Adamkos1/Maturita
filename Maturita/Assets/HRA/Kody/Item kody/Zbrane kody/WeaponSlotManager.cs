using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;
        WeaponHolderSlot backSlotForShield;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        public PlayerManager playerManager;
        public WeaponItem attackingWeapon;

        Animator animator;

        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;

        QuickSlotsUI quickSlotsUI;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if(weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
                else if(weaponSlot.isShieldBackSlot)
                {
                    backSlotForShield = weaponSlot;
                }
            }
        }

        public void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }

        public void LoadWeaponOnSlot (WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamgeCollider();
                quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);

                #region Handle Left Weapon idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            { 
                if (inputHandler.twoHandFlag)
                {
                    if(leftHandSlot.currentWeapon.isMeleeWeapon)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    }
                    else if(leftHandSlot.currentWeapon.isShieldWeapon)
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

                    #region Handle Right Weapon idle Animations
                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamgeCollider();
                quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamgeCollider()
        {
            if (playerInventory.leftWeapon.isUnarmed)
                return;

            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        }

        private void LoadRightWeaponDamgeCollider()
        {
            if (playerInventory.rightWeapon.isUnarmed)
                return;

            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        }

        public void OpenDamageCollider()
        {
            if(playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        { 
                rightHandDamageCollider.DissableDamageCollider();
                leftHandDamageCollider.DissableDamageCollider();
        }

        #endregion

        #region Handle Weapon Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }

}

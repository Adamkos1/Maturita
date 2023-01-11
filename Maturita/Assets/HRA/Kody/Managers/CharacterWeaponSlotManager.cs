using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;
        public WeaponHolderSlot backSlotForShield;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Targets")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
        }

        protected virtual void LoadWeaponHolderSlots()
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

        public virtual void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(characterManager.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterManager.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    characterManager.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (characterManager.isTwoHandingWeapon)
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
                        characterManager.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);

                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                        //backSlotForShield.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHandingWeapon);
                    characterManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    characterManager.characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    characterManager.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterManager.characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    characterManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        protected virtual void LoadLeftWeaponDamgeCollider()
        {
            leftHandDamageCollider =leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            if (characterManager.characterInventoryManager.leftWeapon.isUnarmed)
                return;

            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = characterManager.characterInventoryManager.leftWeapon.baseDamage;

            leftHandDamageCollider.characterManager = characterManager;
            leftHandDamageCollider.teamIDNumber = characterManager.characterStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = characterManager.characterInventoryManager.leftWeapon.poiseBreak;
            characterManager.characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        protected virtual void LoadRightWeaponDamgeCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            if (characterManager.characterInventoryManager.rightWeapon.isUnarmed)
                return;

            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = characterManager.characterInventoryManager.rightWeapon.baseDamage;

            rightHandDamageCollider.characterManager = characterManager;
            rightHandDamageCollider.teamIDNumber = characterManager.characterStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = characterManager.characterInventoryManager.rightWeapon.poiseBreak;
            characterManager.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            characterManager.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
                characterManager.characterEffectsManager.PlayWeaponFX(false);
            }
            else if(characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
                characterManager.characterEffectsManager.PlayWeaponFX(true);
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DissableDamageCollider();
                characterManager.characterEffectsManager.StopWeaponFX(false);
            }

            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DissableDamageCollider();
                characterManager.characterEffectsManager.StopWeaponFX(false);

            }
        }

        public virtual void GrantWeaponAttackPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = characterManager.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.armorPoiseBonus;
        }
    }

}

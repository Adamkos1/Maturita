using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;
        protected CharacterInventoryManager characterInventoryManager;
        protected CharacterAnimatorManager characterAnimatorManager;
        protected CharacterEffectsManager characterEffectsManager;
        protected CharacterStatsManager characterStatsManager;

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
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
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
            LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
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
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
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
                        characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);

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
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamgeCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamgeCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        protected virtual void LoadLeftWeaponDamgeCollider()
        {
            leftHandDamageCollider =leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            if (characterInventoryManager.leftWeapon.isUnarmed)
                return;

            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = characterInventoryManager.leftWeapon.baseDamage;

            leftHandDamageCollider.characterManager = characterManager;
            leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
            characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        protected virtual void LoadRightWeaponDamgeCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            if (characterInventoryManager.rightWeapon.isUnarmed)
                return;

            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = characterInventoryManager.rightWeapon.baseDamage;

            rightHandDamageCollider.characterManager = characterManager;
            rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
            characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
                characterEffectsManager.PlayWeaponFX(false);
            }
            else if(characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
                characterEffectsManager.PlayWeaponFX(true);
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DissableDamageCollider();
                characterEffectsManager.StopWeaponFX(false);
            }

            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DissableDamageCollider();
                characterEffectsManager.StopWeaponFX(false);

            }
        }

        public virtual void GrantWeaponAttackPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterStatsManager.totalPoiseDefence = characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterManager : MonoBehaviour
    {
        public Animator animator;
        public CharacterAnimatorManager characterAnimatorManager;
        public CharacterWeaponSlotManager characterWeaponSlotManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterInventoryManager characterInventoryManager;
        public CharacterEffectsManager characterEffectsManager;
        public CharacterSoundFXManager characterSoundFXManager;
        public CharacterCombatManager characterCombatManager;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("RayCast")]
        public Transform criticalAttackRaycastStartPoint;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isTwoHandingWeapon;
        public bool isAiming;
        public bool isHoldingArrow;
        public bool isPerformingFullyChargedAttack;
        public bool isAttacking;
        public bool isBeingBackStebbed;
        public bool isBeingRiposted;
        public bool isPerformingRiposte;
        public bool isPerformingBackSteb;

        [Header("Interacting")]
        public bool isInteracting;

        [Header("Status")]
        public bool isDead;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isJumping;

        [Header("Spells")]
        public bool isFiringSpell;

        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            animator = GetComponent<Animator>();

        }

        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
        {
            if(usingRightHand)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingLeftHand = true;
                isUsingRightHand = false;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager characterManager;

        [Header("Current FX")]
        public GameObject instantiatedFXModel;

        [Header("Damage FX")]
        public GameObject bloodSplatterFX;

        [Header("Weapon FX")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        [Header("Consumable FX")]
        public GameObject currentParticleFX;
        public GameObject activatedFXModel;
        public int amountToBeHealed;


        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                if(rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }

        public virtual void StopWeaponFX(bool isLeft)
        {
            if (isLeft == false)
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.StopWeaponFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.StopWeaponFX();
                }
            }
        }

        public virtual void PlayBloodSplatterFX(Vector3 bloodSplaterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplaterLocation, Quaternion.identity);
        }

        public virtual void InterruptEffect()
        {
            if(instantiatedFXModel != null)
            {
                Destroy(instantiatedFXModel);
            }
            else
            {
                return;
            }

            //vystreli sip a odstraniho z ruky ak ho drzia
            if(characterManager.isHoldingArrow)
            {
                characterManager.animator.SetBool("isHoldingArrow", false);
                Animator rangedWeaponAnimator = characterManager.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();

                if(rangedWeaponAnimator != null)
                {
                    rangedWeaponAnimator.SetBool("isDrawn", false);
                    rangedWeaponAnimator.Play("Bow_ONLY_Fire_01");
                }
            }

            //prestanes mierit ak mieris
            if(characterManager.isAiming)
            {
                characterManager.animator.SetBool("isAiming", false);
            }
        }

    }

}

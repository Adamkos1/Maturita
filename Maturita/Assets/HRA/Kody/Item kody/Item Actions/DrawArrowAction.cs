using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]

    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;
            if (character.isHoldingArrow)
                return;

            if(character.isTwoHandingWeapon)
            {
                if (character.characterInventoryManager.currentAmmo.currentAmount > 0)
                {
                    //animuje hraca
                    character.characterAnimatorManager.EraseHandIKForWeapon();
                    character.animator.SetBool("isHoldingArrow", true);
                    character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01_R", true);

                    //vytvory sip
                    GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);
                    character.characterEffectsManager.currentRangedFX = loadedArrow;

                    //animuje luk
                    Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                    bowAnimator.SetBool("isDrawn", true);
                    bowAnimator.Play("Bow_ONLY_Draw_01");
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }

}
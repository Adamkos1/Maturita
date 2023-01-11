using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]

    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            if (player.isHoldingArrow)
                return;

            if(player.isTwoHandingWeapon)
            {
                if (player.playerInventoryManager.currentAmmo.currentAmount > 0)
                {
                    //animuje hraca
                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    player.animator.SetBool("isHoldingArrow", true);
                    player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01_R", true);

                    //vytvory sip
                    GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);
                    player.playerEffectsManager.currentRangedFX = loadedArrow;

                    //animuje luk
                    Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                    bowAnimator.SetBool("isDrawn", true);
                    bowAnimator.Play("Bow_ONLY_Draw_01");
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }

}
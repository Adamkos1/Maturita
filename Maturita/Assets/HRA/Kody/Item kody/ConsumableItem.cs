using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class ConsumableItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animation")]
        public string consumeAnimation;
        public bool isInteracting;


        public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if(currentItemAmount > 0)
            {
                if(!isInteracting)
                {
                    playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
                    currentItemAmount -= 1;
                }
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }
    }

}

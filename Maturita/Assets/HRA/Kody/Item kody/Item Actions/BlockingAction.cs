using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Block Action")]

    public class BlockingAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            if (player.isBlocking)
                return;

            if (player.isTwoHandingWeapon)
                return;

            if (player.isHoldingArrow)
                return;

            player.playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            player.playerEquipmentManager.OpenBlockingCollider();
            player.isBlocking = true;
        }
    }

}

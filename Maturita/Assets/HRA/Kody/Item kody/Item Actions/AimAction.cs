using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Aim Action")]

    public class AimAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            player.playerAnimatorManager.EraseHandIKForWeapon();

            if (player.isAiming)
                return;

            if (player.isUsingLeftHand)
                return;

            player.uIManager.crossHair.SetActive(true);
            player.isAiming = true;
        }
    }

}
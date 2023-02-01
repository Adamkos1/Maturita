using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Aim Action")]

    public class AimAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (player.isAiming)
                return;

            if (player.isUsingLeftHand)
                return;

            if(player != null)
            {
                player.playerAnimatorManager.EraseHandIKForWeapon();
                player.uIManager.crossHair.SetActive(true);
            }

            player.isAiming = true;

        }
    }

}
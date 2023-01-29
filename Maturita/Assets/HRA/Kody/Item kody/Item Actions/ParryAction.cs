using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]

    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 25)
                return;

            if (player.isInteracting)
                return;

            player.playerAnimatorManager.EraseHandIKForWeapon();

            WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

            if(parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
            else if (parryingWeapon.weaponType == WeaponType.Shield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }

            player.playerCombatManager.currentAttackType = AttackType.Parry;
        }
    }

}
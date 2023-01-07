using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName ="Item Actions/Light Attack Action")]

    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            //player.playerAnimatorManager.EraseHandIKForWeapon();
            player.playerAnimatorManager.animator.SetBool("isUsingRightHand", true);


            if (player.isSprinting)
            {
                HandleRuningAttack(player.playerInventoryManager.rightWeapon, player);
                return;
            }

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(player.playerInventoryManager.rightWeapon, player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleLightAttack(player.playerInventoryManager.rightWeapon, player);

            }
        }

        private void HandleLightAttack(WeaponItem weapon, PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            player.playerWeaponSlotManager.attackingWeapon = weapon;

            if (player.inputHandler.twoHandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Light_Attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_Light_Attack_01;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_01;
            }
        }

        private void HandleRuningAttack(WeaponItem weapon, PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            player.playerWeaponSlotManager.attackingWeapon = weapon;

            if (player.inputHandler.twoHandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Runing_Attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_Runing_Attack_01;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Runing_Attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Runing_Attack_01;
            }
        }

        private void HandleLightWeaponCombo(WeaponItem weapon, PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.inputHandler.comboFlag)
            {
                player.playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if(player.isTwoHandingWeapon)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_Light_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Light_Attack_02, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.th_Light_Attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Light_Attack_01, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.th_Light_Attack_01;
                    }
                }
                else
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Light_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_02, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_01, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_01;
                    }
                }
            }
        }
    }

}
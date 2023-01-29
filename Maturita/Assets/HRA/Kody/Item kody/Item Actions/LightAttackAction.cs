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
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.isSprinting)
            {
                HandleRuningAttack(player);
                return;
            }

            if (player.isBlocking)
            {
                player.inputHandler.hold_LB_Input = false;
                HandleLightAttack(player);
                return;
            }

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleLightAttack(player);

            }

            player.playerCombatManager.currentAttackType = AttackType.Light;
        }

        private void HandleLightAttack(PlayerManager player)
        {
            if(player.isUsingLeftHand)
            {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_01, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_01;
            }

            else if(player.isUsingRightHand)
            {
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
        }

        private void HandleRuningAttack(PlayerManager player)
        {
            if(player.isUsingLeftHand)
            {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Runing_Attack_01, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Runing_Attack_01;
            }

            else if(player.isUsingRightHand)
            {
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
        }

        private void HandleLightWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Light_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_02, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Light_Attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Light_Attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
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

}
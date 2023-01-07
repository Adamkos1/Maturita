using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]

    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            //player.playerAnimatorManager.EraseHandIKForWeapon();
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.isSprinting)
            {
                HandleJumpingAttack(player);
                return;
            }

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleHeavyAttack(player);

            }
        }


        private void HandleHeavyAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
            }

            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                }
            }
        }

        private void HandleJumpingAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Jumping_Attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Jumping_Attack_01;
            }

            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Jumping_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_Jumping_Attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Jumping_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Jumping_Attack_01;
                }
            }
        }

        private void HandleHeavyWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Heavy_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_Heavy_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_01;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_01;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Heavy_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                        }
                    }
                }
            }
        }


    }

}

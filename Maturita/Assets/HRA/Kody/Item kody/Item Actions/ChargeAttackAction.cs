using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]

    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            //player.playerAnimatorManager.EraseHandIKForWeapon();
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            Debug.Log("kokot");

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleChargeWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleChargeAttack(player);

            }
        }


        private void HandleChargeAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
            }

            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Charge_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_Charge_Attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
                }
            }
        }

        private void HandleChargeWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Charge_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_Charge_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Charge_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Charge_Attack_01;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Charge_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Charge_Attack_01;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Charge_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Charge_Attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Charge_Attack_01;
                        }
                    }
                }
            }
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]

    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            //player.playerAnimatorManager.EraseHandIKForWeapon();
            character.isAttacking = true;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            if (character.isSprinting)
            {
                HandleJumpingAttack(character);
                return;
            }

            if (character.isBlocking)
            {
                if(player != null)
                {
                    player.inputHandler.hold_LB_Input = false;
                }
                HandleHeavyAttack(character);
                return;
            }

            if (character.canDoCombo)
            {
                HandleHeavyWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleHeavyAttack(character);

            }

            character.characterCombatManager.currentAttackType = AttackType.Heavy;

        }


        private void HandleHeavyAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
            }

            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Heavy_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Heavy_Attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
                }
            }
        }

        private void HandleJumpingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Jumping_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Jumping_Attack_01;
            }

            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Jumping_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Jumping_Attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Jumping_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Jumping_Attack_01;
                }
            }
        }

        private void HandleHeavyWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Heavy_Attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_Heavy_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Heavy_Attack_01;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Heavy_Attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Heavy_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_01;
                        }
                    }
                }
            }
        }


    }

}

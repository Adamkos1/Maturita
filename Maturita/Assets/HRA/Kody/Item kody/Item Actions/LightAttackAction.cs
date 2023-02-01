using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName ="Item Actions/Light Attack Action")]

    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            //player.playerAnimatorManager.EraseHandIKForWeapon();
            if (character.characterStatsManager.currentStamina <= 0)
                return;

            if (character.isSprinting)
            {
                HandleRuningAttack(character);
                return;
            }

            if (character.isBlocking)
            {
                if (player != null)
                {
                    player.inputHandler.hold_LB_Input = false;
                }
                HandleLightAttack(character);
                return;
            }

            if (character.canDoCombo)
            {
                HandleLightWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleLightAttack(character);

            }

            character.characterCombatManager.currentAttackType = AttackType.Light;
        }

        private void HandleLightAttack(CharacterManager character)
        {
            if(character.isUsingLeftHand)
            {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_01;
            }

            else if(character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Light_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Light_Attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_01;
                }
            }
        }

        private void HandleRuningAttack(CharacterManager character)
        {
            if(character.isUsingLeftHand)
            {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Runing_Attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Runing_Attack_01;
            }

            else if(character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Runing_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Runing_Attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Runing_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Runing_Attack_01;
                }
            }
        }

        private void HandleLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_02, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_02;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_01;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_Light_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Light_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Light_Attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Light_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Light_Attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Light_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_01;
                        }
                    }
                }
            }
        }
    }

}
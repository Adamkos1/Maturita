using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]

    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            //player.playerAnimatorManager.EraseHandIKForWeapon();
            if (character.characterStatsManager.currentStamina <= 0)
                return;

            Debug.Log("kokot");

            if (character.canDoCombo)
            {
                HandleChargeWeaponCombo(character);
                character.canDoCombo = false;
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleChargeAttack(character);

            }
        }


        private void HandleChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
            }

            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Charge_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Charge_Attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
                }
            }
        }

        private void HandleChargeWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Charge_Attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_Charge_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Charge_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Charge_Attack_01;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_Charge_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_Charge_Attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Charge_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_Charge_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Charge_Attack_01;
                        }
                    }
                }
            }
        }


    }

}

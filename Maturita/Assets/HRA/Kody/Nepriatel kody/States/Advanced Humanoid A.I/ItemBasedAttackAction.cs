
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName ="A.I/Humanoid/ Item Based Attack Actions")]

    public class ItemBasedAttackAction : ScriptableObject
    {
        [Header("Attack Typer")]
        public AIAttackActionType aIAttackActionType = AIAttackActionType.melleeAttackAction;
        public AttackType attackType = AttackType.Light;

        [Header("Action Combo Settings")]
        public bool ActionCanCombo = false;

        [Header("Right Hand or Left hand action")]
        public bool isRightHandedAction = true;

        [Header("Action Settings")]
        public int attackScore = 3;
        public float recoveryTime = 3;
        public float maximumAttackAngle = 80;
        public float minimumAttackAngle = -80;
        public float minimumDistanceNeededToAttack = -5;
        public float maximumDistanceNeededToAttack = 5;

        public void PerformAttackAction(EnemyManager enemyManager)
        {
            if(isRightHandedAction)
            {
                enemyManager.UpdateWhichHandCharacterIsUsing(true);
                PerformRightHandItemActionBasedOnAttackType(enemyManager);
            }
            else
            {
                enemyManager.UpdateWhichHandCharacterIsUsing(false);
                PerformLeftHandItemActionBasedOnAttackType(enemyManager);
            }
        }

        private void PerformRightHandItemActionBasedOnAttackType(EnemyManager enemyManager)
        {
            if(aIAttackActionType == AIAttackActionType.melleeAttackAction)
            {
                PerformRighHandedMeleeAction(enemyManager);
            }
            else if(aIAttackActionType != AIAttackActionType.rangedAttackAattack)
            {

            }
        }

        private void PerformLeftHandItemActionBasedOnAttackType(EnemyManager enemyManager)
        {
            if (aIAttackActionType == AIAttackActionType.melleeAttackAction)
            {

            }
            else if (aIAttackActionType != AIAttackActionType.rangedAttackAattack)
            {

            }
        }

        private void PerformRighHandedMeleeAction(EnemyManager enemyManager)
        {
            if(enemyManager.isTwoHandingWeapon)
            {
                if(attackType == AttackType.Light)
                {
                    enemyManager.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemyManager);
                }
                else if(attackType == AttackType.Heavy)
                {
                    enemyManager.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(enemyManager);
                }
            }
            else
            {
                if (attackType == AttackType.Light)
                {
                    enemyManager.characterInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(enemyManager);
                }
                else if (attackType == AttackType.Heavy)
                {
                    enemyManager.characterInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(enemyManager);
                }
            }
        }

    }

}

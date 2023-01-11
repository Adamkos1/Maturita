using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class AttackState : State
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public EnemyAttackAction currentAttack;
        public PursueTargetState pursueTargetState;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhilistAttacking(enemyManager);

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyManager);
            }

            if(!hasPerformedAttack)
            {
                AttackTarget(enemyManager);
                RollForComboChance(enemyManager);
            }

            if(willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;


        }

        private void AttackTarget(EnemyManager enemyManager)
        {
            enemyManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemyManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            enemyManager.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyManager enemyManager)
        {
            enemyManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemyManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            willDoComboOnNextAttack = false;
            enemyManager.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        public void RotateTowardsTargetWhilistAttacking(EnemyManager enemyManager)
        {
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if(currentAttack.comoboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comoboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }

}

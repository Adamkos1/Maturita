using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class AttackStateHumanoid : State
    {
        public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
        public CombatStanceStateHumanoid combatStanceState;
        public ItemBasedAttackAction currentAttack;
        public PursueTargetStateHumanoid pursueTargetState;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        private void Awake()
        {
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();
            pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }

        public override State Tick(EnemyManager enemyManager)
        {
            if(enemyManager.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemyManager);
            }
            else if(enemyManager.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(enemyManager);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemyManager)
        {
            RotateTowardsTargetWhilistAttacking(enemyManager);

            if (enemyManager.distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyManager);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyManager);
                RollForComboChance(enemyManager);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                ResetStateFlags(); // TOTO tu asi zmenit
                return this;
            }

            ResetStateFlags();
            return rotateTowardsTargetState;


        }

        private State ProcessArcherCombatStyle(EnemyManager enemyManager)
        {
            RotateTowardsTargetWhilistAttacking(enemyManager);

            if (enemyManager.isInteracting)
                return this;

            if(!enemyManager.isHoldingArrow)
            {
                ResetStateFlags();
                return combatStanceState;
            }

            if (enemyManager.currentTarget.isDead)
            {
                //enemyManager.currentTarget = null;
                ResetStateFlags();
                return pursueTargetState;
            }

            if (enemyManager.distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            if (!hasPerformedAttack)
            {
                FireAmmo(enemyManager);
            }

            ResetStateFlags();
            return rotateTowardsTargetState; ;
        }

        private void AttackTarget(EnemyManager enemyManager)
        {
            currentAttack.PerformAttackAction(enemyManager);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyManager enemyManager)
        {
            currentAttack.PerformAttackAction(enemyManager);
            willDoComboOnNextAttack = false;
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

            if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if (currentAttack.ActionCanCombo)
                {
                    willDoComboOnNextAttack = true;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }

        private void ResetStateFlags()
        {
            willDoComboOnNextAttack = false;
            hasPerformedAttack = false;
    }

        private void FireAmmo(EnemyManager enemyManager)
        {
            if(enemyManager.isHoldingArrow)
            {
                hasPerformedAttack = true;
                enemyManager.characterInventoryManager.currentItemBeingUsed = enemyManager.characterInventoryManager.rightWeapon;
                enemyManager.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemyManager);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class AttackState : State
    {
        public CombatStanceState combatStanceState;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        bool willDoComboOnnextAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isInteracting && enemyManager.canDoCombo == false)
            {
                return this;
            }
            else if (enemyManager.isInteracting && enemyManager.canDoCombo)
            {
                if (willDoComboOnnextAttack)
                {
                    willDoComboOnnextAttack = false;
                    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                }
            }


            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }

                if (currentAttack != null)
                {
                //ak sme prilis blizko nepriatela aby zautocil tak dostane novy utok
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                    {
                    }
                    // ak je dost blizko tak spravy =
                    else if(distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                    {
                    //ak je nepriatel vo vzdialenosti na to aby zautocil tak zauttoci
                    if (viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                        {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                            {
                                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                                enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                                enemyManager.isPerformingAction = true;
                                RollForComboChance(enemyManager);

                                if(currentAttack.canCombo && willDoComboOnnextAttack)
                                {
                                    currentAttack = currentAttack.comoboAction;
                                    return this;
                                }
                                else
                                {
                                    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                    currentAttack = null;
                                    return combatStanceState;
                                }
                            }
                        }
                    }
                }
                else
                {
                    GetNewAttack(enemyManager);
                }

            return combatStanceState;
                
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
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
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                willDoComboOnnextAttack = true;
            }
        }
    }

}

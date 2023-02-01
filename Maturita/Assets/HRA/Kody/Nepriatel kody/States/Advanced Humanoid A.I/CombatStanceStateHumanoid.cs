using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public PursueTargetStateHumanoid pursueTargetState;
        public ItemBasedAttackAction[] enemyAttacks;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;

        protected float horizontalMovementValue = 0;

        [Header("State Flags")]
        bool willPerformBlock = false;
        bool willPerformDodge = false;
        bool willPerformParry = false;

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
            enemyManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            //ak ai pada alebo nieco roby zastavi kazdy pohyb
            if (!enemyManager.isGrounded || enemyManager.isInteracting)
            {
                enemyManager.animator.SetFloat("Vertical", 0);
                enemyManager.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //ak je AI daleko od hraca tak sa vrati do pursue tafget state
            if (enemyManager.distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            //kruzenie okolo hraca je randomizovane
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyManager.enemyAnimatorManager);
            }

            if(enemyManager.allowAIToPerformBlock)
            {
                RollForBlockChance(enemyManager);
            }

            if (enemyManager.allowAIToPerformDodge)
            {
                RollForBlockChance(enemyManager);
            }

            if (enemyManager.allowAIToPerformParry)
            {
                RollForBlockChance(enemyManager);
            }


            if (willPerformBlock)
            {

            }


            if (willPerformDodge)
            {

            }


            if (willPerformParry)
            {

            }

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return this;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemyManager)
        {
            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
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

        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroundTarget(enemyAnimatorManager);
        }

        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            verticalMovementValue = 0f;

            horizontalMovementValue = Random.Range(-1f, 1f);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemyManager.viewableAngle <= enemyAttackAction.maximumAttackAngle && enemyManager.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemyManager.viewableAngle <= enemyAttackAction.maximumAttackAngle && enemyManager.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        private void RollForBlockChance(EnemyManager enemyManager)
        {
            int blockChance = Random.Range(0, 100);

            if(blockChance <= enemyManager.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollForBlockDodge(EnemyManager enemyManager)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= enemyManager.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }


        private void RollForBlockParry(EnemyManager enemyManager)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= enemyManager.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }


        private void ResetStateFlags()
        {
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
        }
    }

}

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

        public bool hasPerformedParry = false;
        bool hasPerformedDodge = false;
        bool hasRandomDodgeDirection = false;
        public bool hasAmmoLoaded = false;

        Quaternion targetDodgeDirection;

        private void Awake()
        {
            attackState = GetComponent<AttackStateHumanoid>();
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

            if(enemyManager.allowAIToPerformParry)
            {
                if(enemyManager.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(enemyManager);
                    return this;
                }
            }



           // if(enemyManager.allowAIToPerformBlock)
           // {
            //    RollForBlockChance(enemyManager);
           // }

            if (enemyManager.allowAIToPerformDodge)
            {
                RollForBlockDodge(enemyManager);
            }

            if (enemyManager.allowAIToPerformParry)
            {
                RollForBlockParry(enemyManager);
            }





            if (enemyManager.currentTarget.isAttacking)
            {
                if (willPerformParry && !hasPerformedParry)
                {
                    ParryCurentTarget(enemyManager);
                    return this;
                }
            }

            // if (willPerformBlock)
            //  {
            //     BlockUsingOffHand(enemyManager);
            // }


            if (willPerformDodge && enemyManager.currentTarget.isAttacking)
            {
                Dodge(enemyManager);
            }





            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                ResetStateFlags();
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            HandleMovement(enemyManager);

            return this;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemyManager)
        {
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
                ResetStateFlags();
                return pursueTargetState;
            }

            //kruzenie okolo hraca je randomizovane
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyManager.enemyAnimatorManager);
            }

            if (enemyManager.allowAIToPerformDodge)
            {
                RollForBlockDodge(enemyManager);
            }

            if (willPerformDodge && enemyManager.currentTarget.isAttacking)
            {
                Dodge(enemyManager);
            }

            HandleRotateTowardsTarget(enemyManager);

            if(!hasAmmoLoaded)
            {
                DrawArrow(enemyManager);
                AimAtTargetBeforeFiring(enemyManager);
            }

            if (enemyManager.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

            if(enemyManager.isStationaryArcher)
            {
                enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemyManager.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            }
            else
            {
                HandleMovement(enemyManager);
            }

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
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            hasPerformedParry = false;
            randomDestinationSet = false;
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
            hasAmmoLoaded = false;
        }

        private void BlockUsingOffHand(EnemyManager enemyManager)
        {
            if(enemyManager.isBlocking == false)
            {
                if(enemyManager.allowAIToPerformBlock)
                {
                    enemyManager.isBlocking = true;
                    enemyManager.characterInventoryManager.currentItemBeingUsed = enemyManager.characterInventoryManager.leftWeapon;
                    enemyManager.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }

        private void Dodge(EnemyManager enemyManager)
        {
            if(!hasPerformedDodge)
            {
                if(!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(enemyManager.transform.eulerAngles.x, randomDodgeDirection, enemyManager.transform.eulerAngles.z);
                }

                if(enemyManager.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(enemyManager.transform.rotation, targetDodgeDirection, 1f);
                    enemyManager.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemyManager.transform.eulerAngles.y;
                    float rotationDiffrence = Mathf.Abs(targetYRotation - currentYRotation);

                    if(rotationDiffrence <= 5)
                    {
                        hasPerformedDodge = true;
                        enemyManager.transform.rotation = targetDodgeDirection;
                        enemyManager.enemyAnimatorManager.PlayTargetAnimation("Rolling", true);
                    }

                }
            }
        }

        private void DrawArrow(EnemyManager enemyManager)
        {
            if(!enemyManager.isTwoHandingWeapon)
            {
                enemyManager.isTwoHandingWeapon = true;
                enemyManager.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
            }
            else
            {
                hasAmmoLoaded = true;
                enemyManager.characterInventoryManager.currentItemBeingUsed = enemyManager.characterInventoryManager.rightWeapon;
                enemyManager.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(enemyManager);
            }
        }

        private void AimAtTargetBeforeFiring(EnemyManager enemyManager)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range(enemyManager.minimumTimeToAimAtTarget, enemyManager.maximumTimeToAimAtTarget);
            enemyManager.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        private void ParryCurentTarget(EnemyManager enemyManager)
        {
            if(enemyManager.currentTarget.canBeParried)
            {
                if(enemyManager.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    enemyManager.isParrying = true;
                    enemyManager.enemyAnimatorManager.PlayTargetAnimation("Parry_01", true);
                }
            }
        }

        private void CheckForRiposte(EnemyManager enemyManager)
        {
            if(enemyManager.isInteracting)
            {
                enemyManager.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;

            }

            if(enemyManager.distanceFromTarget >= 1.5)
            {
                enemyManager.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                HandleRotateTowardsTarget(enemyManager);
            }
            else
            {
                enemyManager.isBlocking = false;

                if(!enemyManager.isInteracting && !enemyManager.currentTarget.isBeingRiposted && !enemyManager.currentTarget.isBeingBackStebbed)
                {
                    enemyManager.enemyRigidBody.velocity = Vector3.zero;
                    enemyManager.animator.SetFloat("Vertical", 0);
                    enemyManager.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        private void HandleMovement(EnemyManager enemyManager)
        {
            if (enemyManager.distanceFromTarget <= enemyManager.stoppingDistance)
            {
                enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemyManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                enemyManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                enemyManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }

        }
    }

}

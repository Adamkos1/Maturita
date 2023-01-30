using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PursueTargetStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceStateHumanoid;

        public override State Tick(EnemyManager enemyManager)
        {
            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isInteracting)
                return this;

            if (enemyManager.isPerformingAction)
            {
                enemyManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (enemyManager.distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceStateHumanoid;
            }
            else
            {
                return this;
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
    }

}

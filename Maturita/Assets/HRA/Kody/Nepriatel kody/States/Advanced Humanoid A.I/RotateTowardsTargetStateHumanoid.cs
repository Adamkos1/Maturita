using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class RotateTowardsTargetStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        }

        public override State Tick(EnemyManager enemyManager)
        {
            enemyManager.animator.SetFloat("Vertical", 0);
            enemyManager.animator.SetFloat("Horizontal", 0);

            if (enemyManager.isInteracting)
            {
                return this;
            }

            if (enemyManager.viewableAngle >= 100 && enemyManager.viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle <= -101 && enemyManager.viewableAngle >= -180 && !enemyManager.isInteracting)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle <= -45 && enemyManager.viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle >= 45 && enemyManager.viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}

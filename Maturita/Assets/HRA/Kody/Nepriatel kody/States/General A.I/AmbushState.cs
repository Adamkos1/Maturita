using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager)
        {
            if(isSleeping && enemyManager.isInteracting == false)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager potentialTarget = colliders[i].transform.GetComponent<CharacterManager>();

                if(potentialTarget != null)
                {
                    Vector3 targetsDirection = potentialTarget.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                    if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle || enemyManager.enemyStatsManager.currentHealth < enemyManager.enemyStatsManager.maxHealth)
                    {
                        enemyManager.currentTarget = potentialTarget;
                        isSleeping = false;
                        enemyManager.enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }

            #endregion

            #region Handle State Change

            if(enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }

            #endregion
        }
    }

}

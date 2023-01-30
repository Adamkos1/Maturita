using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public override State Tick(EnemyManager enemyManager)
        {
            #region Handle Enemy Target Detection
            //hlada target v urcitom radiuse
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //ak najde target ktory nie je v tom istom tyme ako on tak ide na dalsi krok
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != enemyManager.enemyStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        //musi stat pred  nim
                        if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                        {
                            if (Physics.Linecast(enemyManager.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                            {
                                return this;
                            }
                            else
                            {
                                enemyManager.currentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }

            #endregion

            #region Handle Switching To Next State
            if (enemyManager.currentTarget != null)
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

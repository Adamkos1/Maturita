using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class IdleStateHumanoid : State
    {
        public PursueTargetStateHumanoid pursueTargetStateHumanoid;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        private void Awake()
        {
            pursueTargetStateHumanoid = GetComponent<PursueTargetStateHumanoid>();
        }

        public override State Tick(EnemyManager enemyManager)
        {
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
                            //ak je medzi ai a targetom nejaka prekazka tak nebude ziaden current target
                            if(Physics.Linecast(enemyManager.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
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

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerManager potentialTarget = colliders[i].transform.GetComponent<PlayerManager>();

                if (potentialTarget != null)
                {
                    if (enemyManager.enemyStatsManager.currentHealth < enemyManager.enemyStatsManager.maxHealth)
                    {
                        enemyManager.currentTarget = potentialTarget;
                    }
                }
            }


            if (enemyManager.currentTarget != null)
            {
                return pursueTargetStateHumanoid;
            }
            else
            {
                return this;
            }

        }
    }

}
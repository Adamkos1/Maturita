using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PatrolStateHumanoid : State
    {
        public PursueTargetStateHumanoid pursueTargetStateHumanoid;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public bool patrolComplete;
        public bool repeatPatrol;

        [Header("Patrol Rest Time")]
        public float endOfPatrolRestTime;
        public float endOfPatrolTimer;

        [Header("Patrol Position")]
        public bool hasPatrolDestination;
        public int patrolDestinationIndex;
        public Transform currentPatrolDestination;
        public float distanceFromCurrentPatrolPoint;
        public List<Transform> listOfPatrolDestinations = new List<Transform>();

        public override State Tick(EnemyManager aiCharacter)
        {
            SearchForTargetWhilstPatroling(aiCharacter);

            //ak A.I nieco robi tak zastavi
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if(aiCharacter.currentTarget != null)
            {
                return pursueTargetStateHumanoid;
            }

            //ak dokoncil hliadku a chceme aby ju opakoval tak ju urobi
            if(patrolComplete && repeatPatrol)
            {
                //odratame si cas na Rest a resetujeme vsetky flags
                if(endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                    endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                    return this;
                }
                else if(endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if(patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return this;
            }

            if (hasPatrolDestination)
            {
                if(currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                    if(distanceFromCurrentPatrolPoint > 1)
                    {
                        aiCharacter.navMeshAgent.enabled = true;
                        aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }  

            if(!hasPatrolDestination)
            {
                patrolDestinationIndex = patrolDestinationIndex + 1;

                if(patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
                hasPatrolDestination = true;
            }
            return this;
        }

        private void SearchForTargetWhilstPatroling(EnemyManager enemyManager)
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
                            if (Physics.Linecast(enemyManager.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                            {
                                return;
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
                return;
            }
            else
            {
                return;
            }

        }
    }

}

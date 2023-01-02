using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AH
{

    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStatsManager enemyStatsManager;
        EnemyBossManager enemyBossManager;
        BossCombatStanceState bossCombatStanceState;

        public NavMeshAgent navMeshAgent;
        public State currentState;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidBody;
        public Collider backStabboxCollider;
        public Collider parryCollider;

        public bool isPerformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 2.5f;
        public float timeUntilDestroyed = 3;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //proste cim vysi rozsah tym viac vidia a opacne
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;
        public bool isPhaseShifting;
        


        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
            enemyRigidBody = GetComponent<Rigidbody>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);

            if (enemyStatsManager.isDead)
            {
                Destroy(backStabboxCollider);
                Destroy(parryCollider);
                Destroy(gameObject, timeUntilDestroyed);
            }
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (enemyStatsManager.isDead)
            {
                SwitchToNextState(null);
                currentTarget = null;
            }

            else if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        public void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}

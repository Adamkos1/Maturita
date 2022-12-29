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
        EnemyStats enemyStats;
        EnemyBossManager enemyBossManager;
        BossCombatStanceState bossCombatStanceState;

        public NavMeshAgent navMeshAgent;
        public State currentState;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidBody;
        public Collider backStabboxCollider;
        public Collider parryCollider;


        public bool isPerformingAction;
        public bool isInteracting;
        public bool isGrounded;
        public bool isInAir;

        public float rotationSpeed = 15;

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //proste cim vysi rozsah tym viac vidia a opacne
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float maximumAggroRadius = 2.5f;
        public float currentRecoveryTime = 0;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;
        public bool isPhaseShifting;
        


        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
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
            enemyAnimatorManager.animator.SetBool("isDead", enemyStats.isDead);

            if (enemyStats.isDead)
            {
                Destroy(backStabboxCollider);
                Destroy(parryCollider);
            }
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (enemyStats.isDead)
            {
                SwitchToNextState(null);
                currentTarget = null;
            }

            else if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

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

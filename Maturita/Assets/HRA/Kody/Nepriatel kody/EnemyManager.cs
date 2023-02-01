using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AH
{

    public class EnemyManager : CharacterManager
    {
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        public EnemyBossManager enemyBossManager;
        public EnemyEffectsManager enemyEffectsManager;
        public BossCombatStanceState bossCombatStanceState;

        public NavMeshAgent navMeshAgent;
        public State currentState;
        public CharacterManager currentTarget;
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
        public AICombatStyle combatStyle;

        [Header("Advanced AI Settings")]
        public bool allowAIToPerformBlock;
        public bool allowAIToPerformDodge;
        public bool allowAIToPerformParry;
        public int blockLikelyHood = 50;        //percento na blokonutie
        public int dodgeLikelyHood = 50;
        public int parryLikelyHood = 50;


        [Header("AI Target Settings")]
        public float distanceFromTarget;
        public Vector3 targetsDirection;
        public float viewableAngle;


        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
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

            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            animator.SetBool("isDead", isDead);

            if(currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            }

            if (isDead)
            {
                Destroy(backStabboxCollider);
                Destroy(parryCollider);
                Destroy(gameObject, timeUntilDestroyed);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (isDead)
            {
                SwitchToNextState(null);
                currentTarget = null;
            }

            else if (currentState != null)
            {
                State nextState = currentState.Tick(this);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerLocomotionManager : MonoBehaviour
    {
        PlayerManager playerManager;

        public Vector3 moveDirection;
        public LayerMask groundLayer;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPiont = 0.6f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 0.75f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
            float movementSpeed = 5;
        [SerializeField]
            float walkingSpeed = 2;
        [SerializeField]
            float sprintSpeed = 7;
        [SerializeField]
            float rotationSpeed = 10;
        [HideInInspector]
        float fallingSpeed = 400;

        [Header("Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        int backtepStaminaCost = 12;
        int sprintStaminaCost = 1;
        int jumpStaminaCost = 15;

        Vector3 normalVector;
        Vector3 targetPosition;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        public void HandleRotation()
        {
            if (playerManager.canRotate)
            {
                if(playerManager.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, playerManager.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotation;

                }
                else
                {
                    if (playerManager.inputHandler.lockOnFlag && playerManager.cameraHandler.currentLockOnTarget != null)
                    {
                        if (playerManager.inputHandler.sprintFlag || playerManager.inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = playerManager.cameraHandler.cameraTransform.forward * playerManager.inputHandler.vertical;
                            targetDirection += playerManager.cameraHandler.cameraTransform.right * playerManager.inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }

                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = playerManager.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                    else if (playerManager.cameraHandler.currentLockOnTarget == null)
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = playerManager.inputHandler.moveAmount;

                        targetDir = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                        targetDir += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                        {
                            targetDir = playerManager.transform.forward;
                        }

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, rs * Time.deltaTime);

                        playerManager.transform.rotation = targetRotation;
                    }
                }
            }
        }

        public void HandleMovement()
        {
            if (playerManager.inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
            moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if(playerManager.inputHandler.sprintFlag && playerManager.inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerManager.playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if(playerManager.inputHandler.moveAmount <= 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;

                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if(playerManager.inputHandler.lockOnFlag && playerManager.inputHandler.sprintFlag == false)
            {
                playerManager.playerAnimatorManager.UpdateAnimatorValues(playerManager.inputHandler.vertical, playerManager.inputHandler.horizontal, playerManager.isSprinting);

            }
            else
            {
                playerManager.playerAnimatorManager.UpdateAnimatorValues(playerManager.inputHandler.moveAmount, 0, playerManager.isSprinting);

            }
        }

        public void HandleRollingAndSprinting()
        {
            if (playerManager.animator.GetBool("isInteracting"))
                return;

            if (playerManager.playerStatsManager.currentStamina <= 15)
                return;
            
            if (playerManager.inputHandler.rollFlag)
            {
                playerManager.inputHandler.rollFlag = false;

                moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;

                //if (inputHandler.moveAmount > 0)
                //{
                    playerManager.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                    playerManager.playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    playerManager.transform.rotation = rollRotation;
                    playerManager.playerStatsManager.TakeStaminaDamage(rollStaminaCost);
                // }

                // else
                // {
                //    animatorHandler.PlayTargetAnimation("Backstep", true);
                //    playerAnimatorHandler.EraseHandIKForWeapon();
                //    playerStats.TakeStaminaDamage(backtepStaminaCost); ;
                //}
            }
        }

        public void HandleFalling(Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = playerManager.transform.position;
            Vector3 targetPosition;
            origin.y += groundDetectionRayStartPiont;

            if(Physics.Raycast(origin, playerManager.transform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);

            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = playerManager.transform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

            if (Physics.SphereCast(origin, 0.2f, -Vector3.up, out hit, groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.3f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        playerManager.playerAnimatorManager.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerManager.playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }

            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if(playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        playerManager.playerAnimatorManager.PlayTargetAnimation("Falling", true);
                    }

                

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || playerManager.inputHandler.moveAmount > 0)
                {
                    playerManager.transform.position = Vector3.Lerp(playerManager.transform.position, targetPosition, Time.deltaTime / 0.1f);
                }

                else
                {
                    playerManager.transform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.playerStatsManager.currentStamina <= 15)
                return;

            if (playerManager.inputHandler.jump_Input)
            {
                if(playerManager.inputHandler.moveAmount > 0)
                {
                    moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                    moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;
                    playerManager.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    playerManager.playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    //myTransform.rotation = jumpRotation;
                    playerManager.playerStatsManager.TakeStaminaDamage(jumpStaminaCost);
                }
            }
        }

    }

}
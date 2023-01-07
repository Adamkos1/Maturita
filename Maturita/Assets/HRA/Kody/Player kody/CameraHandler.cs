using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;                   //transform ktorym camara folowuje hraca
        public Transform targetTransformWhileAiming;        //transform ktorym camara folowuje hraca ked miery
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public Camera cameraObject;

        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;


        public static CameraHandler singleton;

        public float leftAndRightAngleLookSpeed = 250;
        public float leftAndRightAimingSpeed = 75;
        public float upAndDownAimingSpeed = 40;
        public float followSpeed = 0.1f;
        public float upAndDownAngleLookSpeed = 100;

        private float targetPosition;
        private float defaultPosition;

        private float leftAndRightAngle;
        private float upAndDownAngle;

        private float minimumLookDownAngle = -35;
        private float maximumLookUpAngle = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;


        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOnDistance = 30;

        private void Awake()
        {
            singleton = this;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 7 |1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            cameraObject = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }

        //folowuj hraca
        public void FollowTarget()
        {
            if(playerManager.isAiming)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }

            HandleCameraCollsions();
        }


        //otacanie kamery
        public void HandleCameraRotation()
        {
            if(inputHandler.lockOnFlag && currentLockOnTarget != null)
            {
                HandleLockedCameraRotation();
            }
            else if(playerManager.isAiming)
            {
                HandleAimedCameraRotation();
            }
            else
            {
                ClearLockOnTargets();
                inputHandler.lockOnFlag = false;
                HandleStandardCameraRotation();
            }
        }

        public void HandleStandardCameraRotation()
        {
            leftAndRightAngle += inputHandler.mouseX * leftAndRightAngleLookSpeed * Time.deltaTime;
            upAndDownAngle -= inputHandler.mouseY * upAndDownAngleLookSpeed * Time.deltaTime;
            upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookDownAngle, maximumLookUpAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = upAndDownAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        public void HandleLockedCameraRotation()
        {
            if(currentLockOnTarget != null)
            {
                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleAimedCameraRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

            Quaternion targetRotationX;
            Quaternion targetRotationY;

            Vector3 cameraRotationX = Vector3.zero;
            Vector3 cameraRotationY = Vector3.zero;

            leftAndRightAngle += (inputHandler.mouseX * leftAndRightAimingSpeed) * Time.deltaTime;
            upAndDownAngle -= (inputHandler.mouseY * leftAndRightAimingSpeed) * Time.deltaTime;

            cameraRotationY.y = leftAndRightAngle;
            targetRotationY = Quaternion.Euler(cameraRotationY);
            targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
            transform.localRotation = targetRotationY;

            cameraRotationX.x = upAndDownAngle;
            targetRotationX = Quaternion.Euler(cameraRotationX);
            targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
            cameraTransform.localRotation = targetRotationX;
        }

        //handle kolizie
        private void HandleCameraCollsions()
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction , out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet);
            }

            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;

            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }


        //handle zameranie sa na nepriatela
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceLeftTarget = -Mathf.Infinity;
            float shortestDistanceRightTarget = Mathf.Infinity;


            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if(character != null)
                {
                    Vector3 lockTragetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTragetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if(character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                             if(hit.transform.gameObject.layer == enviromentLayer)
                             {
                                //cannot lock on
                             }
                             else
                             {
                                availableTargets.Add(character);
                             }
                        }
                    }
                }
            }

            for (int j = 0; j < availableTargets.Count; j++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[j].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[j];
                }

                if (inputHandler.lockOnFlag)
                {

                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[j].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;


                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceLeftTarget && availableTargets[j] != currentLockOnTarget)
                    {
                        shortestDistanceLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[j];
                    }

                    else if(relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceRightTarget && availableTargets[j] != currentLockOnTarget)
                    {
                        shortestDistanceRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[j];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }


    }
}
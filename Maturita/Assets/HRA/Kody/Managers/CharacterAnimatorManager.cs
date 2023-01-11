using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace AH
{

    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager characterManager;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        public bool handIKWeightsReset = false;


        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
        {
            characterManager.animator.applyRootMotion = isInteracting;
            characterManager.animator.SetBool("canRotate", canRotate);
            characterManager.animator.SetBool("isInteracting", isInteracting);
            characterManager.animator.SetBool("isMirrored", mirrorAnim);
            characterManager.animator.CrossFade(targetAnim, 0.2f);

        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            characterManager.animator.applyRootMotion = isInteracting;
            characterManager.animator.SetBool("isRotatingWithRootMotion", true);
            characterManager.animator.SetBool("isInteracting", isInteracting);
            characterManager.animator.CrossFade(targetAnim, 0.2f);
        }

        public virtual void TakeCriticalDamgeAnimationEvent()
        {
            characterManager.characterStatsManager.TakeDamgeNoAnimation(characterManager.pendingCriticalDamage);
            characterManager.pendingCriticalDamage = 0;
        }

        public virtual void CanRotate()
        {
            characterManager.animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            characterManager.animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            characterManager.animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            characterManager.animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            characterManager.animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            characterManager.animator.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeRiposted = true;
        }

        public virtual void DisableCanBeRiposted()
        {
            characterManager.canBeRiposted = false;
        }

        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
        {
            if (isTwoHandingWeapon)
            {
                if(rightHandTarget != null)
                {
                    rightHandConstraint.data.target = rightHandTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if(leftHandTarget != null)
                {
                    leftHandConstraint.data.target = leftHandTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }

            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }

        public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
        {
            if (characterManager.isInteracting)
                return;

            if(handIKWeightsReset)
            {
                handIKWeightsReset = false;

                if(rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }

                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            handIKWeightsReset = true;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }
            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0;
            }

        }
    }

}

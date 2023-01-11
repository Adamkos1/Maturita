using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager playerManager;

        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if(verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if(isSprinting && v!=0)
            {
                v = 2;
                h = horizontalMovement;
            }

            playerManager.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            playerManager.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DissableCollision()
        {
            playerManager.playerLocomotionManager.characterCollider.enabled = false;
            playerManager.playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
        }

        public void EnableCollision()
        {
            playerManager.playerLocomotionManager.characterCollider.enabled = true;
            playerManager.playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
        }

        private void OnAnimatorMove()
        {
            if (characterManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerManager.playerLocomotionManager.rigidbody.drag = 0;
            Vector3 deltaPosition = playerManager.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerManager.playerLocomotionManager.rigidbody.velocity = velocity;

        }
    }
}
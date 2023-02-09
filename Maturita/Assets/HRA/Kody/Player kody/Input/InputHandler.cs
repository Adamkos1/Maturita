using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    
    public class InputHandler : MonoBehaviour
    {
        PlayerControls inputActions;
        PlayerManager playerManager;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public float rollInputTimer;

        public bool twoHandFlag;
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool inventoryFlag;
        public bool lockOnFlag;
        public bool drinkFlag;
        public bool fireFlag;

        public bool a_Input;
        public bool b_Input;
        public bool x_Input;
        public bool y_input;

        public bool tap_RB_Input;
        public bool hold_LB_Input;
        public bool tap_RT_Input;
        public bool hold_RT_Input;
        public bool tap_LT_Input;
        public bool tap_LB_Input;

        public bool hold_RB_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool input_Has_Been_Qued;
        public float current_Qued_Input_Timer;
        public float default_Qued_Input_Time;
        public bool qued_RB_Input;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void OnEnable() 
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.TapRB.performed += i => tap_RB_Input = true;

                inputActions.PlayerActions.HoldRB.performed += i => hold_RB_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_RB_Input = false;

                inputActions.PlayerActions.HoldRT.performed += i => hold_RT_Input = true;
                inputActions.PlayerActions.HoldRT.canceled += i => hold_RT_Input = false;

                inputActions.PlayerActions.RT.performed += i => tap_RT_Input = true;
                inputActions.PlayerActions.LT.performed += i => tap_LT_Input = true;

                inputActions.PlayerActions.LB.performed += i => hold_LB_Input = true;
                inputActions.PlayerActions.LB.canceled += i => hold_LB_Input = false;

                inputActions.PlayerActions.TapLB.performed += i => tap_LB_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Interact.performed += i => a_Input = true;
                inputActions.PlayerActions.Consume.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Two_Handed.performed += i => y_input = true;

                inputActions.PlayerActions.QuedRB.performed += i => QueInput(ref qued_RB_Input);
            }
            inputActions.Enable();
        }

        private void OnDisable() 
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if (playerManager.isDead)
                return;

            HandleMoveInput();
            HandleRollInput();

            HandleHoldRBInput();
            HandleHoldLBInput();
            HandleHoldRTInput();

            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();
            HandleTapLBInput();

            HandleQuickSlotInput();
            HandleInventoryInput();

            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            HandleQuedInput();
        }


        private void HandleMoveInput()
        {
            if(playerManager.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }       
        }

        private void HandleRollInput()
        { 
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;

                if(playerManager.playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if(moveAmount > 0.5f && playerManager.playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.8f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }



        private void HandleHoldRBInput()
        {
            if (hold_RB_Input)
            {
                if (playerManager.playerInventoryManager.rightWeapon.oh_hold_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                    playerManager.playerInventoryManager.rightWeapon.oh_hold_RB_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (playerManager.isInAir || playerManager.isSprinting || playerManager.isFiringSpell)
            {
                hold_LB_Input = false;
                return;
            }

            if (hold_LB_Input)
            {
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerManager.playerInventoryManager.rightWeapon.oh_hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                        playerManager.playerInventoryManager.rightWeapon.oh_hold_LB_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerManager.playerInventoryManager.leftWeapon.oh_hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.leftWeapon;
                        playerManager.playerInventoryManager.leftWeapon.oh_hold_LB_Action.PerformAction(playerManager);
                    }
                }
            }
            else if (hold_LB_Input == false)
            {
                if (playerManager.isAiming)
                {
                    playerManager.isAiming = false;
                    playerManager.uIManager.crossHair.SetActive(false);
                    playerManager.cameraHandler.ResetAimCameraRotations();
                }

                if (playerManager.isBlocking)
                {
                    playerManager.isBlocking = false;
                    playerManager.isUsingLeftHand = false;
                }
            }
        }

        private void HandleHoldRTInput()
        {
            playerManager.animator.SetBool("isChargingAttack", hold_RT_Input);

            if(hold_RT_Input)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;

                if(playerManager.isTwoHandingWeapon)
                {
                    if(playerManager.playerInventoryManager.rightWeapon.th_hold_RT_Action != null)
                    {
                        playerManager.playerInventoryManager.rightWeapon.th_hold_RT_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerManager.playerInventoryManager.rightWeapon.oh_hold_RT_Action != null)
                    {
                        playerManager.playerInventoryManager.rightWeapon.oh_hold_RT_Action.PerformAction(playerManager);
                    }
                }
            }
        }


        private void HandleTapRTInput()
        {
            if (tap_RT_Input)
            {
                tap_RT_Input = false;

                if (playerManager.isInteracting)
                    return;

                if (playerManager.playerInventoryManager.rightWeapon.oh_tap_RT_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                    playerManager.playerInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleTapLTInput()
        {
            if (tap_LT_Input)
            {
                tap_LT_Input = false;

                if(playerManager.isTwoHandingWeapon)
                {

                    if (playerManager.playerInventoryManager.rightWeapon.oh_tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                        playerManager.playerInventoryManager.rightWeapon.oh_tap_LT_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if(playerManager.playerInventoryManager.leftWeapon.oh_tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.leftWeapon;
                        playerManager.playerInventoryManager.leftWeapon.oh_tap_LT_Action.PerformAction(playerManager);

                    }
                }
            }
        }

        private void HandleTapRBInput()
        {
            if (tap_RB_Input)
            {
                tap_RB_Input = false;

                if(playerManager.playerInventoryManager.rightWeapon.oh_tap_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                    playerManager.playerInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_LB_Input)
            {
                tap_LB_Input = false;

                    if (playerManager.isTwoHandingWeapon)
                    {
                        if (playerManager.playerInventoryManager.rightWeapon.oh_tap_LB_Action != null)
                        {
                            playerManager.UpdateWhichHandCharacterIsUsing(true);
                            playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.rightWeapon;
                            playerManager.playerInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(playerManager);
                        }

                    }
                    else
                    {
                        if(playerManager.playerInventoryManager.leftWeapon.oh_tap_LB_Action != null)
                        {
                            playerManager.UpdateWhichHandCharacterIsUsing(false);
                            playerManager.playerInventoryManager.currentItemBeingUsed = playerManager.playerInventoryManager.leftWeapon;
                            playerManager.playerInventoryManager.leftWeapon.oh_tap_RB_Action.PerformAction(playerManager);
                        }
                    }
            }
        }



        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerManager.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerManager.playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {     
            if(inventoryFlag)
            {
                playerManager.uIManager.UpdateUI();
            }

            if(inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if(inventoryFlag)
                {
                    playerManager.uIManager.OpenSelectWindow();
                    playerManager.uIManager.hudWindow.SetActive(false);
                }
                else
                {
                    playerManager.uIManager.CloseSelectWindow();
                    playerManager.uIManager.CloseAllInventoryWindows();
                    playerManager.uIManager.hudWindow.SetActive(true);
                }
            }
        }


        private void HandleLockOnInput()
        {
            if(lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                playerManager.cameraHandler.HandleLockOn();

                if(playerManager.cameraHandler.nearestLockOnTarget != null)
                {
                    playerManager.cameraHandler.currentLockOnTarget = playerManager.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                playerManager.cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                playerManager.cameraHandler.HandleLockOn();
                if(playerManager.cameraHandler.leftLockTarget != null)
                {
                    playerManager.cameraHandler.currentLockOnTarget = playerManager.cameraHandler.leftLockTarget;
                }
            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                playerManager.cameraHandler.HandleLockOn();
                if (playerManager.cameraHandler.rightLockTarget != null)
                {
                    playerManager.cameraHandler.currentLockOnTarget = playerManager.cameraHandler.rightLockTarget;
                }
            }

            playerManager.cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if(y_input)
            {
                y_input = false;
                twoHandFlag = !twoHandFlag;

                if(twoHandFlag)
                {
                    playerManager.isTwoHandingWeapon = true;
                    playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.rightWeapon, false);
                    playerManager.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    playerManager.isTwoHandingWeapon = false;
                    playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.rightWeapon, false);
                    playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.leftWeapon, true);
                    playerManager.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                }

            }
        }

        private void HandleUseConsumableInput()
        {
            if(x_Input)
            {
                x_Input = false;
                playerManager.playerInventoryManager.currentConsumableItem.AttemptToConsumeItem(playerManager.playerAnimatorManager, playerManager.playerWeaponSlotManager, playerManager.playerEffectsManager);
            }
        }

        private void QueInput(ref bool quedInput)
        {
            if(playerManager.isInteracting)
            {
                quedInput = true;
                current_Qued_Input_Timer = default_Qued_Input_Time;
                input_Has_Been_Qued = true;
            }
        }

        private void HandleQuedInput()
        {
            if(input_Has_Been_Qued)
            {
                if(current_Qued_Input_Timer > 0)
                {
                    current_Qued_Input_Timer = current_Qued_Input_Timer - Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    input_Has_Been_Qued = false;
                    current_Qued_Input_Timer = 0;
                }
            }
        }

        private void ProcessQuedInput()
        {
            if(qued_RB_Input)
            {
                tap_RB_Input = true;
            }
        }

    }

}
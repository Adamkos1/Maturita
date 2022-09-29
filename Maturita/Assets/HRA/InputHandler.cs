using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    
    public class InputHandler : MonoBehaviour
    {
        public float Horizontal;
        public float Vertical;
        public float movetAmount;
        public float mouseX;
        public float mouseY;

        PlayerControls inputActions;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void OnEnable() 
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }

        private void OnDisable() 
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {

        }

        private void MoveInput(float delta)
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
            movetAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
            
        }
        
    }


}
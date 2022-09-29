using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

public class PlayerLocomotion : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;

    public RigidBody rigidBody;
    public Gameobject normalCamera;

    [Header("Stats")]
    [SerializeField]
        float movementSpeed = 5;
        float rotatonSpeed = 10;


    void Start()
    {
        rigidBody = GetComponent<RigidBody>();
        inputHandler = GetComponent<InputHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;
    
    public void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmont;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir = cameraObject.right * inputHandler.horizontal;

        targetDir.Noralize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;
    }

}


}
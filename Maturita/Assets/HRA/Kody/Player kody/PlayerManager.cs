using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerManager : CharacterManager
    {

        [Header("UI")]
        public UIManager uIManager;

        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("Input")]
        public InputHandler inputHandler;

        [Header("Player")]
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerLocomotionManager playerLocomotionManager;
        public PlayerStatsManager playerStatsManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEquipmentManager playerEquipmentManager;

        [Header("Interactible")]
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;

        public int currentScene;

        [Header("Footstep")]
        private float baseStepSpeed = 0.5f;
        private float sprintstepMultipler = 1.5f;
        public bool useFootSteps;
        public float currentOfset => isSprinting ? baseStepSpeed * sprintstepMultipler : baseStepSpeed;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            uIManager = FindObjectOfType<UIManager>();
            interactableUI = FindObjectOfType<InteractableUI>();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();

            WorldSaveGameManager.instance.player = this;
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            isJumping = animator.GetBool("isJumping");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", isDead);
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isGrounded", isGrounded);

            inputHandler.TickInput();
            playerLocomotionManager.HandleRollingAndSprinting();
            playerLocomotionManager.HandleJumping();
            playerStatsManager.RegenerateStamina();
            playerStatsManager.RegenerateMana();
            playerInventoryManager.ConsumableUI();

            CheckForInteractableObject();

            if(isInvulnerable)
            {
                playerLocomotionManager.characterCollider.enabled = false;
            }
            else
            {
                playerLocomotionManager.characterCollider.enabled = true;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            playerLocomotionManager.HandleMovement();
            playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
            playerLocomotionManager.HandleRotation();
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            isSprinting = inputHandler.b_Input;
            inputHandler.jump_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            if (isInAir)
            {
                playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
            }
        }

        #region Player Interaction

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if(inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }
                if(itemInteractableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; //zastavi hraca aby sa neklzal
            Vector3 rotationDirection = playerStandsHereWhenOpeningChest.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero;

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);

        }

        #endregion

        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {

            currentCharacterSaveData.characterName = playerStatsManager.chracterName;
            currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;

            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;

            //zbrane
            currentCharacterSaveData.currentRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            currentCharacterSaveData.currentLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;

            //naboje
            currentCharacterSaveData.currentAmmo = playerInventoryManager.currentAmmo.itemID;
            currentCharacterSaveData.currentAmmoAmount = playerInventoryManager.currentAmmo.currentAmount;

            //lektvary
            currentCharacterSaveData.currentConsumable = playerInventoryManager.currentConsumableItem.itemID;
            currentCharacterSaveData.currentConsumableAmount = playerInventoryManager.currentConsumableItem.currentItemAmount;

            //staty
            currentCharacterSaveData.healthlevel = playerStatsManager.healthlevel;
            currentCharacterSaveData.staminalevel = playerStatsManager.staminalevel;
            currentCharacterSaveData.manaLevel = playerStatsManager.manaLevel;
            currentCharacterSaveData.strenghtLevel = playerStatsManager.strenghtLevel;
            currentCharacterSaveData.dexterityLevel = playerStatsManager.dexterityLevel;
            currentCharacterSaveData.intelligenceLevel = playerStatsManager.intelligenceLevel;
            currentCharacterSaveData.faithLevel = playerStatsManager.faithLevel;
            currentCharacterSaveData.poiseLevel = playerStatsManager.poiseLevel;
            currentCharacterSaveData.currentSoulCount = playerStatsManager.currentSoulCount;
            currentCharacterSaveData.currentHealth = playerStatsManager.currentHealth;    
        }

        public void LoadCharacterDataFromCurrentSavaData(ref CharacterSaveData currentCharacterSaveData)
        {
            playerStatsManager.chracterName = currentCharacterSaveData.characterName;
            playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;

            transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

            //zbrane
            playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandWeaponID);
            playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            //naboje
            playerInventoryManager.currentAmmo = WorldItemDataBase.Instance.GetAmmoItemByID(currentCharacterSaveData.currentAmmo);
            playerInventoryManager.currentAmmo.currentAmount = currentCharacterSaveData.currentAmmoAmount;

            //lektvary
            playerInventoryManager.currentConsumableItem = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentConsumable);
            playerInventoryManager.currentConsumableItem.currentItemAmount = currentCharacterSaveData.currentConsumableAmount;

            //staty
            playerStatsManager.healthlevel = currentCharacterSaveData.healthlevel;
            playerStatsManager.staminalevel = currentCharacterSaveData.staminalevel;
            playerStatsManager.manaLevel = currentCharacterSaveData.manaLevel;
            playerStatsManager.strenghtLevel = currentCharacterSaveData.strenghtLevel;
            playerStatsManager.dexterityLevel = currentCharacterSaveData.dexterityLevel;
            playerStatsManager.intelligenceLevel = currentCharacterSaveData.intelligenceLevel;
            playerStatsManager.faithLevel = currentCharacterSaveData.faithLevel;
            playerStatsManager.poiseLevel = currentCharacterSaveData.poiseLevel;
            playerStatsManager.currentSoulCount = currentCharacterSaveData.currentSoulCount;
            playerStatsManager.currentHealth = currentCharacterSaveData.currentHealth;
        }
    }
}
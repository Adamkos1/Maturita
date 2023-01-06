using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AH
{

    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public RangedAmmoItem rangedAmmo;

        public ConsumableItem estusFlask;

        public bool isFlask;

        public bool isAmmo;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            if(isFlask)
            {
                PickUpEstusFlask(playerManager);
            }
            else if(isAmmo)
            {
                PickAmmo(playerManager);
            }
            else
            {
                PickUpItem(playerManager);
            }

        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);

        }

        private void PickUpEstusFlask(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.currentConsumableItem.currentItemAmount = playerInventory.currentConsumableItem.currentItemAmount + 5;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = estusFlask.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = estusFlask.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);

        }

        private void PickAmmo(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.currentAmmo.currentAmount = playerInventory.currentAmmo.currentAmount + 5;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = rangedAmmo.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = rangedAmmo.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);

        }
    }

}

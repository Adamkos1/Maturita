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

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
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
    }

}

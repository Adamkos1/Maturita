using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventoryManager;
        public BlockingCollider blockingCollider;


        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventory>();
        }


        public void OpenBlockingCollider()
        {
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
            }

            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }


    }

}


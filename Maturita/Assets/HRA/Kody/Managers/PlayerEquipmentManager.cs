using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager playerManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }


        public void OpenBlockingCollider()
        {
            if(playerManager.inputHandler.twoHandFlag)
            {
                playerManager.blockingCollider.SetColliderDamageAbsorption(playerManager.playerInventoryManager.rightWeapon);
            }
            else
            {
                playerManager.blockingCollider.SetColliderDamageAbsorption(playerManager.playerInventoryManager.leftWeapon);
            }

            playerManager.blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            playerManager.blockingCollider.DisableBlockingCollider();
        }


    }

}


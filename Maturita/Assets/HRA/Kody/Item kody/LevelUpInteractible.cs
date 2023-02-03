using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class LevelUpInteractible : Interactable
    {
        public Transform player;
        public PlayerManager playerManager;
        public AudioSource audioSource;
        public AudioClip helouthere;

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uIManager.levelUpWindow.SetActive(true);
            audioSource.PlayOneShot(helouthere);
        }

        private void LateUpdate()
        {
            float dist = Vector3.Distance(player.position, transform.position);

            if(dist > 2f)
            {
                playerManager.uIManager.levelUpWindow.SetActive(false);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    public class NPCInteraction : Interactable
    {
        public PlayerManager playerManager;
        public AudioSource audioSource;
        public AudioClip helouthere;

        public override void Interact(PlayerManager playerManager)
        {
            audioSource.PlayOneShot(helouthere);
        }
    }

}
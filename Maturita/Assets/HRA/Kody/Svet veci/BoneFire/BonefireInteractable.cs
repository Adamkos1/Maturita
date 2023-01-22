using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class BonefireInteractable : Interactable
    {
        [Header("Bonfire Teleport Transform")]
        public Transform bonfireTeleportTransform;

        [Header("Activation Status")]
        public bool bonfireHasBeenActivated;

        [Header("Bonfire FX")]
        public ParticleSystem actiovationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;

        private void Awake()
        {
            if(bonfireHasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "Oddychovat";
            }
            else
            {
                interactableText = "Zapalit ohnisko";
            }

            audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            Debug.Log("Bonfire interagoval si");

            if(bonfireHasBeenActivated)
            {
                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Start", true);
            }
            else
            {
                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
                playerManager.uIManager.ActivateBonfirePopUp();
                bonfireHasBeenActivated = true;
                interactableText = "Oddychovat";
                actiovationFX.gameObject.SetActive(true);
                actiovationFX.Play();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX);
            }
        }
    }

}

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
        public int bonefireID;
        public bool bonfireHasBeenActivated;

        [Header("Bonfire FX")]
        public ParticleSystem actiovationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;
        WorldSaveGameManager worldSaveGameManager;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
        }

        protected override void Start()
        {
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld.ContainsKey(bonefireID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld.Add(bonefireID, false);
            }

            bonfireHasBeenActivated = WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld[bonefireID];

            if (bonfireHasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "Oddychovat";
            }
            else
            {
                interactableText = "Zapalit ohnisko";
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (bonfireHasBeenActivated)
            {
                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Start", true);
            }
            else
            {
                if (WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld.ContainsKey(bonefireID))
                {
                    WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld.Remove(bonefireID);
                }

                WorldSaveGameManager.instance.currentCharacterSaveData.bonefiresInWorld.Add(bonefireID, true);

                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
                playerManager.uIManager.ActivateBonfirePopUp();
                bonfireHasBeenActivated = true;
                interactableText = "Oddychovat";
                actiovationFX.gameObject.SetActive(true);
                actiovationFX.Play();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX);
                worldSaveGameManager.saveGame = true;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterSoundFXManager : MonoBehaviour
    {
        PlayerManager playerManager;
        CharacterManager characterManager;
        public AudioSource audioSource;

        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private int lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private int lastWeaponWhoosh;

        [Header("Footstep")]
        private float footStepTimer = 0;
        public AudioClip[] grassClips;
        public AudioClip[] woodClips;
        public AudioClip[] stoneClips;

        [Header("No daco")]
        public AudioClip noAmmo;

        [Header("Parry/backstab")]
        public AudioClip stabsound;

        protected virtual void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            audioSource = GetComponent<AudioSource>();
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void PlayRandomDamageSoundFX()
        {
            int randomSound = Random.Range(0, takingDamageSounds.Length);
            if (randomSound == lastDamageSoundPlayed)
            {
                PlayRandomDamageSoundFX();
            }
            else
            {
                audioSource.PlayOneShot(takingDamageSounds[randomSound]);
                lastDamageSoundPlayed = randomSound;
            }
        }

        public virtual void PlayRandomWeaponWhoosh()
        {
            if (characterManager.isUsingRightHand)
            {
                int randomSound = Random.Range(0, characterManager.characterInventoryManager.rightWeapon.weaponWhooshes.Length);
                if (randomSound == lastWeaponWhoosh)
                {
                    PlayRandomWeaponWhoosh();
                }
                else
                {
                    audioSource.PlayOneShot(characterManager.characterInventoryManager.rightWeapon.weaponWhooshes[randomSound]);
                    lastWeaponWhoosh = randomSound;
                }
            }
            else if (characterManager.isUsingLeftHand && characterManager.characterInventoryManager.leftWeapon.weaponType == WeaponType.Shield)
            {

            }
            else
            {
                int randomSound = Random.Range(0, characterManager.characterInventoryManager.leftWeapon.weaponWhooshes.Length);
                if (randomSound == lastWeaponWhoosh)
                {
                    PlayRandomWeaponWhoosh();
                }
                else
                {
                    audioSource.PlayOneShot(characterManager.characterInventoryManager.leftWeapon.weaponWhooshes[randomSound]);
                    lastWeaponWhoosh = randomSound;
                }
            }

        }

        public virtual void HandleFootSteps()
        {
            if (characterManager.isGrounded == false)
                return;

            Vector3 origin = playerManager.transform.position;

            footStepTimer -= Time.deltaTime;

            if (footStepTimer <= 0)
            {
                if (Physics.Raycast(playerManager.transform.forward, Vector3.down, out RaycastHit hit, 5))
                {
                    switch (hit.collider.tag)
                    {
                        case "FootSteps/Wood":
                            audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                            break;
                        case "FootSteps/Stone":
                            audioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length)]);
                            break;
                        case "FootSteps/Grass":
                            audioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                            break;
                        default:
                            audioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                            break;

                    }
                }

                Debug.Log("kokot");
                footStepTimer = playerManager.currentOfset;
            }

        }

        public virtual void PlayStabSound()
        {
            audioSource.PlayOneShot(stabsound);
        }
    }
}

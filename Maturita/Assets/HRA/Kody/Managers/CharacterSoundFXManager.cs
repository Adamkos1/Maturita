using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterSoundFXManager : MonoBehaviour
    {
        CharacterManager characterManager;
        AudioSource audioSource;

        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private int lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private int lastWeaponWhoosh;

        protected virtual void Awake()
        {
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

        public virtual void PlayRadonmWeaponWhoosh()
        {
            if(characterManager.isUsingRightHand)
            {
                int randomSound = Random.Range(0, characterManager.characterInventoryManager.rightWeapon.weaponWhooshes.Length);
                if (randomSound == lastWeaponWhoosh)
                {
                    PlayRadonmWeaponWhoosh();
                }
                else
                {
                    audioSource.PlayOneShot(characterManager.characterInventoryManager.rightWeapon.weaponWhooshes[randomSound]);
                    lastWeaponWhoosh = randomSound;
                }
            }
            else
            {
                int randomSound = Random.Range(0, characterManager.characterInventoryManager.leftWeapon.weaponWhooshes.Length);
                if (randomSound == lastWeaponWhoosh)
                {
                    PlayRadonmWeaponWhoosh();
                }
                else
                {
                    audioSource.PlayOneShot(characterManager.characterInventoryManager.leftWeapon.weaponWhooshes[randomSound]);
                    lastWeaponWhoosh = randomSound;
                }
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager characterManager;

        [Header("Current Ranged FX")]
        public GameObject currentRangedFX;

        [Header("Damage FX")]
        public GameObject bloodSplatterFX;

        [Header("Weapon FX")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                if(rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }

        public virtual void StopWeaponFX(bool isLeft)
        {
            if (isLeft == false)
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.StopWeaponFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.StopWeaponFX();
                }
            }
        }

        public virtual void PlayBloodSplatterFX(Vector3 bloodSplaterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplaterLocation, Quaternion.identity);
        }
    }

}

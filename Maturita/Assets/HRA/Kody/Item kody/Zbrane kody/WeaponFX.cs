using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrial;

        private void Awake()
        {
            StopWeaponFX();
        }


        public void PlayWeaponFX()
        {
            normalWeaponTrial.Play();
        }


        public void StopWeaponFX()
        {
            normalWeaponTrial.Stop();
        }
    }

}

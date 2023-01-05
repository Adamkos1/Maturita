using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        string damageAnimation = "Damage_01";

        private void OnTriggerEnter(Collider other)
        {
            CharacterStatsManager characterStats = other.GetComponent<CharacterStatsManager>();

            if(characterStats != null)
            {
                characterStats.TakeDamage(damage, damageAnimation);
            }
        }
    }
}

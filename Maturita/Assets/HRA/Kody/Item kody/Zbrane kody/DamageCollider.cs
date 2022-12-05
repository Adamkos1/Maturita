using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damgeCollider;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damgeCollider = GetComponent<Collider>();
            damgeCollider.gameObject.SetActive(true);
            damgeCollider.isTrigger = true;
            damgeCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damgeCollider.enabled = true;
        }

        public void DissableDamageCollider()
        {
            damgeCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();

                if(enemyCharacterManager != null)
                {
                    if(enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }

                if(playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if(collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }

    }

}

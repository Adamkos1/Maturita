using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        public CharacterStatsManager characterStats;
        Collider damgeCollider;
        public bool enabledDamageColliderOnStartUp = false;

        [Header("Poise")]
        public float poiseBreak;
        public float offfensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        private void Awake()
        {
            characterStats = GetComponentInParent<CharacterStatsManager>();
            damgeCollider = GetComponent<Collider>();
            damgeCollider.gameObject.SetActive(true);
            damgeCollider.isTrigger = true;
            damgeCollider.enabled = false;
            damgeCollider.enabled = enabledDamageColliderOnStartUp;
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
                PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
                CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager playerEffectsManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if(playerCharacterManager != null)
                {
                    if(playerCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if(shield != null && playerCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if(playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }

                if(playerStats != null)
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                    playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                    playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                    if (playerStats.totalPoiseDefence > poiseBreak)
                    {
                        playerStats.TakeDamgeNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }

            if(collision.tag == "Enemy")
            {
                EnemyStatsManager enemyStats = collision.GetComponentInParent<EnemyStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponentInParent<CharacterManager>();
                CharacterEffectsManager enemyEffectsManager = collision.GetComponent<CharacterEffectsManager>();

                if (!enemyStats.isDead)
                {
                    BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                    if (enemyCharacterManager != null)
                    {
                        if (enemyCharacterManager.isParrying)
                        {
                            characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                            return;
                        }
                        else if (shield != null && enemyCharacterManager.isBlocking)
                        {
                            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                            if (enemyStats != null)
                            {
                                enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                                return;
                            }
                        }
                    }

                    if (enemyStats != null)
                    {
                        enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                        enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                        Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                        enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                        if (enemyStats.isBoss)
                        {
                            if (enemyStats.totalPoiseDefence > poiseBreak)
                            {
                                enemyStats.TakeDamgeNoAnimation(currentWeaponDamage);
                            }
                            else
                            {
                                enemyStats.TakeDamgeNoAnimation(currentWeaponDamage);
                                enemyStats.BreakGuard();
                            }
                        }
                        else
                        {
                            if (enemyStats.totalPoiseDefence > poiseBreak)
                            {
                                enemyStats.TakeDamgeNoAnimation(currentWeaponDamage);
                            }
                            else
                            {
                                enemyStats.TakeDamage(currentWeaponDamage);
                            }
                        }
                    }
                }
            }

            if(collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

    }

}

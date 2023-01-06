using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damgeCollider;
        public bool enabledDamageColliderOnStartUp = false;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offfensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        protected bool shieldHasBeenHit = false;
        protected bool hasBeenParried = false;
        protected string currentDamageAnimation;

        private void Awake()
        {
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

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager, enemyStats, shield);
                }

                if(enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);
                    enemyEffects.PlayBloodSplatterFX(contactPoint);

                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamgeNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(currentWeaponDamage, currentDamageAnimation);
                    }
                }
            }

            if(collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager, CharacterStatsManager enemyStats, BlockingCollider shield)
        {
            if (shield != null && enemyManager.isBlocking)
            {
                float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                    shieldHasBeenHit = true;
                }
            }
        }

        protected virtual void ChooseWichDirectionDamageCameFrom(float direction)
        {
            Debug.Log(direction);
            
            if(direction >= 145 && direction <= 180)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if (direction <= -145 && direction >= -180)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if(direction >= -45 && direction <= 15)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if(direction >= -144 && direction <= -45)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
            else if (direction >= 45 && direction <= 144)
            {
                currentDamageAnimation = "Damage_Left_01";
            }

        }

    }

}

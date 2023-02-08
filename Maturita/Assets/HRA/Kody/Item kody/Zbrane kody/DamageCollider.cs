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
        public int physicalDamage = 25;

        public float guardBreakModifier = 1;

        protected bool shieldHasBeenHit = false;
        protected bool hasBeenParried = false;
        protected string currentDamageAnimation;

        protected virtual void Awake()
        {
            damgeCollider = GetComponent<Collider>();
            damgeCollider.gameObject.SetActive(true);
            damgeCollider.isTrigger = true;
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
            if(collision.tag == ("Character"))
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();

                if(enemyManager != null)
                {
                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);
                }

                if(enemyManager.characterStatsManager != null)
                {
                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                    enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseBreak;

                    //detekuje kde na colajderi sa dotkne nassa zbran
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);
                    enemyManager.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
                    enemyManager.characterEffectsManager.InterruptEffect();

                    //da damage
                    DealDamage(enemyManager.characterStatsManager);
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

        protected virtual void CheckForBlock(CharacterManager enemyManager)
        {
            CharacterStatsManager enemyShield = enemyManager.characterStatsManager;
            Vector3 directionFromToEnemy = (characterManager.transform.position - enemyManager.transform.position);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromToEnemy, enemyManager.transform.forward);

            if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * enemyShield.blockingPhysicalDamageAbsorption) / 100;

                enemyManager.characterCombatManager.AttemptBlock(this, physicalDamage, "Block_01");
                enemyShield.TakeDamageAfterBlock(Mathf.RoundToInt(physicalDamageAfterBlock), characterManager);

            }
        }

        protected virtual void DealDamage(CharacterStatsManager enemyStats)
        {
            float finalPhysicalDamage = physicalDamage;

            //ak pozuivame pravu ruku a podla toho aky utok tak taky damage
            if(characterManager.isUsingRightHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackStaminaMultiplier;
                }
            }
            else if(characterManager.isUsingLeftHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackStaminaMultiplier;
                }
            }

            //deal modified damgage
            if (enemyStats.totalPoiseDefence > poiseBreak)
            {
                enemyStats.TakeDamgeNoAnimation(Mathf.RoundToInt(finalPhysicalDamage));
            }
            else
            {
                enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), currentDamageAnimation, characterManager);
            }
        }

        protected virtual void ChooseWichDirectionDamageCameFrom(float direction)
        { 
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

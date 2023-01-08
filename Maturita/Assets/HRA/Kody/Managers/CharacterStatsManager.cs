using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterAnimatorManager characterAnimatorManager;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int maxHealth;
        public int currentHealth;
        HealthBar healthBar;

        public float maxStamina;
        public float currentStamina;
        StaminaBar staminaBar;

        public float maxMana;
        public float currentMana;

        public int currentSoulCount = 0;
        public int soulsAwardedOnDeath = 50;

        [Header("Characeter Level")]
        public int playerLevel = 1;

        [Header("STAT Levels")]
        public int healthlevel = 10;
        public int staminalevel = 10;
        public int manaLevel = 10;
        public int strenghtLevel = 10;
        public int dexterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;
        public int poiseLevel = 10;


        [Header("Poise")]
        public float totalPoiseDefence;
        public float offensivePoiseBonus;
        public float armorPoiseBonus;
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        public bool isDead;

        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        private void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamage(int damage, string damageAnimation)
        {
            if (isDead)
                return;

            characterAnimatorManager.EraseHandIKForWeapon();
        }

        public virtual void TakeDamgeNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public void DrainStaminaBasedOnAttackType()
        {

        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthlevel * 10;
            return maxHealth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminalevel * 10;
            return maxStamina;
        }

        public float SetMaxManaFromManaLevel()
        {
            maxMana = manaLevel * 10;
            return maxMana;
        }
    }

}


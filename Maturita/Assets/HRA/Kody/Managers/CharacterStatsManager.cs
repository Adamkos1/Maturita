using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterStatsManager : MonoBehaviour
    {
        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int healthlevel = 10;
        public int maxHealth;
        public int currentHealth;
        HealthBar healthBar;

        public int staminalevel = 10;
        public float maxStamina;
        public float currentStamina;
        StaminaBar staminaBar;

        public int manaLevel = 10;
        public float maxMana;
        public float currentMana;

        public int soulCount = 0;
        public int soulsAwardedOnDeath = 50;

        [Header("Poise")]
        public float totalPoiseDefence;
        public float offensivePoiseBonus;
        public float armorPoiseBonus;
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        public bool isDead;

        private void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {

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
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerStats : MonoBehaviour
    {
        private const int V = 10;
        public int healthlevel = 10;
        public int maxHealth;
        public int currentHealth;
        HealthBar healthBar;

        public int staminalevel = 10;
        public int maxStamina;
        public int currentStamina;
        StaminaBar staminaBar;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthlevel * V;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminalevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01" , true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;

            staminaBar.SetCurrentStamina(currentStamina);
        }

    }

}

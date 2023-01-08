using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerStatsManager : CharacterStatsManager
    {
        StaminaBar staminaBar;
        HealthBar healthBar;
        ManaBar manaBar;
        PlayerAnimatorManager playerAnimatorHandler;
        PlayerManager playerManager;

        public float staminaRegenerationAmount = 30;
        public float staminaRegenerationTimer = 0;


        protected override void Awake()
        {
            base.Awake();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            manaBar = FindObjectOfType<ManaBar>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);

            maxMana = SetMaxManaFromManaLevel();
            currentMana = maxMana;
            manaBar.SetMaxMana(maxMana);
        }

        public override void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public override void TakeDamage(int damage, string damageAnimation)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            playerAnimatorHandler.PlayTargetAnimation(damageAnimation , true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }

        }

        public override void TakeDamgeNoAnimation(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            base.TakeDamgeNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;

            if(currentStamina <= 0)
            {
                currentStamina = 0;
            }

            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void SpendMana(int mana)
        {
            currentMana = currentMana - mana;

            if(currentMana < 0)
            {
                currentMana = 0;
            }

            manaBar.SetCurrentMana(currentMana);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else 
            {
                staminaRegenerationTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }

        public void HandleDeath()
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorHandler.PlayTargetAnimation("Death_01", true);
        }

    }

}

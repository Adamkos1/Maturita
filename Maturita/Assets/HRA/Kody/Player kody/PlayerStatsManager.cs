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


        private void Awake()
        {
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

            maxMana = SetMaxManaFromHealthLevel();
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

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthlevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminalevel * 10;
            return maxStamina;
        }

        private float SetMaxManaFromHealthLevel()
        {
            maxMana = manaLevel * 10;
            return maxMana;
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            playerAnimatorHandler.PlayTargetAnimation(damageAnimation , true);


        }

        public override void TakeDamgeNoAnimation(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            base.TakeDamgeNoAnimation(damage);
            if (currentHealth <= 0)
            {
                playerAnimatorHandler.PlayTargetAnimation("Death_01", true);
            }
            healthBar.SetCurrentHealth(currentHealth);
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
            soulCount = soulCount + souls;
        }

    }

}
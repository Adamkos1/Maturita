using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerStatsManager : CharacterStatsManager
    {
        public StaminaBar staminaBar;
        public HealthBar healthBar;
        public ManaBar manaBar;
        PlayerManager playerManager;

        public float staminaRegenerationAmount = 5;
        public float staminaRegenerationAmountWhileBlocking = 0.5f;
        public float staminaRegenerationTimer = 0;

        public float manaRegenerationAmount = 30;
        public float manaRegenerationTimer = 0;



        protected override void Awake()
        {
            base.Awake();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            manaBar = FindObjectOfType<ManaBar>();
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

        public override void TakeDamage(int damage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (playerManager.isInvulnerable)
                return;

            if (playerManager.isDead)
                return;

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            playerManager.playerAnimatorManager.PlayTargetAnimation(damageAnimation , true);

            playerManager.characterSoundFXManager.PlayRandomDamageSoundFX();

            if (currentHealth <= 0)
            {
                HandleDeath();
            }

        }

        public override void TakeDamgeNoAnimation(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (playerManager.isDead)
                return;

            base.TakeDamgeNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public override void DrainStamina(float staimnaToDrain)
        {
            base.DrainStamina(staimnaToDrain);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
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

                if(currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    if (playerManager.isBlocking)
                    {
                        currentStamina += staminaRegenerationAmountWhileBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        currentStamina += staminaRegenerationAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                }

            }
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

        public void RegenerateMana()
        {
            if (playerManager.isInteracting)
            {
                manaRegenerationTimer = 0;
            }
            else
            {
                manaRegenerationTimer += Time.deltaTime;

                if (currentMana < maxMana && manaRegenerationTimer > 1f)
                {
                    currentMana += manaRegenerationAmount * Time.deltaTime;
                    manaBar.SetCurrentMana(Mathf.RoundToInt(currentMana));
                }
            }
        }

        public override void HealCharacter(int healAmount)
        {
            base.HealCharacter(healAmount);

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }

        public void HandleDeath()
        {
            currentHealth = 0;
            playerManager.isDead = true;
            playerManager.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
        }

    }

}

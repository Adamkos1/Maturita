using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemyManager;
        public UIEnemyHealthBar enemyHealthBar;
        EnemyBossManager enemyBossManager;
        EnemyAnimatorManager enemyAnimatorManager;

        public bool isBoss;

        private void Awake()
        {
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            if(!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }

        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !enemyManager.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthlevel * 10;
            return maxHealth;
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {

            base.TakeDamage(damage, damageAnimation = "Damage_01");

            if (isDead)
                return;

            if (enemyManager.isInvulnerable)
                return;


            if (!isBoss)
            {
                currentHealth = currentHealth - damage;
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                currentHealth = currentHealth - damage;
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    isDead = true;
                    HandleDeath();
                }
            }

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public override void TakeDamgeNoAnimation(int damage)
        {
            if (enemyManager.isInvulnerable)
                return;

            if (isDead)
                return;

            base.TakeDamgeNoAnimation(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                currentHealth = currentHealth - damage;
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    isDead = true;
                    HandleDeath();
                }
            }

            if (currentHealth <= 0 && !enemyManager.canBeRiposted)
            {
                HandleDeath();
            }
        }

        public void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
            isDead = true;
        }

        public void BreakGuard()
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

    }

}

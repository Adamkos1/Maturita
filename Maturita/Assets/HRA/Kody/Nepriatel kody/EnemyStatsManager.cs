using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemyManager;
        public UIEnemyHealthBar enemyHealthBar;

        public bool isBoss;

        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
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

        public override void TakeDamage(int damage, string damageAnimation)
        {

            base.TakeDamage(damage, damageAnimation);

            if (enemyManager.isDead)
                return;

            if (enemyManager.isInvulnerable)
                return;


            if (!isBoss)
            {
                currentHealth = currentHealth - damage;
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyManager.enemyBossManager != null)
            {
                currentHealth = currentHealth - damage;
                enemyManager.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    enemyManager.isDead = true;
                    HandleDeath();
                }
            }

            enemyManager.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public override void TakeDamgeNoAnimation(int damage)
        {
            if (enemyManager.isInvulnerable)
                return;

            if (enemyManager.isDead)
                return;

            base.TakeDamgeNoAnimation(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyManager.enemyBossManager != null)
            {
                currentHealth = currentHealth - damage;
                enemyManager.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    enemyManager.isDead = true;
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
            enemyManager.enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
            enemyManager.isDead = true;
        }

        public void BreakGuard()
        {
            enemyManager.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

    }

}

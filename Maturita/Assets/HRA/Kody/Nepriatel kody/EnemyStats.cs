using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class EnemyStats : CharacterStats
    {
        public UIEnemyHealthBar enemyHealthBar;
        EnemyAnimatorManager enemyAnimatorManager;

        public int soulsAwardedOnDeath = 50;

        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthlevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void TakeDamgeNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
            isDead = true;
        }

    }

}

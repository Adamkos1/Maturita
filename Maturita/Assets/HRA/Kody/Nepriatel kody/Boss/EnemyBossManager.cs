using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
        UIBossHealthBar bossHealthBar;
        BossCombatStanceState bossCombatStanceState;
        EnemyManager enemyManager;
        WorldEventManager worldEventManager;

        [Header("Second Phase FX")]
        public GameObject particleFx;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyManager = GetComponent<EnemyManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyManager.enemyStatsManager.maxHealth);
        }

        private void Update()
        {
            if(enemyManager.currentTarget != null)
            {
                worldEventManager.ActiveBossFight();
            }

            if(enemyManager.enemyStatsManager.currentHealth <= 0)
            {
                worldEventManager.BossHasBeenDefeated();
            }
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                bossCombatStanceState.hasPhaseShifted = true;
                ShiftToSecondPhase();
            }
        }

        public void ShiftToSecondPhase()
        {
            enemyManager.animator.SetBool("isInvulnerable", true);
            enemyManager.animator.SetBool("isPhaseShifting", true);
            enemyManager.enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }

}
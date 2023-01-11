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

        [Header("Second Phase FX")]
        public GameObject particleFx;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyManager = GetComponent<EnemyManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyManager.enemyStatsManager.maxHealth);
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
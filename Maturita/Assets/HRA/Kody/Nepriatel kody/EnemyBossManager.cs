using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
        UIBossHealthBar bossHealthBar;
        EnemyStats enemyStats;
        EnemyAnimatorManager enemyAnimatorManager;
        BossCombatStanceState bossCombatStanceState;

        [Header("Second Phase FX")]
        public GameObject particleFx;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
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
            enemyAnimatorManager.anim.SetBool("isInvulnerable", true);
            enemyAnimatorManager.anim.SetBool("isPhaseShifting", true);
            enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemyManager;
        EnemyEffectsManager enemyEffectsManager;
        EnemyBossManager enemyBossManager;


        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);


                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }

        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

            GameObject phaseFX = Instantiate(enemyBossManager.particleFx, bossFXTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            enemyEffectsManager.PlayWeaponFX(false);
        }

        public void StopWeaponTrailFX()
        {
            enemyEffectsManager.StopWeaponFX(false);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }


    }

}

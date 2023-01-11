using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemyManager;

        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyManager.characterStatsManager.soulsAwardedOnDeath);


                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
                }
            }

        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

            GameObject phaseFX = Instantiate(enemyManager.enemyBossManager.particleFx, bossFXTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            enemyManager.enemyEffectsManager.PlayWeaponFX(false);
        }

        public void StopWeaponTrailFX()
        {
            enemyManager.enemyEffectsManager.StopWeaponFX(false);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = enemyManager.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= enemyManager.animator.deltaRotation;
            }
        }


    }

}

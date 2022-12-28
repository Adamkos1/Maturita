using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        EnemyBossManager enemyBossManager;


        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
            enemyBossManager = GetComponentInParent<EnemyBossManager>();
        }

        public override void TakeCriticalDamgeAnimationEvent()
        {
            enemyStats.TakeDamgeNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);


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

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }


    }

}

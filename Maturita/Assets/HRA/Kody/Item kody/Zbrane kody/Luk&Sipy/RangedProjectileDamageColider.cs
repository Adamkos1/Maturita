using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class RangedProjectileDamageColider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetratedASurface;
        protected GameObject penetratedProjectile;

        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();

                if (enemyManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);
                }

                if (enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);
                    enemyEffects.PlayBloodSplatterFX(contactPoint);

                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamgeNoAnimation(physicalDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, currentDamageAnimation, characterManager);
                    }
                }
            }

            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }

            if(!hasAlreadyPenetratedASurface && penetratedProjectile == null)
            {
                hasAlreadyPenetratedASurface = true;
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));

                penetratedProjectile = penetratedArrow;
                penetratedArrow.transform.parent = collision.transform;
                penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
            }

            Destroy(transform.root.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class RangedProjectileDamageColider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetratedASurface;

        Rigidbody arrowRigidBody;
        CapsuleCollider arrowCapsuleCollider;

        protected override void Awake()
        {
            damgeCollider = GetComponent<Collider>();
            damgeCollider.gameObject.SetActive(true);
            damgeCollider.enabled = true;
            arrowCapsuleCollider = GetComponent<CapsuleCollider>();
            arrowRigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterManager enemyManager = collision.gameObject.GetComponent<CharacterManager>();

                if (enemyManager != null)
                {
                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                    enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);//toto detekuje kde sa kolajderi stretnu
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWichDirectionDamageCameFrom(directionHitFrom);
                    enemyManager.characterEffectsManager.PlayBloodSplatterFX(contactPoint);

                    if (enemyManager.characterStatsManager.totalPoiseDefence > poiseBreak)
                    {
                        enemyManager.characterStatsManager.TakeDamgeNoAnimation(physicalDamage);
                    }
                    else
                    {
                        enemyManager.characterStatsManager.TakeDamage(physicalDamage, currentDamageAnimation, characterManager);
                    }
                }
            }

            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }

            if(!hasAlreadyPenetratedASurface)
            {
                hasAlreadyPenetratedASurface = true;
                arrowRigidBody.isKinematic = true;
                arrowCapsuleCollider.enabled = false;

                gameObject.transform.position = collision.GetContact(0).point;
                gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
                gameObject.transform.parent = collision.collider.transform;
            }
        }

        private void FixedUpdate()
        {
            if(arrowRigidBody.velocity != Vector3.zero)
            {
                arrowRigidBody.rotation = Quaternion.LookRotation(arrowRigidBody.velocity);
            }
        }
    }

}

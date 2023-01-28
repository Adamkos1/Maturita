using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterStatsManager spellTarget;
        IllusionaryWall spelltargetwall;
        Rigidbody rigidbody;
        Vector3 impactNormal;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;


        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                characterManager = collision.transform.GetComponent<CharacterManager>();
                spellTarget = collision.transform.GetComponent<CharacterStatsManager>();
                spelltargetwall = collision.transform.GetComponent<IllusionaryWall>();

                if(spelltargetwall != null)
                {
                    spelltargetwall.wallHasBeenHit = true;
                }


                if (spellTarget != null && spellTarget.teamIDNumber != teamIDNumber)
                {
                    spellTarget.TakeDamage(physicalDamage, currentDamageAnimation, characterManager);
                }

                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 0.5f);
                Destroy(gameObject);
            }
        }

    }

}

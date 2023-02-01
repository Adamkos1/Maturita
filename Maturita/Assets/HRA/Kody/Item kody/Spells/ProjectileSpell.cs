using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]

    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;
        Rigidbody rigidbody;

        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);

            if(character.isUsingLeftHand)
            {
                GameObject instantiatedWarmUpspellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.transform);
                instantiatedWarmUpspellFX.gameObject.transform.localScale = new Vector3(1, 1, 1);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
            else
            {
                GameObject instantiatedWarmUpspellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
                instantiatedWarmUpspellFX.gameObject.transform.localScale = new Vector3(1, 1, 1);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
        }

        public override void SuccessfullyCastSpell(CharacterManager character)
        {
            base.SuccessfullyCastSpell(character);

            PlayerManager player = character as PlayerManager;

            //ak je caster hrac tak sprav toto
            if(player != null)
            {
                if (character.isUsingLeftHand)
                {
                    GameObject instantiatedSpellFX = Instantiate(spellCastFX, player.playerWeaponSlotManager.leftHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);

                    rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;

                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                    }


                    rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpVelocity);
                    rigidbody.useGravity = isEffectedByGravity;
                    rigidbody.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                }
                else
                {
                    GameObject instantiatedSpellFX = Instantiate(spellCastFX, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);

                    rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;

                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                    }


                    rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpVelocity);
                    rigidbody.useGravity = isEffectedByGravity;
                    rigidbody.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                }
            }
            //ak nie je caster hrac tak sprav toto
            else
            {

            }
        }
    }

}


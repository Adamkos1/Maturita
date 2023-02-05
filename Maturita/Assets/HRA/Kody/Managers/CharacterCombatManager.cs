using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Combat Transforms")]
        public Transform backstabReceiverTransform;
        public Transform riposteReceiverTransform;


        public LayerMask characterLayer;
        public float criticalAttackRange = 0.7f;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Attack Animations")]
        public string oh_Light_Attack_01 = "Oh_Light_Attack_01";
        public string oh_Light_Attack_02 = "Oh_Light_Attack_02";
        public string oh_Heavy_Attack_01 = "Oh_Heavy_Attack_01";
        public string oh_Runing_Attack_01 = "Oh_Runing_Attack_01";
        public string oh_Jumping_Attack_01 = "Oh_Jumping_Attack_01";
        public string oh_Charge_Attack_01 = "Oh_Charging_Attack_Charge_01";

        public string th_Runing_Attack_01 = "Th_Runing_Attack_01";
        public string th_Jumping_Attack_01 = "Th_Jumping_Attack_01";
        public string th_Light_Attack_01 = "Th_Light_Attack_01";
        public string th_Light_Attack_02 = "Th_Light_Attack_02";
        public string th_Heavy_Attack_01 = "Th_Heavy_Attack_01";
        public string th_Charge_Attack_01 = "Th_Charging_Attack_Charge_01";

        public string weaponArt = "Weapon Art";
        public int pendingCriticalDamage;

        public string lastAttack;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if(character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
            }
            else if(character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {

        }

        public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, string blockAnimation)
        {
            float staminaDamageAbsorption = (physicalDamage * attackingWeapon.guardBreakModifier) * (character.characterStatsManager.blockingStabilityRating / 100);

            float staminaDamage = (physicalDamage * attackingWeapon.guardBreakModifier) - staminaDamageAbsorption;

            character.characterStatsManager.currentStamina = character.characterStatsManager.currentStamina - staminaDamage;

            if(character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
            character.animator.SetBool("isFiringSpell", true);
        }

        IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerforimgBackStab)
        {
            for(float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
            {
                Quaternion backstabRotation = Quaternion.LookRotation(characterPerforimgBackStab.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
                transform.parent = characterPerforimgBackStab.characterCombatManager.backstabReceiverTransform;
                transform.localPosition = characterPerforimgBackStab.characterCombatManager.backstabReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerforimgRiposte)
        {
            for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
            {
                Quaternion backstabRotation = Quaternion.LookRotation(-characterPerforimgRiposte.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
                transform.parent = characterPerforimgRiposte.characterCombatManager.riposteReceiverTransform;
                transform.localPosition = characterPerforimgRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }


        public void GetBackStabbed(CharacterManager characterPerforimgBackStab)
        {
            character.isBeingBackStebbed = true;

            StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerforimgBackStab));
            character.characterAnimatorManager.PlayTargetAnimation("Back Stabbed", true);
        }

        public void GetRiposted(CharacterManager characterPerforimgBackStab)
        {
            character.isBeingRiposted = true;

            StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerforimgBackStab));
            character.characterAnimatorManager.PlayTargetAnimation("Riposted", true);
        }


        public void AttemptBackStabOrRiposte()
        {
            if (character.isInteracting)
                return;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(character.criticalAttackRaycastStartPoint.transform.position, character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

                if(enemyCharacter.canBeRiposted)
                {
                    if (dotValue <= 1.7f && dotValue >= 0.2f)
                    {
                        AttemptRiposte(hit);
                        return;
                    }
                }

                if(dotValue >= -0.75 && dotValue <= -0.49f)
                {
                    AttemptBackStab(hit);
                }
            }
        }

        private void AttemptBackStab(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if(enemyCharacter != null)
            {
                if(enemyCharacter.isBeingBackStebbed || !enemyCharacter.isBeingRiposted)
                {
                    //ked nas backstabnu aby nas ine debiliny neublizili
                    EnableIsInvulnerable();
                    character.isPerformingBackSteb = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * (character.characterInventoryManager.rightWeapon.physicalDamage));

                    int roundedCrtiticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCrtiticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
        }

        private void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (enemyCharacter.isBeingBackStebbed || !enemyCharacter.isBeingRiposted)
                {
                    //ked nas backstabnu aby nas ine debiliny neublizili
                    EnableIsInvulnerable();
                    character.isPerformingRiposte = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * (character.characterInventoryManager.rightWeapon.physicalDamage));

                    int roundedCrtiticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCrtiticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }

        private void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        private void ApplyPendingDamage()
        {
            character.characterStatsManager.TakeDamgeNoAnimation(pendingCriticalDamage);
        }

        public void EnableCanBeParried()
        {
            character.canBeParried = true;
        }

        public void DisableCanBeParried()
        {
            character.canBeParried = false;
        }

    }

}

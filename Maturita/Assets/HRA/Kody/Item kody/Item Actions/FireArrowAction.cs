using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]

    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;
            EnemyManager enemyManager = character as EnemyManager;


            if (character.isTwoHandingWeapon)
            {
                if(player != null)
                {
                    if(player.characterInventoryManager.currentAmmo.currentAmount > 0)
                    {
                        ArrowInstantiationLocation arrowInstantiationLocation;
                        arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

                        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                        bowAnimator.SetBool("isDrawn", false);
                        bowAnimator.Play("Bow_ONLY_Fire_01");
                        Destroy(character.characterEffectsManager.instantiatedFXModel);

                        //resetovat hracov live arrow
                        character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
                        character.animator.SetBool("isHoldingArrow", false);

                        //vytvorit a vystrelit live arrow
                        GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                        Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
                        RangedProjectileDamageColider damageColider = liveArrow.GetComponentInChildren<RangedProjectileDamageColider>();

                        if (character.isAiming)
                        {
                            Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                            RaycastHit hitPoint;

                            if (Physics.Raycast(ray, out hitPoint, 100.0f))
                            {
                                liveArrow.transform.LookAt(hitPoint.point);
                                Debug.Log(hitPoint.transform.name);
                            }
                            else
                            {
                                liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, character.lockOnTransform.eulerAngles.y, 0);

                            }
                        }
                        else
                        {
                            //dat naboju rychlost
                            if (player.cameraHandler.currentLockOnTarget != null)
                            {
                                Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                                liveArrow.transform.rotation = arrowRotation;
                            }
                            else
                            {
                                liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, character.lockOnTransform.eulerAngles.y, 0);
                            }
                        }


                        rigidbody.AddForce(liveArrow.transform.forward * player.characterInventoryManager.currentAmmo.forwardCelocity);
                        rigidbody.AddForce(liveArrow.transform.up * player.characterInventoryManager.currentAmmo.upwardVelocity);
                        rigidbody.useGravity = player.characterInventoryManager.currentAmmo.useGravity;
                        rigidbody.mass = player.characterInventoryManager.currentAmmo.ammoMass;
                        liveArrow.transform.parent = null;

                        //dat live arrowu damage
                        damageColider.characterManager = player;
                        damageColider.ammoItem = player.characterInventoryManager.currentAmmo;
                        damageColider.physicalDamage = player.characterInventoryManager.currentAmmo.physicalDamage;
                        character.characterInventoryManager.currentAmmo.currentAmount = player.characterInventoryManager.currentAmmo.currentAmount - 1;
                    }             
                }

                //AI (NEPRIATEL) vystreli sip
                else if (enemyManager != null)
                {

                    ArrowInstantiationLocation arrowInstantiationLocation;
                    arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

                    Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                    bowAnimator.SetBool("isDrawn", false);
                    bowAnimator.Play("Bow_ONLY_Fire_01");
                    Destroy(character.characterEffectsManager.instantiatedFXModel);

                    //resetovat hracov live arrow
                    character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
                    character.animator.SetBool("isHoldingArrow", false);

                    //vytvorit a vystrelit live arrow
                    GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, Quaternion.identity);
                    Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
                    RangedProjectileDamageColider damageColider = liveArrow.GetComponentInChildren<RangedProjectileDamageColider>();

                    //dat naboju rychlost
                    if (enemyManager.currentTarget != null)
                    {
                        Quaternion arrowRotation = Quaternion.LookRotation(enemyManager.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                        liveArrow.transform.rotation = arrowRotation;
                    }

                    rigidbody.AddForce(liveArrow.transform.forward * enemyManager.characterInventoryManager.currentAmmo.forwardCelocity);
                    rigidbody.AddForce(liveArrow.transform.up * enemyManager.characterInventoryManager.currentAmmo.upwardVelocity);
                    rigidbody.useGravity = enemyManager.characterInventoryManager.currentAmmo.useGravity;
                    rigidbody.mass = enemyManager.characterInventoryManager.currentAmmo.ammoMass;
                    liveArrow.transform.parent = null;

                    //dat live arrowu damage
                    damageColider.characterManager = enemyManager;
                    damageColider.ammoItem = enemyManager.characterInventoryManager.currentAmmo;
                    damageColider.physicalDamage = enemyManager.characterInventoryManager.currentAmmo.physicalDamage;
                    damageColider.teamIDNumber = enemyManager.characterStatsManager.teamIDNumber;
                }
            }
        }
    }

}
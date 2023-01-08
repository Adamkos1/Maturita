using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]

    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if(player.isTwoHandingWeapon && player.playerInventoryManager.currentAmmo.currentAmount > 0)
            {
                ArrowInstantiationLocation arrowInstantiationLocation;
                arrowInstantiationLocation = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

                Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                bowAnimator.SetBool("isDrawn", false);
                bowAnimator.Play("Bow_ONLY_Fire_01");
                Destroy(player.playerEffectsManager.currentRangedFX);

                //resetovat hracov live arrow
                player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
                player.playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

                //vytvorit a vystrelit live arrow
                GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
                RangedProjectileDamageColider damageColider = liveArrow.GetComponentInChildren<RangedProjectileDamageColider>();

                if (player.isAiming)
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
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);

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
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                    }
                }


                rigidbody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardCelocity);
                rigidbody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);
                rigidbody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
                rigidbody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                //dat live arrowu damage
                damageColider.characterManager = player;
                damageColider.ammoItem = player.playerInventoryManager.currentAmmo;
                damageColider.currentWeaponDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
                player.playerInventoryManager.currentAmmo.currentAmount = player.playerInventoryManager.currentAmmo.currentAmount - 1;
            }
        }
    }

}
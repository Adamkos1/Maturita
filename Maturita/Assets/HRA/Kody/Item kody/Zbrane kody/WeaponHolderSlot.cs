using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public Transform shieldParentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;
        public bool isShieldBackSlot;


        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null)
            {
                if(parentOverride != null && shieldParentOverride != null)
                {
                    if(currentWeapon.weaponType == WeaponType.StraightSword || currentWeapon.weaponType == WeaponType.FaithCaster || currentWeapon.weaponType == WeaponType.PyroCaster || currentWeapon.weaponType == WeaponType.Unarmed)
                    {
                        model.transform.parent = parentOverride;
                    }
                    else if (currentWeapon.weaponType == WeaponType.Shield)
                    {
                        model.transform.parent = shieldParentOverride;
                    }
                }

                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }

}
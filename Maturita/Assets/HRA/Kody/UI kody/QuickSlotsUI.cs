using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;
        public Image consumableIcon;
        public Image arrowIcon;

        public void UpdateWeaponQuickSlotUI(bool isLeft, WeaponItem weapon)
        {
            if(isLeft == false)
            {
                if(weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }

        public void UpdateConsumableSlotUI(ConsumableItem consumableItem)
        {
            ItemToNumber itemToNumber = FindObjectOfType<ItemToNumber>();

            if (consumableItem != null && consumableItem.itemIcon != null && itemToNumber != null)
            {
                FlaskItem flask = consumableItem as FlaskItem;

                if(flask != null)
                {
                    if(flask.currentItemAmount > 0)
                    {
                        consumableIcon.sprite = consumableItem.itemIcon;
                        consumableIcon.enabled = true;
                        itemToNumber.SetItemCountText(flask.currentItemAmount);
                    }
                    else
                    {
                        consumableIcon.sprite = flask.emptyImage;
                        consumableIcon.enabled = true;
                        itemToNumber.SetItemCountText(flask.currentItemAmount);
                    }
                }
                else
                {
                    consumableIcon.sprite = consumableItem.itemIcon;
                    consumableIcon.enabled = true;
                }
            }
        }

        public void UpdateArrowIcon(RangedAmmoItem currentAmmo)
        {
            AmmoToNumber ammoToNumber = FindObjectOfType<AmmoToNumber>();

            if (currentAmmo != null && currentAmmo.itemIcon != null && ammoToNumber != null)
            {
                arrowIcon.sprite = currentAmmo.itemIcon;
                arrowIcon.enabled = true;
                ammoToNumber.SetItemCountText(currentAmmo.currentAmount);
            }
            else
            {
                arrowIcon.sprite = null;
                arrowIcon.enabled = false;
            }
        }
    }

}

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
        public Image spellIcon;

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
            if(consumableItem != null && consumableItem.itemIcon != null)
            {
                FlaskItem flask = consumableItem as FlaskItem;

                if(flask != null)
                {
                    if(flask.currentItemAmount > 0)
                    {
                        consumableIcon.sprite = consumableItem.itemIcon;
                        consumableIcon.enabled = true;
                    }
                    else
                    {
                        consumableIcon.sprite = flask.emptyImage;
                        consumableIcon.enabled = true;
                    }
                }
                else
                {
                    consumableIcon.sprite = consumableItem.itemIcon;
                    consumableIcon.enabled = true;
                }
            }
        }

        public void UpdateSpellIcon(SpellItem spell)
        {
            if(spell != null && spell.itemIcon != null)
            {
                spellIcon.sprite = spell.itemIcon;
                spellIcon.enabled = true;
            }
            else
            {
                spellIcon.sprite = null;
                spellIcon.enabled = false;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class ItemStatsWindowUI : MonoBehaviour
    {
        public Text itemNameText;
        public Image itemIconImage;

        [Header("Equipment Stats Window")]
        public GameObject weaponStats;


        [Header("Weapon Stats")]
        public Text physicalDamageText;
        public Text magicDamageText;
        public Text physicalAbsorptionText;
        public Text magicAbsorptionText;

        public void UpdateWeaponItemStats(WeaponItem weapon)
        {
            CloseAllStatWindows();

            if (weapon != null)
            {
                if (weapon.itemName != null)
                {
                    itemNameText.text = weapon.itemName;
                }
                else
                {
                    itemNameText.text = string.Empty;
                }

                if (weapon.itemIcon != null)
                {
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = weapon.itemIcon;
                }
                else
                {
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                physicalDamageText.text = weapon.physicalDamage.ToString();
                physicalAbsorptionText.text = weapon.physicalDamageAbsorption.ToString();

                magicDamageText.text = weapon.magicDamage.ToString();
                magicAbsorptionText.text = weapon.magicDamageAbsorption.ToString();

                weaponStats.SetActive(true);
            }
            else
            {
                itemNameText.text = string.Empty;
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                weaponStats.SetActive(false);
            }
        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
        }
    }

}

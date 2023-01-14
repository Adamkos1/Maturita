using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class WeaponInventorySlot : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        WeaponItem item;

        private void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
                item = newItem;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if(uIManager.rightHandSlot01Selected)
            {
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Add(uIManager.playerManager.playerInventoryManager.weaponInRightHandSlots[0]);
                uIManager.playerManager.playerInventoryManager.weaponInRightHandSlots[0] = item;
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.rightHandSlot02Selected)
            {
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Add(uIManager.playerManager.playerInventoryManager.weaponInRightHandSlots[1]);
                uIManager.playerManager.playerInventoryManager.weaponInRightHandSlots[1] = item;
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot01Selected)
            {
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Add(uIManager.playerManager.playerInventoryManager.weaponInLeftHandSlots[0]);
                uIManager.playerManager.playerInventoryManager.weaponInLeftHandSlots[0] = item;
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot02Selected)
            {
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Add(uIManager.playerManager.playerInventoryManager.weaponInLeftHandSlots[1]);
                uIManager.playerManager.playerInventoryManager.weaponInLeftHandSlots[1] = item;
                uIManager.playerManager.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else
            {
                return;
            }

            uIManager.playerManager.playerInventoryManager.rightWeapon = uIManager.playerManager.playerInventoryManager.weaponInRightHandSlots[uIManager.playerManager.playerInventoryManager.currentRightWeaponIndex];
            uIManager.playerManager.playerInventoryManager.leftWeapon = uIManager.playerManager.playerInventoryManager.weaponInLeftHandSlots[uIManager.playerManager.playerInventoryManager.currentLeftWeaponIndex];

            uIManager.playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.playerManager.playerInventoryManager.rightWeapon, false);
            uIManager.playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.playerManager.playerInventoryManager.leftWeapon, true);

            uIManager.equipmentWindowUI.LoadWeaponsEquipmentScreen(uIManager.playerManager.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

    }

}

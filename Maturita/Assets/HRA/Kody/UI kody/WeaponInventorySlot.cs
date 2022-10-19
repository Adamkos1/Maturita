using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uIManager;

        public Image icon;
        WeaponItem item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
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
                playerInventory.weaponsInventory.Add(playerInventory.weaponInRightHandSlots[0]);
                playerInventory.weaponInRightHandSlots[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if(uIManager.rightHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponInRightHandSlots[1]);
                playerInventory.weaponInRightHandSlots[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponInLeftHandSlots[0]);
                playerInventory.weaponInLeftHandSlots[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponInLeftHandSlots[1]);
                playerInventory.weaponInLeftHandSlots[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else
            {
                return;
            }

            playerInventory.rightWeapon = playerInventory.weaponInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uIManager.equipmentWindowUI.LoadWeaponsEquipmentScreen(playerInventory);
            uIManager.ResetAllSelectedSlots();
        }
    }

}

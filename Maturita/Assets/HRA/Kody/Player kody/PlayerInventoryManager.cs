using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public List<WeaponItem> weaponsInventory;

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] != null)
            {
                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if(currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] != null)
            {
                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            if (currentRightWeaponIndex > weaponInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = - 1;
                rightWeapon = characterWeaponSlotManager.unarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            if (currentLeftWeaponIndex > weaponInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = characterWeaponSlotManager.unarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, true);
            }
        }

        public void ConsumableUI()
        {
            quickSlotsUI.UpdateConsumableSlotUI(currentConsumableItem);
        }
    }

}

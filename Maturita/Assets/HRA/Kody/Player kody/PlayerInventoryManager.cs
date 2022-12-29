using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class PlayerInventoryManager : MonoBehaviour
    {
        PlayerWeaponSlotManager playerWeaponSlotManager;

        [Header("Quick Slot Items")]
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumableItem;

        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[0];
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[0];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        public List<WeaponItem> weaponsInventory;


        private void Awake()
        {
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        private void Start()
        {
            rightWeapon = weaponInRightHandSlots[0];
            leftWeapon = weaponInLeftHandSlots[0];
            playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] != null)
            {
                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if(currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] != null)
            {
                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            if (currentRightWeaponIndex > weaponInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = - 1;
                rightWeapon = playerWeaponSlotManager.unarmedWeapon;
                playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
                playerWeaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            if (currentLeftWeaponIndex > weaponInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = playerWeaponSlotManager.unarmedWeapon;
                playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, true);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class EquipmentWindowUI : MonoBehaviour
    {

        public WeaponEquipmentSlotUI[] weaponEqiupmentSlotsUI;

        public void LoadWeaponsEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for(int i = 0; i < weaponEqiupmentSlotsUI.Length; i++)
            {
                if(weaponEqiupmentSlotsUI[i].rightHandSlot01)
                {
                    weaponEqiupmentSlotsUI[i].AddItem(playerInventory.weaponInRightHandSlots[0]);
                }
                else if (weaponEqiupmentSlotsUI[i].rightHandSlot02)
                {
                    weaponEqiupmentSlotsUI[i].AddItem(playerInventory.weaponInRightHandSlots[1]);
                }
                else if (weaponEqiupmentSlotsUI[i].leftHandSlot01)
                {
                    weaponEqiupmentSlotsUI[i].AddItem(playerInventory.weaponInLeftHandSlots[0]);
                }
                else
                {
                    weaponEqiupmentSlotsUI[i].AddItem(playerInventory.weaponInLeftHandSlots[1]);
                }

            }
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class UIManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public ItemStatsWindowUI itemStatsWindowUI;
        public EquipmentWindowUI equipmentWindowUI;
        private QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public GameObject crossHair;
        public Text soulCount;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentScreenWindow;
        public GameObject levelUpWindow;
        public GameObject itemStatsWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        [Header("Pop Ups")]
        BonefirePopUPUI bonfirePopUPUI;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();

            bonfirePopUPUI = GetComponentInChildren<BonefirePopUPUI>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsEquipmentScreen(playerManager.playerInventoryManager);
            //quickSlotsUI.UpdateSpellIcon(playerManager.playerInventoryManager.currentSpell);
            soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for(int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i < playerManager.playerInventoryManager.weaponsInventory.Count)
                {
                    if(weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                    }

                    weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            itemStatsWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }

        public void ActivateBonfirePopUp()
        {
            bonfirePopUPUI.DisplayBonfireLitPopUp();
        }
    }

}

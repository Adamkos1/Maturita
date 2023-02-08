using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterWeaponSlotManager characterWeaponSlotManager;
        protected QuickSlotsUI quickSlotsUI;

        [Header("Current Item Being Used")]
        public Item currentItemBeingUsed;

        [Header("Quick Slot Items")]
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumableItem;
        public RangedAmmoItem currentAmmo;

        public SpellItem pyroSpell;
        public SpellItem healSpell;

        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[0];
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[0];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        private void Awake()
        {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        }


        private void Start()
        {
            characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }

}

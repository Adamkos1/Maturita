using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [System.Serializable]

    public class CharacterSaveData
    {
        public string characterName;
        public int characterLevel;

        [Header("Equipment")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;
        public int currentAmmo;
        public int currentAmmoAmount;
        public int currentConsumable;
        public int currentConsumableAmount;

        [Header("Player Inventory")]
        public List<int> weaponsInventory;


        [Header("Player Stats")]
        public int healthlevel;
        public int staminalevel;
        public int manaLevel;
        public int strenghtLevel;
        public int dexterityLevel;
        public int intelligenceLevel;
        public int faithLevel;
        public int poiseLevel;
        public int currentSoulCount;
        public int currentHealth;


        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Items Looted From World")]
        public SerializbleDictionary<int, bool> itemsInWorld; //int je worltd item ID a bool je ze ci bool vyluteni

        [Header("Enemies killed")]
        public SerializbleDictionary<int, bool> enemiesInWorld;
        public SerializbleDictionary<int, bool> bossHasBeenKilled;
        public SerializbleDictionary<int, bool> bossFightStarted;


        public CharacterSaveData()
        {
            itemsInWorld = new SerializbleDictionary<int, bool>();
            enemiesInWorld = new SerializbleDictionary<int, bool>();
            bossHasBeenKilled = new SerializbleDictionary<int, bool>();
            bossFightStarted = new SerializbleDictionary<int, bool>();
        }


    }

}

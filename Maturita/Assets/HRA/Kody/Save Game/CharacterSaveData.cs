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
        public SerializbleDictionary<int, bool> itemsInWorld;

        public CharacterSaveData()
        {
            itemsInWorld = new SerializbleDictionary<int, bool>();
        }


    }

}

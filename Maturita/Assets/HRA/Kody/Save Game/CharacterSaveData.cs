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

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

    }

}

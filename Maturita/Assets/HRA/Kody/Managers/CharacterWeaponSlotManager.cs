using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;
        public WeaponHolderSlot backSlotForShield;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName ="Items/Ammo")]

    public class RangedAmmoItem : Item
    {
        [Header("Ammo Type")]
        public AmmoType ammoType;

        [Header("Ammo Velocity")]
        public float forwardCelocity = 250;
        public float upwardVelocity = 0;
        public float ammoMass = 0;
        public bool useGravity = false;

        [Header("Ammo Capacity")]
        public int carryLimit = 100;
        public int currentAmount = 50;

        [Header("Ammo Base Damage")]
        public int physicalDamage = 50;

        [Header("Item Models")]
        public GameObject loadedItemModel; //model ktory sa ukaze ked natiahneme luk
        public GameObject liveAmmoModel;  //model ktory dokaze ublizit
        public GameObject penetratedModel;  //model ktory sa zobrazi ked sa sip zapichne
    }

}

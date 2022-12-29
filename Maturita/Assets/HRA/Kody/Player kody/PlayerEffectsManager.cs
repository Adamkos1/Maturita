using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;

        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountTobeHealed;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
                playerStats.HealPlayer(amountTobeHealed);
                GameObject healEffect = Instantiate(currentParticleFX, playerStats.transform);
                Destroy(instantiatedFXModel.gameObject);
                Destroy(healEffect.gameObject, 1);
                weaponSlotManager.LoadBothWeaponsOnSlots();
        }

        public void DestroyWhenNone()
        {
            Destroy(instantiatedFXModel.gameObject);
            weaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }

}

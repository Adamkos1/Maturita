using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;

        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountTobeHealed;

        private void Awake()
        {
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
                playerStatsManager.HealPlayer(amountTobeHealed);
                GameObject healEffect = Instantiate(currentParticleFX, playerStatsManager.transform);
                Destroy(instantiatedFXModel.gameObject);
                Destroy(healEffect.gameObject, 1);
                playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        public void DestroyWhenNone()
        {
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }

}

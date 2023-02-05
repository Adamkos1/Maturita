using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager playerManger;

        public GameObject currentParticleFX;
        public GameObject activatedFXModel;

        public int amountTobeHealed;

        private void Awake()
        {
            playerManger = GetComponent<PlayerManager>();
        }

        public void HealPlayerFromEffect()
        {
                playerManger.playerStatsManager.HealCharacter(amountTobeHealed);
                GameObject healEffect = Instantiate(currentParticleFX, playerManger.playerStatsManager.transform);
                Destroy(activatedFXModel.gameObject);
                Destroy(healEffect.gameObject, 1);
                playerManger.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        public void DestroyWhenNone()
        {
            Destroy(activatedFXModel.gameObject);
            playerManger.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }

}

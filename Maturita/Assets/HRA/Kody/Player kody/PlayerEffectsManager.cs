using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager playerManger;

        private void Awake()
        {
            playerManger = GetComponent<PlayerManager>();
        }

        public void DestroyWhenNone()
        {
            Destroy(activatedFXModel.gameObject);
            playerManger.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        public void HealCharacterFromEffect()
        {
            playerManger.characterStatsManager.HealCharacter(amountToBeHealed);
            GameObject healEffect = Instantiate(currentParticleFX, playerManger.characterStatsManager.transform);
            Destroy(healEffect.gameObject, 1);
            playerManger.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }

}

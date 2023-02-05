using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]

    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public Sprite emptyImage;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if(currentItemAmount > 0)
            {
                base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
                GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
                playerEffectsManager.currentParticleFX = recoveryFX;
                playerEffectsManager.amountTobeHealed = healthRecoverAmount;
                playerEffectsManager.activatedFXModel = flask;
                weaponSlotManager.rightHandSlot.UnloadWeapon();
            }
        }
    }

}

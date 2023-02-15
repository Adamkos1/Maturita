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

        public override void AttemptToConsumeItem(CharacterManager character)
        {
            if(currentItemAmount > 0)
            {
                base.AttemptToConsumeItem(character);
                GameObject flask = Instantiate(itemModel, character.characterWeaponSlotManager.rightHandSlot.transform);
                character.characterEffectsManager.currentParticleFX = recoveryFX;
                character.characterEffectsManager.amountToBeHealed = healthRecoverAmount;
                character.characterEffectsManager.activatedFXModel = flask;
                character.characterWeaponSlotManager.rightHandSlot.UnloadWeapon();
            }
        }
    }

}

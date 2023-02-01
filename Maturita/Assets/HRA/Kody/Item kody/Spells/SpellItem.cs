using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int manaCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(CharacterManager character)
        {
            Debug.Log("skusil si spell");
        }

        public virtual void SuccessfullyCastSpell(CharacterManager character)
        {
            Debug.Log("dal si spell");

            PlayerManager player = character as PlayerManager;

            if(player != null)
            {
                player.playerStatsManager.SpendMana(manaCost);
            }
        }


    }

}

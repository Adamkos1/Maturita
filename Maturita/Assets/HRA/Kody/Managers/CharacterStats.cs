using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class CharacterStats : MonoBehaviour
    {
        public int healthlevel = 10;
        public int maxHealth;
        public int currentHealth;
        HealthBar healthBar;

        public int staminalevel = 10;
        public float maxStamina;
        public float currentStamina;
        StaminaBar staminaBar;

        public int manaLevel = 10;
        public float maxMana;
        public float currentMana;

        public int soulCount = 0;

        public bool isDead;
    }

}


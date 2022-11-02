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
        public int maxStamina;
        public int currentStamina;
        StaminaBar staminaBar;

        public bool isDead;
    }

}


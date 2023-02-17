using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class PlayerHealthBar : MonoBehaviour
    {
        public Slider slider;
        [SerializeField] float secondTimer = 3;
        private PlayerUIYellowBar yellowBar;

        private void Start()
        {
            slider = GetComponent<Slider>();
            yellowBar = GetComponentInChildren<PlayerUIYellowBar>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        public void SetCurrentHealth(int currentHealth)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true);

                yellowBar.timer = secondTimer;

                if (currentHealth > slider.value)
                {
                    yellowBar.slider.value = currentHealth;
                }
            }

            slider.value = currentHealth;
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace AH
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Slider slider;
        private float timeUntilIsHidden = 0;
        private UIYellowBar yellowBar;
        [SerializeField] float secondTimer = 3;
        [SerializeField] Text damageText;
        [SerializeField] int currentDamageTaken;


        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            yellowBar = GetComponentInChildren<UIYellowBar>();
        }

        private void OnDisable()
        {
            currentDamageTaken = 0;
        }

        public void SetHealth(int health)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true);

                yellowBar.timer = secondTimer;

                if (health > slider.value)
                {
                    yellowBar.slider.value = health;
                }
            }

            currentDamageTaken = currentDamageTaken + Mathf.RoundToInt(slider.value - health);
            damageText.text = currentDamageTaken.ToString();

            slider.value = health;
            timeUntilIsHidden = 5;   
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

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            timeUntilIsHidden = timeUntilIsHidden - Time.deltaTime;

            if (slider != null)
            {
                if (timeUntilIsHidden <= 0)
                {
                    timeUntilIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }
                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }

        }
    }

}

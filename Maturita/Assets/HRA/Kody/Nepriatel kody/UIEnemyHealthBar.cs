using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace AH
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        float timeUntilIsHidden = 0;


        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilIsHidden = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            timeUntilIsHidden = timeUntilIsHidden - Time.deltaTime;
            transform.LookAt(Camera.main.transform);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class PlayerUIYellowBar : MonoBehaviour
    {
        public Slider slider;
        PlayerHealthBar parentHealthBar;

        public float timer;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            parentHealthBar = GetComponentInParent<PlayerHealthBar>();
        }

        private void OnEnable()
        {
            if (timer <= 0)
            {
                timer = 2f;
            }
        }

        public void SetMaxStat(int maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }

        private void Update()
        {
            if (timer <= 0)
            {
                if (slider.value > parentHealthBar.slider.value)
                {
                    slider.value = slider.value - 0.5f;
                }
                else if (slider.value <= parentHealthBar.slider.value)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }
    }
}

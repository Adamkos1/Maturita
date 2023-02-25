using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class flickerLight : MonoBehaviour
    {
        public Light IDkligt;

        public float Timer;
        public float maxTime;


        private void Start()
        {
            Timer = maxTime;
        }

        private void Update()
        {
            FlickeringLight();
        }

        void FlickeringLight()
        {
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;
            }

            if(Timer <= 0)
            {
                IDkligt.enabled = !IDkligt.enabled;
                Timer = maxTime;
            }
        }
    }
}

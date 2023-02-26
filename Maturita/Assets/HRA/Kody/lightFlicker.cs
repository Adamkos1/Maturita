using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    public class lightFlicker : MonoBehaviour
    {
        public float flickerIntensity = 0.2f;
        public float flickerPerSecond = 3f;
        public float speedRandomness = 0.2f;


        float time = 0.2f;
        public float StartingIntensity = 0.2f;
        public Light light;

        private void Start()
        {
            StartingIntensity = light.intensity;
        }

        private void Update()
        {
            time += Time.deltaTime * (1 - Random.Range(-speedRandomness, speedRandomness)) * Mathf.PI;
            light.intensity = StartingIntensity + Mathf.Sin(time * flickerIntensity) * flickerIntensity;
        }


    }
}

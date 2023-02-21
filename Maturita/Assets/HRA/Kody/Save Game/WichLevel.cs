using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class WichLevel : MonoBehaviour
    {
        PlayerManager playerManager;

        public bool level1;
        public bool level0;


        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            if(level1)
            {
                playerManager.currentScene = 1;
            }
            else if(level0)
            {
                playerManager.currentScene = 0;
            }
        }
    }

}

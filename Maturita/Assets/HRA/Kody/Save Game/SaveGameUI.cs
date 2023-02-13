using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    public class SaveGameUI : MonoBehaviour
    {
        WorldSaveGameManager worldSaveGameManager;

        private void Awake()
        {
            worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
        }

        public void SaveGametru()
        {
            worldSaveGameManager.saveGame = true;
        }

        public void LoadGametru()
        {
            worldSaveGameManager.loadGame = true;
        }
    }
}

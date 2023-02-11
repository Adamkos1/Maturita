using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

        [Header("Save Data Writer")]
        SaveGameDataWriter saveGameDataWriter;

        [Header("Current Character Data")]
        public CharacterSaveData currentCharacterSaveData;

        [SerializeField] private string fileName;

        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            else if(loadGame)
            {
                loadGame = false;
            }
        }

        //New Game

        //Save game
        public void SaveGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;

            //da data o nasich characteroch do daneho save filu
            player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

            saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

            Debug.Log("Saving game..");
            Debug.Log("File saved as... " + fileName); ;
        }

        //Load game

    }

}

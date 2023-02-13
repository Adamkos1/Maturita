using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace AH
{

    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;
        SaveGameUI saveGameUI;

        [Header("Save Data Writer")]
        SaveGameDataWriter saveGameDataWriter;

        [Header("Current Character Data")]
        public CharacterSaveData currentCharacterSaveData;

        [SerializeField] private string fileName;

        [Header("Save/Load")]
        [SerializeField] public bool saveGame;
        [SerializeField] public bool loadGame;

        private void Awake()
        {
            saveGameUI = FindObjectOfType<SaveGameUI>();

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
                LoadGame();
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
        public void LoadGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldSceneAsynchronously());
        }

        private IEnumerator LoadWorldSceneAsynchronously()
        {
            if(player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(player.currentScene);

            while(!loadOperation.isDone)
            {
                float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                yield return null;
            }

            player.LoadCharacterDataFromCurrentSavaData(ref currentCharacterSaveData);

        }
    }

}

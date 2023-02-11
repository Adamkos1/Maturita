using System;
using System.IO;
using UnityEngine;


namespace AH
{

    public class SaveGameDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string dataSaveFileName = "";

        public CharacterSaveData LoadCharacterDataFromJson()
        {
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            CharacterSaveData loadedSaveData = null;

            if(File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }
                }
                catch(Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
        {
            //vytvori cestu na ulozenie nasho suboru
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("sAVE path = " + savePath);

                //serialize C# game data objekt do jsonu
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //zapisat subor do nasho systemu
                using(FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log("EROOR KED SA SNAZIL ULOZIT DATA TAK HRA NEMOHLA BYT ULOZENA" + ex);
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, dataSaveFileName));
        }

        public bool CheckIfSaveFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

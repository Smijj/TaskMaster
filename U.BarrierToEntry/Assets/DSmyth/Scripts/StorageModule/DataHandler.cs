using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.StorageModule {
    public static class DataHandler {
        public static GameData SaveData {
            get {
                string path = GameData.FilePath;        // Cache path
                GameData data = Load<GameData>(path);   // Get data object
                if (data == null) {
                    Debug.Log($"No data at \"{path}\". Creating new Data Object: {typeof(GameData).Name}.");
                    data = new GameData();  // Create new data object if it doesnt exist
                    Save(data, path);       // Save the new data to create the JSON file at path
                }
                return data;
            }
        }

        /// <summary>
        /// Internal Save function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveData"></param>
        /// <param name="filePath"></param>
        public static void Save<T>(T saveData, string filePath) {
            if (saveData == null) {
                Debug.Log($"Save Failed. Invalid saveData object: {typeof(T).Name}.");
                return;
            }
            if (filePath == string.Empty) {
                Debug.Log($"Save Failed. Invalid file path: '{filePath}'.");
                return;
            }

            string saveDataJSON = JsonUtility.ToJson(saveData);
            System.IO.File.WriteAllText(filePath, saveDataJSON);

            Debug.Log($"Save Sucessful.\nJSON: {saveDataJSON}\nPath: '{filePath}'");
        }
        /// <summary>
        /// Internal Load Function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Load<T>(string filePath) {
            if (!System.IO.File.Exists(filePath)) {
                Debug.Log($"Load Failed. No file at {filePath} path.");
                return default;
            }
            
            string saveDataJSON = System.IO.File.ReadAllText(filePath);
            T saveData = JsonUtility.FromJson<T>(saveDataJSON);
            return saveData;
        }
        
    }
}

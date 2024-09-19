using Libraries;
using UnityEngine;

namespace DataManagers
{
    /// <summary>
    /// Saves and load player's data
    /// </summary>
    public static class SavesHelper
    {
        /// <summary>
        /// Save player data to a JSON file
        /// </summary>
        /// <param name="playerData"></param>
        public static void SaveDataToFile(PlayerData playerData)
        {
            var path = Application.persistentDataPath + "/GameData.json";
            var json = JsonUtility.ToJson(playerData);

            Debug.Log("Data Saved to: [" + path + "]");
        
            //TODO: Здесь можно сделать асинхронную запись, но т.к. информции мало то нет необходимости
            System.IO.File.WriteAllText(path, json);
        }

        /// <summary>
        /// Load player data from JSON file
        /// </summary>
        /// <returns></returns>
        public static PlayerData LoadDataFromFile()
        {
            var path = Application.persistentDataPath + "/GameData.json";
			
            if(!System.IO.File.Exists(path)) 
                return new PlayerData();
			
            //TODO: Asynchronous reading can be done here, but since there is little information, there is no need in it
            var fileContents = System.IO.File.ReadAllText(path);
            var loadedValues = JsonUtility.FromJson<PlayerData>(fileContents);
        
            Debug.Log("Data Loaded from: [" + path + "]");
        
            return loadedValues;
        }

        /// <summary>
        /// Load the JSON file with the ingredients and overwrite the parameters in the local list
        /// </summary>
        public static IngredientsList LoadIngredientsFromFile()
        {
            // TODO: Now for convenience, loading is done from the Resources folder, in theory it can be done from the server
            var textAsset = Resources.Load("Ingredients", typeof(TextAsset)) as TextAsset;

            if (textAsset == null)
                return new IngredientsList();
        
            Debug.Log("Ingredients successfully loaded from JSON file.");
        
            var loadedValues = JsonUtility.FromJson<IngredientsList>(textAsset.text);

            return loadedValues;
        }
    }
}

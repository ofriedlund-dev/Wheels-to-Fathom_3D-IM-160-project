/*****************************************************************************
// File Name : FileDataHandler.cs
// Author : Owen M. Friedlund
// Creation Date : April 17, 2026
//
// Brief Description : This is a document that saves and loads game data into and from the json file
*****************************************************************************/
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName = "";
    private const string SavingFolderName = "Saves"; // e.g., "C:\...\YourGameFiles\Saves\"

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        Directory.CreateDirectory(Path.Combine(dataDirPath, SavingFolderName));
    }
    /// <summary>
    /// Loads the game's save data into the game from the json file
    /// </summary>
    /// <returns></returns>
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, SavingFolderName, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading game data from " + fullPath + "\n" + e);
            }
            
        }
        return loadedData;
    }
    /// <summary>
    /// Saves the game data into the scores to the game's json file
    /// </summary>
    /// <param name="data"></param>
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, SavingFolderName, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // Serialize the GameData to JSON
            string jsonData = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonData);
                }
            }

            // Write the JSON data to the file
            File.WriteAllText(fullPath, jsonData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game data to " + fullPath + "\n" + e);
        }
    }
}

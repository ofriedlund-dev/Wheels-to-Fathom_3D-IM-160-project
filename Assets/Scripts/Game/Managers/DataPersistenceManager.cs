/*****************************************************************************
// File Name : DataPersistenceManager.cs
// Author : Owen M. Friedlund
// Creation Date : April 17, 2026
//
// Brief Description : This is a document that holds the code for the DataPersistenceManager class,
// which is used to manage the persistence of game data across scenes and game sessions
*****************************************************************************/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField] private string dataFileName = "WheelstoFathomSaveData.json";
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance = null;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    /// <summary>
    /// This is the Awake function for the DataPersistenceManager script
    /// which implements the singleton pattern to ensure
    /// that only one instance of the DataPersistenceManager exists in the game and that it persists across scenes
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// This is the Start function for the DataPersistenceManager script,
    /// which initializes the FileDataHandler and finds all objects in the scene that
    /// implement the IDataPersistence interface
    /// </summary>
    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, dataFileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    /// <summary>
    /// This function is called to initialize a new game by creating a new GameData object with default values
    /// and it can be called when the player starts a new game or when no game data is found when trying to load a game
    /// </summary>
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    /// <summary>
    /// This function is called to save the game data from all objects in the scene that
    /// implement the IDataPersistence interface and it saves that data to a file using the FileDataHandler class
    /// and if no game data is found, it initializes a new GameData object with default values and saves that instead
    /// </summary>
    public void SaveGame()
    {
        // FIX: Check if gameData is null and initialize it before trying to save data.
        if (this.gameData == null)
        {
            Debug.LogWarning("Attempting to save game, but no GameData object found. Creating a new one with default values.");
            this.gameData = new GameData(); // Initialize GameData
        }

        // Pass data into the GameData object
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        
        // Save that GameData object to the file
        dataHandler.Save(gameData);
    }
    /// <summary>
    /// This function is called to load the game data from the file and pass it to all objects
    /// in the scene that implement the IDataPersistence interface and if no game data is found, it initializes
    /// a new GameData object with default values and passes that to the objects instead
    /// </summary>
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.LogWarning("No game data found! Unable to load!");
            NewGame();
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    /// <summary>
    /// This is the OnApplicationQuit function for the DataPersistenceManager script,
    /// which calls the SaveGame function to save the game data when the application is quit
    /// </summary>
    public void OnApplicationQuit()
    {
        SaveGame();
    }
    /// <summary>
    /// This is a helper function that finds all objects in the scene that implement the IDataPersistence interface
    /// and returns them as a list
    /// </summary>
    /// <returns></returns>
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}

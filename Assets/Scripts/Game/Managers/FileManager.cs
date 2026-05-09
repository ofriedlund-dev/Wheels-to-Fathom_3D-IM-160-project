/*****************************************************************************
// File Name : FileManager.cs
// Author : Owen M. Friedlund
// Creation Date : April 17, 2026
//
// Brief Description : This is a document that holds the code for the FileManager class,
// which is used to manage file operations in the game
*****************************************************************************/
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string streamingAssetsPath;
    private string gameDataPath;
    private const string WorkingFolderName = "Wheels to Fathom Data"; // e.g., "C:\...\YourGameFiles\"
    public static FileManager instance = null;
    /// <summary>
    /// This is the Awake function for the FileManager script, which initializes the singleton instance
    /// and sets up the file paths for the game data and streaming assets.
    /// It also checks if the player's working folder already exists and creates it if it doesn't, copying
    /// any necessary initial files from the streaming assets.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // 1. Define the paths
        gameDataPath = Path.Combine(Application.persistentDataPath, WorkingFolderName);
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, WorkingFolderName);
        // 2. Check if the player's working folder already exists
        if (!Directory.Exists(gameDataPath))
        {
            // If the folder doesn't exist, create it and copy initial files.
            InitializeWorkingFiles();
        }
    }
    /// <summary>
    /// This is a helper function that initializes the player's working folder by creating it and copying
    /// any necessary initial files from the streaming assets.
    /// It checks if the folder already exists before creating it to avoid overwriting any existing data
    /// </summary>
    void InitializeWorkingFiles()
    {
        // 3. Create the player's working folder
        Directory.CreateDirectory(gameDataPath);
        Debug.Log("Created writable folder at: " + gameDataPath);
        string sourceFile = Path.Combine(streamingAssetsPath, "puzzle.txt");
        string destFile = Path.Combine(gameDataPath, "puzzle.txt");

        #if UNITY_ANDROID || UNITY_WEBGL
            // Special handling needed for compressed Android/WebGL files
            // Use UnityWebRequest to fetch the file contents first
            // (Omitted for brevity, but critical for these platforms)
        #else
            File.Copy(sourceFile, destFile, true);
        #endif
    }
}
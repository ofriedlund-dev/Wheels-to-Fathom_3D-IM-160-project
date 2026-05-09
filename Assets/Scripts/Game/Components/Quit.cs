/*****************************************************************************
// File Name : Quit.cs
// Author : Owen M. Friedlund
// Creation Date : April 15, 2026
//
// Brief Description : This is a document that quits the game in the editor and game
*****************************************************************************/
using UnityEngine;

public class Quit : MonoBehaviour
{
    /// <summary>
    /// This function is called when the quit button is pressed and it quits the game in the editor and game
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

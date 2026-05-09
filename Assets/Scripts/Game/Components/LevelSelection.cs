/*****************************************************************************
// File Name : Level Selection.cs
// Author : Owen M. Friedlund
// Creation Date : April 16, 2026
//
// Brief Description : This is a document that loads the scene of the desired level
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    /// <summary>
    /// loads the scene based on the levelIndex
    /// </summary>
    /// <param name="levelIndex"></param>
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}

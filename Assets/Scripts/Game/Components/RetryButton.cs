/*****************************************************************************
// File Name : RetryButton.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that executes the retry function from the game manager
*****************************************************************************/
using UnityEngine;

public class RetryButton : MonoBehaviour
{
    /// <summary>
    /// This function is called when the retry button is pressed and it calls the retry function from the game manager
    /// to reload the current scene
    /// </summary>
    public void RetryFunction()
    {
        if (GameManager.gmInstance != null)
        {
            //Reloads the the current scene
            GameManager.gmInstance.Retry();
        }
    }
}

/*****************************************************************************
 // File Name : Tutorial Screen.cs
 // Author : Owen M. Friedlund
 // Creation Date : May 9, 2026
 //
 // Brief Description : This is a document that holds the code for the TutorialScreen class,
 // which is used to handle the tutorial screen that appears at the beginning of the game
 *****************************************************************************/
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    /// <summary>
    /// This function is called when the close button on the tutorial screen is pressed,
    /// and it sets the tutorial screen to inactive to allow the player to start the game
    /// </summary>
    public void CloseTutorialScreen()
    {
        GameManager.gmInstance.TutorialScreen.SetActive(false);
    }
    /// <summary>
    /// This function is called when the tutorial button is pressed on the menu screen,
    /// and it sets the tutorial screen to active to allow the player to view the tutorial information
    /// </summary>
    public void OpenTutorialScreen()
    {
        GameManager.gmInstance.TutorialScreen.SetActive(true);
    }
}

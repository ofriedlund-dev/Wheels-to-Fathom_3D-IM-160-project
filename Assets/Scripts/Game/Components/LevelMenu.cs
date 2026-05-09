/*****************************************************************************
// File Name : LevelMenu.cs
// Author : Owen M. Friedlund
// Creation Date : April 15, 2026
//
// Brief Description : This is a document that switches the menu states based on the index provided
*****************************************************************************/
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    /// <summary>
    /// Sets the GameManager's MenuIndex to the menuIndex
    /// </summary>
    /// <param name="menuIndex"></param>
    public void SwitchMenu(int menuIndex)
    {
        GameManager.gmInstance.MenuIndex = menuIndex;
    }
}

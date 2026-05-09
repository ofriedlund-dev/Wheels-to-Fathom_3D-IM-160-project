/*****************************************************************************
// File Name : Lock.cs
// Author : Owen M. Friedlund
// Creation Date : April 28, 2026
//
// Brief Description : This is a document that holds the code for the Lock class,
// which represents the lock that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class Lock : MonoBehaviour
{
    /// <summary>
    /// This function is called every frame, and it checks if the player has found the key,
    /// and if they have, it destroys the lock object to allow the player to progress
    /// </summary>
    void Update()
    {
        if (GameManager.gmInstance.KeyFound)
        {
            Destroy(this.gameObject);
        }
    }
}

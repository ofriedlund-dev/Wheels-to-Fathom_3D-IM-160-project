/*****************************************************************************
// File Name : Garage.cs
// Author : Owen M. Friedlund
// Creation Date : April 15, 2026
//
// Brief Description : This is a document that holds the code for the Garage class,
// which is used to control the behavior of the garage door in the game
*****************************************************************************/
using UnityEngine;

public class Garage : MonoBehaviour
{
    /// <summary>
    /// This function is called when another collider stays within the trigger collider of the garage door
    /// and it checks if the player has found the pin and if there are no keys left
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameManager.gmInstance.PinFound && GameObject.FindGameObjectsWithTag("Key").Length == 0)
        {
            Debug.Log("Garage Door Opened");
            GameManager.gmInstance.PlayerWon = true;
        }
    }
}

/*****************************************************************************
// File Name : Gate.cs
// Author : Owen M. Friedlund
// Creation Date : April 28, 2026
//
// Brief Description : This is a document that holds the code for the Gate class,
// which is used to control the behavior of the gate in the game
*****************************************************************************/
using UnityEngine;

public class Gate : MonoBehaviour
{
    /// <summary>
    /// This function is called when another collider enters the trigger collider of the gate
    /// and it checks if the player has found the key and if so, it moves the gate up to open it
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.gmInstance.KeyFound)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 19, transform.position.z);
            }
        }
    }
    /// <summary>
    /// This function is called when another collider exits the trigger collider of the gate
    /// and it checks if the player has found the key and if so, it moves the gate down to close it
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.gmInstance.KeyFound)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 19, transform.position.z);
            }
        }
    }
}

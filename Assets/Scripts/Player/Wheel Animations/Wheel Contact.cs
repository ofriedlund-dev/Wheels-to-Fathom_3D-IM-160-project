/*****************************************************************************
// File Name : Wheel Contact.cs
// Author : Owen M. Friedlund
// Creation Date : April 14, 2026
//
// Brief Description : This is a document that holds the code for the WheelContact class,
// which is used to handle collision detection for the player's wheels, to determine if the player is grounded or not
*****************************************************************************/
using UnityEngine;

public class WheelContact : MonoBehaviour
{
    private PlayerController playerController;
    /// <summary>
    /// This function is called when the player's wheel collides with the ground,
    /// and it sets the player's grounded state to true
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController = GetComponentInParent<PlayerController>();
            playerController.Grounded = true;
        }
    }

    /// <summary>
    /// This function is called when the player's wheel stops colliding with the ground,
    /// and it sets the player's grounded state to false
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController = GetComponentInParent<PlayerController>();
            playerController.Grounded = false;
        }
    }
}

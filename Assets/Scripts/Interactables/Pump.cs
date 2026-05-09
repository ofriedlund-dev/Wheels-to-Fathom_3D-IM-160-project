/*****************************************************************************
// File Name : Pump.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that holds the code for the Pump class,
// which represents the pump that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class Pump : BaseInteractable
{
    public Pump() : base(true) { }
    /// <summary>
    /// This function is called when the player collides with the pump object,
    /// and it checks if the colliding object is the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
}

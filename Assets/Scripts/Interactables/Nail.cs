/*****************************************************************************
// File Name : Nail.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that holds the code for the Nail class,
// which represents the nail that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class Nail : BaseInteractable
{
    public Nail() : base(true) { }
    /// <summary>
    /// This function is called when the player collides with the nail object,
    /// and it checks if the colliding object is the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
}

/*****************************************************************************
// File Name : Spare Tires.cs
// Author : Owen M. Friedlund
// Creation Date : April 7, 2026
//
// Brief Description : This is a document that holds the code for the SpareTires class,
// which represents the spare tires that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class SpareTires : BaseInteractable
{
    public SpareTires() : base(true) { }
    /// <summary>
    /// This function is called when the player collides with the spare tires object,
    /// and it checks if the colliding object is the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.gmInstance.AddLife();
            Use();
        }
    }
}

/*****************************************************************************
// File Name : Key.cs
// Author : Owen M. Friedlund
// Creation Date : April 28, 2026
//
// Brief Description : This is a document that holds the code for the Key class,
// which represents the key that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class Key : BaseInteractable
{
    public Key() : base(true) { }
    /// <summary>
    /// This function is called when the player collides with the key object,
    /// and it checks if the colliding object is the player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.gmInstance.KeyFound = true;
            Use();
        }
    }
    /// <summary>
    /// This function is called every frame, and it rotates the key object around the y-axis
    /// to make it visually appealing
    /// </summary>
    void Update()
    {
        this.transform.Rotate(0, 0.5f, 0);
    }
}

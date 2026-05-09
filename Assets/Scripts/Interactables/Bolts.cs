/*****************************************************************************
// File Name : Bolts.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that holds the code for the Bolts class,
// which represents the bolts that the player can collect in the game
*****************************************************************************/
using UnityEngine;

public class Bolts : BaseInteractable
{
    private bool collected = false;
    public Bolts() : base(true) {}
    public Bolts(bool dropped) : base(true) {}
    /// <summary>
    /// uses the object when the player collides
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
    /// <summary>
    /// This function is called when the player interacts with the bolts object,
    /// and it checks if the bolts have already been collected or not, and if not, 
    /// it increments the bolts count in the GameManager and destroys the bolts object,
    /// and if the bolts have already been collected,
    /// it just destroys the bolts object without incrementing the bolts count
    /// </summary>
    protected override void Use()
    {
        if (OneUse && !collected)
        {
            GameManager.gmInstance.Bolts++;
            collected = true;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

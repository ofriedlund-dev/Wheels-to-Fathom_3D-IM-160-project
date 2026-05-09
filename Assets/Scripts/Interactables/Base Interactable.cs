/*****************************************************************************
// File Name : Base Interactable.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that holds the code for the BaseInteractable class,
// which serves as a base class for all interactable objects in the game
*****************************************************************************/
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    private bool oneUse = true;
    public bool OneUse { get { return oneUse; } set { oneUse = value; } }
    public BaseInteractable() { }
    public BaseInteractable(bool value) { oneUse = value; }
    /// <summary>
    /// This function is called when the player interacts with the object,
    /// and it executes the interaction logic for that object
    /// </summary>
    protected virtual void Use()
    {
        if (oneUse)
        {
            Destroy(this.gameObject);
        }
    }
}

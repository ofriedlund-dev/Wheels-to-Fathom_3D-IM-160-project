/*****************************************************************************
// File Name : Wheel Jack.cs
// Author : Owen M. Friedlund
// Creation Date : February 7, 2026
//
// Brief Description : This is a document that makes the camera follow the player
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelJack : MonoBehaviour
{
    private InputAction launchAction; // Input action for launching the player
    [SerializeField] private bool isLaunching = false; // Flag to track if the player is currently being launched
    [SerializeField] private float launchForce = 250000f; // Force applied to the player when they collide with the wheel jack
    /// <summary>
    /// Sets up the launch action and assigns the performed and canceled methods
    /// </summary>
    void Start()
    {
        launchAction = InputSystem.actions.FindAction("Interact");
        launchAction.performed += LaunchActionPerformed;
        launchAction.canceled += LaunchActionCanceled;
    }

    /// <summary>
    /// Sets is launching to false
    /// </summary>
    /// <param name="context"></param>
    private void LaunchActionCanceled(InputAction.CallbackContext context)
    {
        isLaunching = false;
    }

    /// <summary>
    /// Sets is launching to true
    /// </summary>
    /// <param name="context"></param>
    private void LaunchActionPerformed(InputAction.CallbackContext context)
    //public void OnInteract()
    {
        isLaunching = true;
    }

    /// <summary>
    /// Checks to see if the player is touching, and if the player is and the isLaunching is true,
    /// launches them in the air
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is colliding with the wheel jack.");
            Debug.Log("Is launching: " + isLaunching);
            if (isLaunching)
            {
                // Get the Rigidbody component of the player
                Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    Debug.Log("Applying launch force to the player.");
                    playerRigidbody.AddForce(new Vector3(0, launchForce, 0), ForceMode.Impulse);
                }
                GetComponent<Animator>().SetBool("LaunchPlease", isLaunching);
                isLaunching = false; // Reset the launching flag after applying the force
                GetComponent<Animator>().SetBool("LaunchPlease", isLaunching);
            }
        }
    }
    void OnDestroy()
    {
        launchAction.performed -= LaunchActionPerformed;
        launchAction.canceled -= LaunchActionCanceled;
    }
}

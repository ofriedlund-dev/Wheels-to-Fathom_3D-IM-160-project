/*****************************************************************************
// File Name : CheckPoimt.cs
// Author : Owen M. Friedlund
// Creation Date : February 12, 2026
//
// Brief Description : This is a document that sets up the basic checkpoint system in each level
*****************************************************************************/
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated;
    public bool IsActivated { get { return isActivated; } set { isActivated = value; } }
    /// <summary>
    /// Sets the isActivated to false and the light color to yellow
    /// </summary>
    void Start()
    {
        isActivated = false;
        GetComponent<Light>().color = Color.yellow; // Set the checkpoint's light color to red when not activated
    }

    /// <summary>
    /// Checks if the player is touching it and if it is sets the checkpoint to active and changes the respawn point
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = true;
            Debug.Log("Checkpoint activated!");
            GameManager.gmInstance.spawnPoint.transform.position = transform.position;
            GetComponent<Light>().color = Color.green; // Change the checkpoint's light color to green when activated
        }
    }
}

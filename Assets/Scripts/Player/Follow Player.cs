/*****************************************************************************
// File Name : Follow Player.cs
// Author : Owen M. Friedlund
// Creation Date : February 12, 2026
//
// Brief Description : This is a document that holds the code for the FollowPlayer class,
// which is used to control the camera's position and rotation relative to the player
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float height = 5f;
    private float distance = 15f;
    private float rotationSpeed = 5f;
    private float rotationAngle;
    private float heightAngle = 20f;
    private InputAction moveCamera;
    private InputAction zoomOut;
    private InputAction zoomIn;
    private bool camIsControlled = false;
    /// <summary>
    /// This is the start function for the FollowPlayer script, which sets up the input actions for controlling the camera
    /// and initializes the camera's position and rotation based on the player's position
    /// </summary>
    void Start()
    {
        moveCamera = InputSystem.actions.FindAction("Move Camera");
        moveCamera.performed += MoveCameraPerformed;
        moveCamera.canceled += MoveCameraCanceled;
        zoomOut = InputSystem.actions.FindAction("Zoom Out");
        zoomOut.performed += ZoomOutPerformed;
        zoomIn = InputSystem.actions.FindAction("Zoom In");
        zoomIn.performed += ZoomInPerformed;

    }
    /// <summary>
    /// This function is called when the zoom in action is performed
    /// and it decreases the distance from the player while clamping it to a minimum value
    /// </summary>
    /// <param name="context"></param>
    private void ZoomInPerformed(InputAction.CallbackContext context)
    {
        distance -= 1f;
        distance = Mathf.Clamp(distance, 0f, 15f);
    }
    /// <summary>
    /// This function is called when the zoom out action is performed
    /// and it increases the distance from the player while clamping it to a maximum value
    /// </summary>
    /// <param name="context"></param>
    private void ZoomOutPerformed(InputAction.CallbackContext context)
    {
        distance += 1f;
        distance = Mathf.Clamp(distance, 0f, 15f);
    }
    /// <summary>
    /// This function is called when the move camera action is canceled
    /// and it sets the camIsControlled flag to false to stop the camera from being controlled
    /// </summary>
    /// <param name="context"></param>
    private void MoveCameraCanceled(InputAction.CallbackContext context)
    {
        camIsControlled = false;
    }
    /// <summary>
    /// This function is called when the move camera action is performed
    /// and it sets the camIsControlled flag to true to allow the camera to be controlled
    /// </summary>
    /// <param name="context"></param>
    private void MoveCameraPerformed(InputAction.CallbackContext context)
    {
        camIsControlled = true;
    }

    /// <summary>
    /// This is the LateUpdate function for the FollowPlayer script, which updates the camera's position
    /// and rotation based on the player's position and the current rotation angles
    /// and allows the player to control the camera's rotation when the camIsControlled flag is true
    /// </summary>
    void LateUpdate()
    {
        if (camIsControlled)
        {
            rotationAngle += Input.GetAxis("Mouse X") * rotationSpeed;
            heightAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
            if (distance > 0f)
            {
                heightAngle = Mathf.Clamp(heightAngle, 0f, 80f);
            }
        }
        float combinedRotation = player.transform.eulerAngles.y + rotationAngle;
        Quaternion rotation = Quaternion.Euler(heightAngle, combinedRotation, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + player.transform.position;
        transform.position = position;
        transform.rotation = rotation;
    }
}
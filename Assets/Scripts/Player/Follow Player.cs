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
    private bool camIsControlled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveCamera = InputSystem.actions.FindAction("Move Camera");
        moveCamera.performed += MoveCameraPerformed;
        moveCamera.canceled += MoveCameraCanceled;
    }

    private void MoveCameraCanceled(InputAction.CallbackContext context)
    {
        camIsControlled = false;
    }

    private void MoveCameraPerformed(InputAction.CallbackContext context)
    {
        camIsControlled = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (camIsControlled)
        {
            rotationAngle += Input.GetAxis("Mouse X") * rotationSpeed;
            heightAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
            heightAngle = Mathf.Clamp(heightAngle, 5f, 60f);
        }
        float combinedRotation = player.transform.eulerAngles.y + rotationAngle;
        Quaternion rotation = Quaternion.Euler(heightAngle, combinedRotation, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + player.transform.position;
        transform.position = position;
        transform.rotation = rotation;
    }
}
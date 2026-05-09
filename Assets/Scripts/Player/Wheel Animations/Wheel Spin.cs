/*****************************************************************************
// File Name : Wheel Spin.cs
// Author : Owen M. Friedlund
// Creation Date : February 13, 2026
//
// Brief Description : This is a document that holds the code for the WheelSpin class,
// which is used to handle the spinning animation of the player's wheels
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WheelSpin : MonoBehaviour
{
    private InputAction howardMove;
    private float forwardValue;
    private float turnValue;
    [SerializeField] private float spinSpeed;
    [SerializeField] bool isLeft;

    /// <summary>
    /// This function is called when the script is first initialized,
    /// and it sets up the input actions for the player's movement,
    /// and subscribes to the scene unload event to clean up the input actions when the scene is unloaded
    /// </summary>
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        howardMove = InputSystem.actions.FindAction("Move");
        howardMove.performed += HowardMovePerformed;
        howardMove.canceled += HowardMoveCanceled;
    }
    /// <summary>
    /// This function is called when the scene is unloaded,
    /// and it unsubscribes from the input actions to prevent memory leaks
    /// and unintended behavior when the scene is reloaded
    /// </summary>
    /// <param name="arg0"></param>
    private void OnSceneUnloaded(Scene arg0)
    {
        howardMove.performed -= HowardMovePerformed;
        howardMove.canceled -= HowardMoveCanceled;
    }

    /// <summary>
    /// This function is called when the move action is performed,
    /// and it updates the forward and turn values based on the input from the player
    /// </summary>
    /// <param name="context"></param>
    private void HowardMoveCanceled(InputAction.CallbackContext context)
    {
        forwardValue = 0.0f;
        turnValue = 0.0f;
    }

    /// <summary>
    /// This function is called when the move action is performed,
    /// and it updates the forward and turn values based on the input from the player
    /// </summary>
    /// <param name="context"></param>
    private void HowardMovePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove)
        {
            forwardValue = context.ReadValue<Vector2>().y;
            turnValue = context.ReadValue<Vector2>().x;
        }
    }

    /// <summary>
    /// This function is called every frame, and it rotates the wheel based on the forward and turn values,
    /// and it uses the spinSpeed variable to control how fast the wheel spins,
    /// and it also checks if the wheel is a left or right wheel to determine the direction of the spin
    /// </summary>
    void Update()
    {
        if (turnValue == 0)
        {
            transform.Rotate(0,0,spinSpeed * -forwardValue * Time.deltaTime);
        }
        else if (isLeft)
        {
            transform.Rotate(0,0,spinSpeed * -turnValue * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0,0,spinSpeed * turnValue * Time.deltaTime);
        }
    }
}

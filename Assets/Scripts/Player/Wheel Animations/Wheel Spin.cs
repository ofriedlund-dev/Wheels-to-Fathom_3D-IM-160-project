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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        howardMove = InputSystem.actions.FindAction("Move");
        howardMove.performed += HowardMovePerformed;
        howardMove.canceled += HowardMoveCanceled;
    }
    private void OnSceneUnloaded(Scene arg0)
    {
        howardMove.performed -= HowardMovePerformed;
        howardMove.canceled -= HowardMoveCanceled;
    }

    private void HowardMoveCanceled(InputAction.CallbackContext context)
    {
        forwardValue = 0.0f;
        turnValue = 0.0f;
    }

    private void HowardMovePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove)
        {
            forwardValue = context.ReadValue<Vector2>().y;
            turnValue = context.ReadValue<Vector2>().x;
        }
    }

    // Update is called once per frame
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

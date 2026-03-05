using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData howardData;
    private InputAction howardMove;
    private InputAction howardTurbo;
    private InputAction retry;
    private InputAction rotationReset;
    private Rigidbody howardRB;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float airLeakRate;
    private float forwardValue;
    private float turnValue;
    private float sprintValue;
    private bool isMoving;
    private bool isSprinting;
    private float lastFillPercentage = 100;
    private float lastDullPercentage = 0;
    private bool grounded = false;
    /// <summary>
    /// Sets up the moveand sprint actions
    /// </summary>
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        howardRB = GetComponent<Rigidbody>();
        howardMove = InputSystem.actions.FindAction("Move");
        howardMove.performed += HowardMovePerformed;
        howardMove.canceled += HowardMoveCanceled;
        howardTurbo = InputSystem.actions.FindAction("Sprint");
        howardTurbo.performed += HowardTurboPerformed;
        howardTurbo.canceled += HowardTurboCanceled;
        retry = InputSystem.actions.FindAction("Retry");
        retry.performed += RetryPerformed;
        rotationReset = InputSystem.actions.FindAction("Rotation Reset");
        rotationReset.performed += RotationResetetryPerformed;
        isMoving = false;
        isSprinting = false;
        sprintValue = 1.0f;
        if (GetComponents<PlayerData>() != null)
        {
            howardData = GetComponent<PlayerData>();
        }
        else
        {
            Debug.LogError("Howard does not have his data =(.");
        }
    }

    private void RotationResetetryPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove) gameObject.transform.SetPositionAndRotation(gameObject.transform.position, new Quaternion(0, transform.rotation.y, 0, transform.rotation.w));
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        howardMove.performed -= HowardMovePerformed;
        howardMove.canceled -= HowardMoveCanceled;
        howardTurbo.performed -= HowardTurboPerformed;
        howardTurbo.canceled -= HowardTurboCanceled;
        retry.performed -= RetryPerformed;
    }

    private void RetryPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove)
        {
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
            GameManager.gmInstance.LoseLife();
            howardData.Holes = 0;
            howardData.Fullness = lastFillPercentage;
            howardData.Dullness = lastDullPercentage;
        }
    }

    private void HowardTurboCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
        sprintValue = 1.0f;
    }

    private void HowardTurboPerformed(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            isSprinting = true;
            sprintValue = sprintMultiplier;
        }
        else
        {
            isSprinting = false;
            sprintValue = 1.0f;
        }
    }

    private void HowardMoveCanceled(InputAction.CallbackContext context)
    {
        forwardValue = 0.0f;
        turnValue = 0.0f;
        isMoving = false;
    }

    private void HowardMovePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove)
        {
            forwardValue = context.ReadValue<Vector2>().y;
            turnValue = context.ReadValue<Vector2>().x;
            isMoving = true;
            
        }
        else
        {
            Debug.Log("You are not allowed to move at this time.");
        }
    }


    /// <summary>
    /// Moves and rotates player and updates the player data
    /// </summary>
    void Update()
    {
        Vector3 moveDirection = transform.forward * forwardValue * forwardSpeed * sprintValue;
        if (grounded)
        {
            howardRB.linearVelocity = new Vector3(moveDirection.x, howardRB.linearVelocity.y, moveDirection.z);
        }
        else
        {
            howardRB.linearVelocity = howardRB.linearVelocity;
        }
        //transform.Translate(Vector3.forward * forwardValue * forwardSpeed * Time.deltaTime * sprintValue);
        //transform.Translate(Vector3.right * horizontalValue * turnSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turnValue * turnSpeed * Time.deltaTime * sprintValue);

        //Debug.Log(howardData.DisplayPlayerInfo());
        if (isMoving)
        {
            if (isSprinting && grounded)
            {
                howardData.Dullness += 2f * Time.deltaTime;
            }
            else if (grounded)
            {
                howardData.Dullness += 0.5f * Time.deltaTime;
            }
        }
        if (howardData.Punctured)
        {
            howardData.Fullness -= airLeakRate * howardData.Holes * Time.deltaTime;
        }
        if (howardData.Fullness == 0)
        {
            GameManager.gmInstance.LoseLife();
            howardData.Holes = 0;
            howardData.Fullness = lastFillPercentage;
            howardData.Dullness = lastDullPercentage;
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Spiky Thing")
        {
            if (!howardData.Punctured)
            {
                howardData.Punctured = true;
            }
            howardData.Holes++;
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
        }
        if (collision.gameObject.tag == "Plug")
        {
            if (howardData.Punctured && howardData.Holes != 0)
            {
                howardData.Holes--;
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "Inflater")
        {
            if (howardData.Punctured || howardData.Fullness + 25 < 100)
            {
                howardData.Fullness += 25;
            }
            else if (howardData.Fullness != 100)
            {
                howardData.Fullness = 100;
            }
            else
            {
                Debug.Log("Howard Overinflated and died, RIP =(");
                GameManager.gmInstance.LoseLife();
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") grounded = true;
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") grounded = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Checkpoint")
        {
            if (!collider.gameObject.GetComponent<CheckPoint>().IsActivated)
            {
                GameManager.gmInstance.spawnPoint.transform.position = collider.gameObject.transform.position;
                GameManager.gmInstance.spawnPoint.transform.rotation = collider.gameObject.transform.rotation;
                collider.gameObject.GetComponent<CheckPoint>().IsActivated = true;
                lastFillPercentage = howardData.Fullness;
                lastDullPercentage = howardData.Dullness;
                Debug.Log("New Checkpoint Set");
            }
        }
    }
}

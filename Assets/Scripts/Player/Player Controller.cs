using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData howardData;
    [SerializeField] private InputAction howardMove;
    private InputAction howardTurbo;
    private InputAction retry;
    private InputAction rotationReset;
    private InputAction launchAction; // Input action for launching the player
    private Rigidbody howardRB;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float airLeakRate;
    [SerializeField] private float launchForce = 25000f;
    [SerializeField] private bool isLaunching = false; // Flag to track if the player is currently being launched
    private float forwardValue;
    private float turnValue;
    private float sprintValue;
    private bool isMoving;
    private bool isSprinting;
    private float lastFillPercentage = 100;
    private float lastDullPercentage = 0;
    private bool grounded = false;
    public bool Grounded { get { return grounded; } set { grounded = value; } }
    /// <summary>
    /// Sets up the moveand sprint actions
    /// </summary>
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        launchAction = InputSystem.actions.FindAction("Launch");
        launchAction.performed += LaunchActionPerformed;
        launchAction.canceled += LaunchActionCanceled;
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

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Find the left and right wheel objects in the scene
    }

    private void LaunchActionPerformed(InputAction.CallbackContext context)
    {
        isLaunching = true;
    }

    private void LaunchActionCanceled(InputAction.CallbackContext context)
    {
        isLaunching = false;
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
        Debug.Log("Grounded: " + grounded);
        Vector3 moveDirection = transform.forward * forwardValue * forwardSpeed * sprintValue;
        if (grounded)
        {
            howardRB.linearVelocity = new Vector3(moveDirection.x, howardRB.linearVelocity.y, moveDirection.z);
        }
        else
        {
            howardRB.linearVelocity = new Vector3(moveDirection.x / 2, howardRB.linearVelocity.y, moveDirection.z / 2);
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
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "WheelJack") grounded = true;
        if (collider.gameObject.CompareTag("WheelJack"))
        {
            Debug.Log("Player is colliding with the wheel jack.");
            Debug.Log("Is launching: " + isLaunching);
            if (isLaunching)
            {
                Debug.Log("Applying launch force to the player.");
                howardRB.AddForce(new Vector3(0, launchForce, 0), ForceMode.Impulse);
                collider.gameObject.GetComponentInParent<Animator>().SetTrigger("launch");
                isLaunching = false; // Reset the launching flag after applying the force
            }
            //collision.gameObject.GetComponentInParent<Animator>().ResetTrigger("launch");
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "WheelJack") grounded = false;
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
    void OnDestroy()
    {
        howardMove.performed -= HowardMovePerformed;
        howardMove.canceled -= HowardMoveCanceled;
        howardTurbo.performed -= HowardTurboPerformed;
        howardTurbo.canceled -= HowardTurboCanceled;
        retry.performed -= RetryPerformed;
        rotationReset.performed -= RotationResetetryPerformed;
    }
}

/*****************************************************************************
// File Name : Player Controller.cs
// Author : Owen M. Friedlund
// Creation Date : February 12, 2026
//
// Brief Description : This is a document that holds the code for the PlayerController class
// which is used to control the player's movement and interactions in the game
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData howardData;
    [SerializeField] private InputAction howardMove;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AudioClip wheelJackSound;
    [SerializeField] private AudioClip inflateSound;
    [SerializeField] private AudioClip punctureSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    private InputAction howardTurbo;
    private InputAction retry;
    private InputAction rotationReset;
    private InputAction launchAction; // Input action for launching the player

    private InputAction quitAction; // Input action for quitting the game
    private InputAction pauseAction; // Input action for pausing the game
    private InputAction leaveLevelAction; // Input action for leaving the level
    private Rigidbody howardRB;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float airLeakRate;
    [SerializeField] private float launchForce = 25000f;
    [SerializeField] private bool isLaunching = false; // Flag to track if the player is currently being launched
    [SerializeField] private GameObject plugPrefab;
    [SerializeField] private float maxBoltDistance;
    [SerializeField] private float minBoltDistance;
    private float forwardValue;
    private float turnValue;
    private float sprintValue;
    private bool isMoving;
    private bool isSprinting;
    private float lastFillPercentage = 100;
    private float lastDullPercentage = 0;
    private bool grounded = false;
    public bool Grounded { get { return grounded; } set { grounded = value; } }
    private bool paused = false;
    /// <summary>
    /// Sets up the move and sprint actions
    /// </summary>
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        //SceneManager.sceneLoaded += OnSceneLoaded;
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
        quitAction = InputSystem.actions.FindAction("Quit");
        quitAction.performed += QuitPerformed;
        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += PausePerformed;
        leaveLevelAction = InputSystem.actions.FindAction("Leave Level");
        leaveLevelAction.performed += LeaveLevelPerformed;
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
        audioSource = GetComponent<AudioSource>();
    }

    // private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    // {
    //     // Find the left and right wheel objects in the scene
    // }

    /// <summary>
    /// Sets the launching flag to true when the launch action is performed
    /// </summary>
    /// <param name="context"></param>
    private void LaunchActionPerformed(InputAction.CallbackContext context)
    {
        isLaunching = true;
    }
    /// <summary>
    /// Sets the launching flag to false when the launch action is canceled
    /// </summary>
    /// <param name="context"></param>
    private void LaunchActionCanceled(InputAction.CallbackContext context)
    {
        isLaunching = false;
    }
    /// <summary>
    /// Resets the player's rotation on the x and z axes when the rotation reset action is performed
    /// allowing the player to correct their rotation if they get flipped upside down or something similar
    /// </summary>
    /// <param name="context"></param>
    private void RotationResetetryPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove) gameObject.transform.SetPositionAndRotation(gameObject.transform.position, new Quaternion(0, transform.rotation.y, 0, transform.rotation.w));
    }

    /// <summary>
    /// Unsubscribes from all input action events when the scene is unloaded to prevent errors and unintended behavior
    /// when the player object is destroyed
    /// </summary>
    /// <param name="arg0"></param>
    private void OnSceneUnloaded(Scene arg0)
    {
        howardMove.performed -= HowardMovePerformed;
        howardMove.canceled -= HowardMoveCanceled;
        howardTurbo.performed -= HowardTurboPerformed;
        howardTurbo.canceled -= HowardTurboCanceled;
        retry.performed -= RetryPerformed;
        quitAction.performed -= QuitPerformed;
        pauseAction.performed -= PausePerformed;
        leaveLevelAction.performed -= LeaveLevelPerformed;
    }
    /// <summary>
    /// Pauses and unpauses the game when the pause action is performed, by setting the time scale to 0 and 1 respectively
    /// </summary>
    /// <param name="context"></param>

    private void PausePerformed(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            Time.timeScale = 0f; // Pause the game
            paused = true;
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            paused = false;
        }
    }
    /// <summary>
    /// Loads the main menu scene when the leave level action is performed
    /// allowing the player to exit the level and return to the main menu
    /// </summary>
    /// <param name="context"></param>
    private void LeaveLevelPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Quits the game when the quit action is performed, and if in the editor, stops play mode
    /// </summary>
    /// <param name="context"></param>
    private void QuitPerformed(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /// <summary>
    /// Restarts the current level when the retry action is performed
    /// </summary>
    /// <param name="context"></param>
    private void RetryPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gmInstance.HowardCanMove)
        {
            GameManager.gmInstance.LoseLife();
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
            howardData.Holes = 0;
            howardData.Fullness = lastFillPercentage;
            howardData.Dullness = lastDullPercentage;
        }
    }

    /// <summary>
    /// Sets the sprinting flag to false and resets the sprint multiplier to 1 when the sprint action is canceled
    /// </summary>
    /// <param name="context"></param>
    private void HowardTurboCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
        sprintValue = 1.0f;
    }

    /// <summary>
    /// Sets the sprinting flag to true and the sprint multiplier to the specified value
    /// when the sprint action is performed
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// Sets the forward and turn values based on the input action when the move action is performed
    /// </summary>
    /// <param name="context"></param>
    private void HowardMoveCanceled(InputAction.CallbackContext context)
    {
        forwardValue = 0.0f;
        turnValue = 0.0f;
        isMoving = false;
    }
    /// <summary>
    /// Sets the forward and turn values based on the input action when the move action is performed
    /// but only if the player is allowed to move at that time according to the game manager
    /// </summary>
    /// <param name="context"></param>
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
    /// <summary>
    /// Handles collisions with various objects in the game, such as spiky things that puncture the player
    /// plugs that can fix holes, killboxes that cause the player to lose a life
    /// and inflaters that can increase the player's fullness
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Spiky Thing")
        {
            audioSource.clip = punctureSound;
            audioSource.Play();
            if (!howardData.Punctured)
            {
                howardData.Punctured = true;
            }
            howardData.Holes++;
            GameManager.gmInstance.TotalHoles++;
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
        }
        if (collision.gameObject.tag == "Plug")
        {
            audioSource.clip = itemPickupSound;
            audioSource.Play();
            if (howardData.Punctured && howardData.Holes != 0)
            {
                howardData.Holes--;
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "Bolt")
        {
            audioSource.clip = itemPickupSound;
            audioSource.Play();
        }
        if (collision.gameObject.tag == "Killbox")
        {
            audioSource.clip = deathSound;
            audioSource.Play();
            GameManager.gmInstance.LoseLife();
            howardData.Holes = 0;
            howardData.Fullness = lastFillPercentage;
            howardData.Dullness = lastDullPercentage;
            GameManager.gmInstance.TotallyNotSonicRingsMechanic();
        }
        if (collision.gameObject.tag == "Inflater")
        {

            if (howardData.Punctured || howardData.Fullness + 25 < 100)
            {
                howardData.Fullness += 25;
                audioSource.clip = inflateSound;
                audioSource.Play();
            }
            else if (howardData.Fullness != 100)
            {
                howardData.Fullness = 100;
                audioSource.clip = inflateSound;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = deathSound;
                audioSource.Play();
                Debug.Log("Howard Overinflated and died, RIP =(");
                GameManager.gmInstance.LoseLife();
            }
        }
    }

    /// <summary>
    /// Handles trigger collisions with the ground and wheel jacks to set the grounded flag
    /// and apply launch forces when necessary
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "WheelJack") grounded = true;
        if (collider.gameObject.CompareTag("WheelJack"))
        {
            Debug.Log("Player is colliding with the wheel jack.");
            Debug.Log("Is launching: " + isLaunching);
            if (isLaunching)
            {
                audioSource.clip = wheelJackSound;
                audioSource.Play();
                Debug.Log("Applying launch force to the player.");
                howardRB.AddForce(new Vector3(0, launchForce, 0), ForceMode.Impulse);
                collider.gameObject.GetComponentInParent<Animator>().SetTrigger("launch");
                isLaunching = false; // Reset the launching flag after applying the force
            }
            //collision.gameObject.GetComponentInParent<Animator>().ResetTrigger("launch");
        }
    }
    /// <summary>
    /// Handles trigger exit collisions with the ground and wheel jacks to unset the grounded flag
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "WheelJack") grounded = false;
    }
    /// <summary>
    /// Handles trigger enter collisions with checkpoints to set the spawn point
    /// </summary>
    /// <param name="collider"></param>
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

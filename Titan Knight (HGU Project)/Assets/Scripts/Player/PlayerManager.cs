/* Mark Gaskins
 * THIS SHOULD BE ATTATCHED TO THE "PLAYER CONTROLLER" OBJECT AT ALL TIMES
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    [Space(20)]
    [Header("Player Preferences")]
    [Tooltip("The walkspeed of the player")] [Range(4.0f, 12.0f)] public float speed = 6.0f;
    [Tooltip("The walkspeed multiplayer when the player's running")] [Range(0.1f, 4.5f)] public float runSpeedMultiplier = 1.5f;
    [Tooltip("The jump force")] [Range(1.0f, 12)] public float jumpPower = 8.0f;
    [Tooltip("Gravity's influence on the player")] [Range(10.0f, 50.0f)] public float gravity = 20.0f;
    [Tooltip("The speed in which the player rotates when input is recieved")] [SerializeField] [Range(300,1000)] float pModelRotSpeed = 800;
    [Space(5)]
    //[Tooltip("The amount of health that the player starts with. (default: 100)")] [SerializeField] private int startPlayerHealth = 100; // Do not use this to reference current player health.
    //[Tooltip("A non-static reference for the current Player Health. (read values only!)")] public int currentPlayerHealth;


    [Space(10)]
    [Header("Camera Preferences")]
    [Tooltip("Should the Camera FOV change when the player sprints?")] [SerializeField] public bool doSprintingFOV = true;
    [Tooltip("The sprinting FOV of the Camera. (FOV changes to this, when player sprints.)")] [Range(60, 100)] public float sprintingFOV = 90;
    [Tooltip("The standard FOV of the Camera. (FOV reverts to this, when not sprinting.)")] [Range(60, 100)] public float normalFOV = 65;
    [Tooltip("The speed in which FOV changes when the player sprints, or stops sprinting.")] [SerializeField] [Range(0, 20)] int smoothFOVSpeed = 10;

    [Space(26)]
    [Header("Player References")]
    [Tooltip("The parent object of the player model, which rotates the player model in the direction that the player is facing.")] [SerializeField] Transform pModelRotation;

    [Space(20)]
    [Header("Camera References")]
    [Tooltip("This is the player camera. It has a Fixed Position!")] [SerializeField] GameObject pCamera; // The pos/rot of this will update with the pCamera_Position.
    [Tooltip("This is the game object that updates the position and rotation of the pCamera.")] [SerializeField] GameObject pCamera_Position;

    [Space(20)]
    [Header("Cursor References")] // Handle the crosshair switching in a canvas to support animation of the crosshair.
    [Tooltip("Should the game use the dynamic cursors?\n This hides the default cursor and replaces it with one determined by the current mode.")] [SerializeField] public bool doDynamicCursors;
    [Space(8)]
    [Tooltip("Parent of the dynamic cursors.")] [SerializeField] RectTransform CursorsParent;
    [Tooltip("The default cursor. Appears in idle mode.")] [SerializeField] GameObject DefaultCursor; // (Default Cursor)
    [Tooltip("The build cursor. Appears in build mode.")] [SerializeField] GameObject BuildCursor; // (Build mode)
    [Tooltip("The crosshair cursor. Appears in combat mode.")] [SerializeField] GameObject CrosshairCursor; // (Combat mode)

    [Space(20)]
    [Header("Misc. References")]
    [Tooltip("This is currently a Screen Space canvas, but later it should be modified to be a World Space canvas near the player.")][SerializeField] Slider healthDisplay;
    [Tooltip("Later, this should be a sort of armor indicator. Currently, it is a second health display.")] [SerializeField] GameObject[] ArmorIcons;
    [SerializeField] TextMeshProUGUI healthText;

    [Space(5)]

    private CharacterController controller;

    private Health playerHealth; // This manages all player health.

    private bool isRunning;
    float verticalSpeed;

    private GameManager gm;

    public static bool isDead;

    [HideInInspector] public Vector3 movementDirection;

    private void Awake() // Assign defaults
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<Health>();
        gm = FindObjectOfType<GameManager>();

        Camera.main.fieldOfView = normalFOV;
    }

    public void Die() // Automatically called when the player dies. This is just for the proto-stage of Titan Knights.
    {
        Debug.Log($"<color=\"red\"><b>Player has died!</b></color>");

        isDead = true;
    }


    private void FixedUpdate()
    {
        HandleVisuals();

        HandleMovement();
        HandleSprinting();

        // HandleJumping();

        if (doDynamicCursors) HandleDynamicCursor();
        else // Hide the dynamic cursor if doDynamicCursors is false.
        {
            BuildCursor.SetActive(false);
            DefaultCursor.SetActive(false);
            CrosshairCursor.SetActive(false);

            Cursor.visible = true;
        }

        if (isDead)
        {
            pModelRotation.gameObject.SetActive(false);

            controller.enabled = false;
            this.enabled = false;

            StartCoroutine(ReloadScene()); // Reload the scene.
        }


        // Update temporary health display (shown for debug reasons)
        healthDisplay.value = playerHealth.currentHealth;
        healthDisplay.maxValue = playerHealth.startHealth;
        healthText.text = $"{playerHealth.currentHealth}%";

        if (playerHealth.currentHealth >= 66) ArmorIcons[0].SetActive(true); else ArmorIcons[0].SetActive(false);
        if (playerHealth.currentHealth >= 33) ArmorIcons[1].SetActive(true); else ArmorIcons[1].SetActive(false);
        if (playerHealth.currentHealth >= 10) ArmorIcons[2].SetActive(true); else ArmorIcons[2].SetActive(false);
    }

    private IEnumerator ReloadScene()  // Reloads the scene if and when the player dies.
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void HandleDynamicCursor() // Handle the crosshair switching in a canvas to support animation of the crosshair.
    {
        Cursor.visible = false;

        CursorsParent.position = Input.mousePosition;

        if (gm.currentMode == GameMode.Build) // Show the build crosshair if the current mode is build mode
        {
            BuildCursor.SetActive(true);

            DefaultCursor.SetActive(false);
            CrosshairCursor.SetActive(false);
        }
        else if (gm.currentMode == GameMode.Idle)  // Show the default (idle) crosshair if the current mode is idle mode
        {
            DefaultCursor.SetActive(true);

            BuildCursor.SetActive(false);
            CrosshairCursor.SetActive(false);
        }
        else if (gm.currentMode == GameMode.Combat)  // Show the combat crosshair if the current mode is combat mode
        {
            CrosshairCursor.SetActive(true);

            DefaultCursor.SetActive(false);
            BuildCursor.SetActive(false);
        }
    }

    private void HandleJumping()
    {
        // Check for standing on the ground
        if (controller.isGrounded)
        {
            verticalSpeed = -1; // Vertical speed is used to make the jumping smooth, instead of instantly teleporting player upwards

            //Jumping has been discontinued.
            //if (Input.GetButton("Jump"))
            //    verticalSpeed = jumpPower;

        }

        verticalSpeed -= gravity * Time.deltaTime; // Apply gravity
        movementDirection.y = verticalSpeed;

    }

    private void HandleSprinting()
    {
        // Handle sprinting 
        if (!isRunning) // Not sprinting
        {
            if (doSprintingFOV && Camera.main.fieldOfView > normalFOV) // Smoothly transition back to normal FOV, at smoothFOVSpeed speed
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, normalFOV, smoothFOVSpeed * Time.deltaTime);

            controller.Move(movementVelocity * Time.deltaTime);
        }
        else // Sprinting 
        {
            if (doSprintingFOV && Camera.main.fieldOfView < sprintingFOV) // Smoothly transition to sprinting FOV, at smoothFOVSpeed speed
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, sprintingFOV, smoothFOVSpeed * Time.deltaTime);


            controller.Move(new Vector3(
            movementDirection.x * (speed * runSpeedMultiplier),
            movementDirection.y * speed, /* Prevent the jump power from multiplying! */
            movementDirection.z * (speed * runSpeedMultiplier)) * Time.deltaTime);
        }

    }


    private Vector3 movementVelocity;
    private void HandleMovement() // The movement will be completely rescripted in order to rotate movement grid by 45°
    {

        if (Input.GetKey(KeyCode.LeftShift)) isRunning = true; else isRunning = false; // Handle sprinting with left shift


        // Make the player face the quadrant of the screen that the mouse is in
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            pModelRotation.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        // Make the player move in the direction of the pModelRotation.
        movementDirection = new Vector3(pModelRotation.transform.forward.x, pModelRotation.transform.forward.y, pModelRotation.transform.forward.z) * Input.GetAxis("Vertical");
        movementVelocity = movementDirection * speed;

    }

    private void HandleVisuals()
    {
        // Update position of player's "isometric" camera
        pCamera.transform.position = pCamera_Position.transform.position;
        pCamera.transform.rotation = pCamera_Position.transform.rotation;
    }
}

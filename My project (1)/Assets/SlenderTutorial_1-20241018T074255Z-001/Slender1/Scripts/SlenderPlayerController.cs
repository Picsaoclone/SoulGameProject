using UnityEngine;

// This will auto add the Character Controller to the GameObject if it's not already applied:
[RequireComponent(typeof(CharacterController))]
public class SlenderPlayerController : MonoBehaviour
{
    // Camera:
    public Camera playerCam;

    // Movement Settings:
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpPower = 8f;  // Adjusted for jumping
    public float gravity = 10f;

    // Camera Settings:
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;

    // Camera Zoom Settings
    public int ZoomFOV = 35;
    public int initialFOV;
    public float cameraZoomSmooth = 1;

    private bool isZoomed = false;

    // Can the player move?
    public bool canMove = true;

    // Components:
    public CharacterController characterController;

    // Sound Effects:
    public AudioSource cameraZoomSound;

    // Movement Strategy:
    private IMovementStrategy movementStrategy;

    // Camera Control Strategy:
    private ICameraControlStrategy cameraControlStrategy;

    // Camera Rotation Variables:
    private float rotationX = 0f;

    void Start()
    {
        // Ensure we are using the Character Controller component:
        characterController = GetComponent<CharacterController>();

        // Lock and hide cursor:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize movement strategy (walking by default):
        movementStrategy = new WalkingMovement(this);

        // Initialize camera control strategy (free camera by default):
        cameraControlStrategy = new FreeCameraControl(this);
    }

    void Update()
    {
        // Handle player movement:
        movementStrategy.Move();

        // Handle camera movement:
        cameraControlStrategy.ControlCamera();

        // Handle zooming:
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isZoomed = true;
            cameraZoomSound.Play();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isZoomed = false;
            cameraZoomSound.Play();
        }

        if (isZoomed)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);
        }
    }

    // Method to switch movement strategy based on user input:
    public void SwitchMovementStrategy(bool isRunning)
    {
        movementStrategy = isRunning ? new RunningMovement(this) : new WalkingMovement(this);
    }

    // Accessor for rotationX (to be used in FreeCameraControl)
    public float GetRotationX()
    {
        return rotationX;
    }

    // Mutator for rotationX (to update the vertical camera rotation)
    public void SetRotationX(float value)
    {
        rotationX = value;
    }

    // Accessor for character controller (for movement strategies)
    public CharacterController GetCharacterController()
    {
        return characterController;
    }

    // Accessor for jump power (for movement strategies)
    public float GetJumpPower()
    {
        return jumpPower;
    }
}

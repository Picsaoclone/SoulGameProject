using UnityEngine;
using UnityEngine.UI; // For UI elements
using UnityEngine.SceneManagement; // For Scene Management

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public Slider volumeSlider; // Reference to the volume slider
    public AudioSource backgroundMusic; // Reference to background music or main game audio
    public bool isPaused = false; // Track the pause state

    private SlenderPlayerController playerController; // Reference to the player controller

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuUI.SetActive(false);

        // Get reference to player controller
        playerController = FindObjectOfType<SlenderPlayerController>();

        // Initialize volume slider
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        // Toggle the pause menu when "Esc" is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Method to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume time
        isPaused = false;
        playerController.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Method to pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Stop time
        isPaused = true;
        playerController.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Method to handle volume change
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Adjust background music volume
        }
    }

    // Method for the Exit Game button
    public void ExitGame()
    {
        // Save game state here if needed
        Debug.Log("Exiting game...");
        Application.Quit(); // Exits the application
    }

    // Placeholder for Options button functionality
    public void OpenOptions()
    {
        Debug.Log("Opening options...");
        // Implement options menu functionality if required
    }
}

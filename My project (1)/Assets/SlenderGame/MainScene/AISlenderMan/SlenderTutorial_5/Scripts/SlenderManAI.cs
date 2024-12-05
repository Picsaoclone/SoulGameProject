using UnityEngine;
using UnityEngine.SceneManagement; // Import thư viện SceneManagement
using UnityEngine.UI; // Import thư viện UI để hiển thị cảnh báo
using TMPro;
public class SlenderManAI : MonoBehaviour
{
    public Transform player; // Reference to the player's GameObject
    public float teleportDistance = 10f; // Maximum teleportation distance
    public float teleportCooldown = 5f; // Time between teleportation attempts
    public float returnCooldown = 10f; // Time before returning to base spot
    [Range(0f, 1f)] public float chaseProbability = 0.65f; // Probability of chasing the player
    public float rotationSpeed = 5f; // Rotation speed when looking at the player
    public AudioClip teleportSound; // Reference to the teleport sound effect
    private AudioSource audioSource;

    public GameObject staticObject; // Reference to the "static" GameObject
    public float staticActivationRange = 5f; // Range at which "static" should be activated
    public float deathRange = 2f; // Range at which player dies (adjust as needed)
    public float cautionRange = 5f; // Range for caution warning
    public string dieSceneName = "MainMenu2"; // Name of the scene to load when player dies
    public Text cautionText; // Reference to the UI Text for the caution warning

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = teleportCooldown;

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the teleport sound
        audioSource.clip = teleportSound;

        // Ensure the "static" object is initially turned off
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        // Ensure the caution text is initially hidden
        if (cautionText != null)
        {
            cautionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot();
                teleportTimer = returnCooldown;
                returningToBase = false;
            }
            else
            {
                DecideTeleportAction();
                teleportTimer = teleportCooldown;
            }
        }

        RotateTowardsPlayer();

        // Check player distance and toggle the "static" object accordingly
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check death range and switch to die scene
        if (distanceToPlayer <= deathRange)
        {
            SceneManager.LoadScene(dieSceneName);
        }

        // Show caution text if player is within caution range
        if (distanceToPlayer <= cautionRange)
        {
            if (cautionText != null && !cautionText.gameObject.activeSelf)
            {
                cautionText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (cautionText != null && cautionText.gameObject.activeSelf)
            {
                cautionText.gameObject.SetActive(false);
            }
        }

        // Check static activation range
        if (distanceToPlayer <= staticActivationRange)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true);
            }
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false);
            }
        }
    }

    private void DecideTeleportAction()
    {
        float randomValue = Random.value;

        if (randomValue <= chaseProbability)
        {
            TeleportNearPlayer();
        }
        else
        {
            TeleportToBaseSpot();
        }
    }

    private void TeleportNearPlayer()
    {
        Vector3 randomPosition = player.position + Random.onUnitSphere * teleportDistance;
        randomPosition.y = transform.position.y; // Keep the same Y position
        transform.position = randomPosition;

        // Play the teleport sound
        audioSource.Play();
    }

    private void TeleportToBaseSpot()
    {
        transform.position = baseTeleportSpot;
        returningToBase = true;

        // Play the teleport sound
        audioSource.Play();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Ignore the vertical component

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class SlenderManAI : MonoBehaviour
{
    public Transform player;  // Reference to the player's GameObject
    public float teleportDistance = 10f;
    public float teleportCooldown = 5f;
    public float rotationSpeed = 5f;
    public float chaseProbability = 0.65f; // Chance to chase player
    public AudioClip teleportSound;
    private AudioSource audioSource;

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;

    // Q-Learning Variables
    private Dictionary<string, float[]> qTable;  // Q-table to store Q-values
    private float learningRate = 0.1f;  // Learning rate (alpha)
    private float discountFactor = 0.9f;  // Discount factor (gamma)
    private float explorationRate = 0.2f;  // Exploration rate (epsilon)

    // State variables
    private float distanceToPlayer;

    // Collision avoidance variables
    public float avoidanceRadius = 2f;  // Radius for collision avoidance

    // Prediction variables
    public float predictionTime = 1f;  // Time in seconds for prediction

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = teleportCooldown;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = teleportSound;

        // Initialize Q-table (states and actions)
        qTable = new Dictionary<string, float[]>();
    }

    private void Update()
    {
        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Get current state based on the distance to player
            string state = GetState();

            // Choose action based on Q-learning algorithm
            int action = ChooseAction(state);

            // Execute the action (teleport or pursue)
            ExecuteAction(action);

            // Receive reward based on new state
            float reward = CalculateReward();

            // Update Q-table
            UpdateQTable(state, action, reward);

            teleportTimer = teleportCooldown;
        }

        // Prediction and Pursuit
        PredictAndPursuePlayer();

        // Collision avoidance
        AvoidCollisions();

        // Rotate towards the player
        RotateTowardsPlayer();
    }

    // Get the state based on distance to the player
    private string GetState()
    {
        if (distanceToPlayer < 5f)
        {
            return "close";  // State when close to player
        }
        else if (distanceToPlayer < 15f)
        {
            return "medium"; // State when at medium distance
        }
        else
        {
            return "far";  // State when far from player
        }
    }

    // Choose action based on epsilon-greedy strategy
    private int ChooseAction(string state)
    {
        float[] actions = qTable.ContainsKey(state) ? qTable[state] : new float[3] { 0f, 0f, 0f };

        // Exploration vs. Exploitation (epsilon-greedy)
        if (Random.value < explorationRate)
        {
            return Random.Range(0, 3);  // Explore: Choose a random action
        }
        else
        {
            // Exploit: Choose the action with the highest Q-value
            return System.Array.IndexOf(actions, Mathf.Max(actions));
        }
    }

    // Execute action based on the chosen action
    private void ExecuteAction(int action)
    {
        switch (action)
        {
            case 0: // Teleport near the player
                TeleportNearPlayer();
                break;
            case 1: // Move towards the player
                PursuePlayer();
                break;
            case 2: // Return to base
                TeleportToBaseSpot();
                break;
        }
    }

    // Calculate the reward based on distance to player
    private float CalculateReward()
    {
        if (distanceToPlayer < 5f)
        {
            return 1f;  // Reward for being close to player
        }
        else if (distanceToPlayer > 15f)
        {
            return -1f; // Penalty for being too far from player
        }
        else
        {
            return 0.5f;  // Neutral reward for medium distance
        }
    }

    // Update Q-table using Q-learning formula
    private void UpdateQTable(string state, int action, float reward)
    {
        float[] actions = qTable.ContainsKey(state) ? qTable[state] : new float[3] { 0f, 0f, 0f };
        float maxFutureReward = Mathf.Max(actions);  // Get the highest Q-value for the next state

        // Q-learning formula: Q(s, a) = Q(s, a) + alpha * (reward + gamma * max_future_reward - Q(s, a))
        actions[action] = actions[action] + learningRate * (reward + discountFactor * maxFutureReward - actions[action]);

        // Update Q-table
        qTable[state] = actions;
    }

    // Teleport near the player
    private void TeleportNearPlayer()
    {
        Vector3 teleportPosition = player.position + Random.onUnitSphere * teleportDistance;
        transform.position = teleportPosition;
        audioSource.Play();
    }

    // Move towards the player
    private void PursuePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 3f);
    }

    // Teleport to base position
    private void TeleportToBaseSpot()
    {
        transform.position = baseTeleportSpot;
        audioSource.Play();
    }

    // Rotate towards the player
    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // *Prediction and Pursuit Algorithm*
    private void PredictAndPursuePlayer()
    {
        Vector3 predictedPosition = PredictPlayerPosition();
        transform.position = Vector3.MoveTowards(transform.position, predictedPosition, Time.deltaTime * 5f);
    }

    private Vector3 PredictPlayerPosition()
    {
        // Predict the player's future position based on their current velocity and prediction time
        Vector3 velocity = (player.position - player.position) / Time.deltaTime;  // Assume velocity is constant
        Vector3 predictedPosition = player.position + velocity * predictionTime;
        return predictedPosition;
    }

    // *Collision Avoidance Algorithm*
    private void AvoidCollisions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, avoidanceRadius))
        {
            // If a collision is detected in front of Slender Man, adjust path
            transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.forward, Time.deltaTime * 2f);  // Move back
        }
    }
}
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject saveMessage; // Optional UI feedback message

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int pageCount = FindObjectOfType<GameLogic>().pageCount; // Get the number of collected pages
            Vector3 playerPosition = other.transform.position;      // Get the player's position

            // Save the game
            SaveManager.SaveGame(pageCount, playerPosition);

            Debug.Log("Save triggered! Page Count: " + pageCount + ", Player Position: " + playerPosition);

            if (saveMessage != null)
            {
                saveMessage.SetActive(true);
                Invoke(nameof(HideMessage), 2f); // Hide message after 2 seconds
            }
        }
    }

    private void HideMessage()
    {
        saveMessage.SetActive(false);
    }
}

using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject counter; // UI text displaying page count
    public int pageCount;      // Number of pages collected

    private void Start()
    {
        // Attempt to load saved data
        SaveData data = SaveManager.LoadGame();
        if (data != null)
        {
            pageCount = data.pageCount;

            // Restore player position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = data.playerPosition;
            }

            Debug.Log("Game state loaded! Pages: " + pageCount + ", Position: " + data.playerPosition);
        }
        else
        {
            pageCount = 0; // Start fresh if no save file exists
            Debug.Log("No save data found. Starting a new game.");
        }
    }

    private void Update()
    {
        // Update the UI with the current page count
        counter.GetComponent<TMPro.TextMeshProUGUI>().text = pageCount + "/8";
    }
}

using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour, IPageObserver
{
    // Singleton instance
    public static GameLogic Instance { get; private set; }

    public GameObject counter; // UI text displaying page count
    public int pageCount = 0;  // Number of pages collected
    private List<IPageObserver> observers = new List<IPageObserver>(); // List of observers

    private void Awake()
    {
        // Ensure only one instance of GameLogic exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the GameLogic object persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any additional instances
        }
    }

    private void Start()
    {
        // Register as observer for CollectPages
        CollectPages[] collectPages = FindObjectsOfType<CollectPages>();
        foreach (var page in collectPages)
        {
            page.RegisterObserver(this);
        }

        // Load saved data
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
        }
        else
        {
            pageCount = 0;
        }
    }

    private void Update()
    {
        counter.GetComponent<TMPro.TextMeshProUGUI>().text = pageCount + "/8";  // Update the page count UI
    }

    // Implementation of IPageObserver
    public void OnPageCollected(int pageCount)
    {
        this.pageCount = pageCount;
        Debug.Log("Page collected! Current page count: " + this.pageCount);
    }
    public void RegisterObserver(IPageObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }
}

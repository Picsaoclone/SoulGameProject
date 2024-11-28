using UnityEngine;
using System.Collections.Generic;

public class CollectPages : MonoBehaviour
{
    public GameObject collectText;  // Text to show when in range to collect
    public AudioSource collectSound; // Sound to play when collecting

    private bool inReach = false;    // Flag to check if the player is in reach to collect
    private GameObject page;        // The page object
    private List<IPageObserver> observers = new List<IPageObserver>(); // List of observers

    void Start()
    {
        collectText.SetActive(false);
        page = this.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;
            collectText.SetActive(true); // Show collection UI when player is in range
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
            collectText.SetActive(false); // Hide collection UI when player leaves range
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("pickup"))
        {
            // Notify observers that a page has been collected
            NotifyPageCollected();

            // Play sound and disable the page
            collectSound.Play();
            collectText.SetActive(false);
            page.SetActive(false);
        }
    }

    // Register an observer (GameLogic)
    public void RegisterObserver(IPageObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    // Notify all observers when a page is collected
    private void NotifyPageCollected()
    {
        foreach (var observer in observers)
        {
            observer.OnPageCollected(GameLogic.Instance.pageCount + 1);
        }
    }
}

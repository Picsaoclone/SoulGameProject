using UnityEngine;
using System.Collections; // Đúng namespace cho IEnumerator
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour, IPageObserver
{
    public static GameLogic Instance { get; private set; }
    public GameObject counter;
    public int pageCount = 0;
    private bool isExitNotified = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CollectPages[] collectPages = FindObjectsOfType<CollectPages>();
        foreach (var page in collectPages)
        {
            page.RegisterObserver(this);
        }

        // Load saved data if any
        SaveData data = SaveManager.LoadGame();
        if (data != null)
        {
            pageCount = data.pageCount;
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
        counter.GetComponent<TMPro.TextMeshProUGUI>().text = pageCount + "/7";
    }

    public void OnPageCollected(int pageCount)
    {
        this.pageCount = pageCount;
        Debug.Log("Page collected! Current page count: " + this.pageCount);

        if (this.pageCount >= 7 && !isExitNotified)
        {
            isExitNotified = true;
            StartCoroutine(ShowExitNotification()); // Gọi Coroutine đúng cách
        }
    }

    private IEnumerator ShowExitNotification()
    {
        Debug.Log("Bạn đã thu thập đủ 7 tờ giấy! Đang chuyển cảnh...");
        counter.GetComponent<TMPro.TextMeshProUGUI>().text = "7/7";
        yield return new WaitForSeconds(0.5f); // Chờ 2 giây
        SceneManager.LoadScene("EndingScene");
    }
}

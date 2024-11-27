using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene"); // Replace "GameScene" with your actual game scene name
    }

    public void LoadGame()
    {
        if (SaveManager.SaveFileExists())
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogWarning("No save file exists!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited.");
    }
}

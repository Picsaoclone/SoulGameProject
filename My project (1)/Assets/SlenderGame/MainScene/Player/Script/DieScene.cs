using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DieScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("MainMenu2", LoadSceneMode.Single);
    }
}

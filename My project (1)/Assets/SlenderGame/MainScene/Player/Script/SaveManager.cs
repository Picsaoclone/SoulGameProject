using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int pageCount;
    public Vector3 playerPosition;
}


public static class SaveManager
{
    private static string saveFilePath = Application.persistentDataPath + "/savegame.json";

    public static void SaveGame(int pageCount, Vector3 playerPosition)
    {
        SaveData data = new SaveData
        {
            pageCount = pageCount,
            playerPosition = playerPosition
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Game Saved!");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded!");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }
}

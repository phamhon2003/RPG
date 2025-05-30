using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


public class UISceneMenu : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    public void Load()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    public void New()
    {
        DeleteAllSaves();
        DeleteAllJsonSaves();
        LoadSceneStatic.nextSceneName = "SampleScene";
        SceneManager.LoadScene("LoadingScene");

    }
    public void Exit()
    {
        LoadSceneStatic.nextSceneName = "SampleScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void DeleteAllSaves()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs deleted.");
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted: " + SavePath);
        }
        else
        {
            Debug.Log("No save file found at: " + SavePath);
        }
    }
    public void DeleteAllJsonSaves()
    {
        string dirPath = Application.persistentDataPath;
        string[] files = Directory.GetFiles(dirPath, "*.json");

        foreach (string file in files)
        {
            File.Delete(file);
            Debug.Log("Deleted file: " + file);
        }
    }
}

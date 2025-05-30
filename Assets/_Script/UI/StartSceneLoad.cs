using UnityEngine;

public class StartSceneLoad : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("NextSceneName"))
        {
            LoadSceneStatic.nextSceneName = PlayerPrefs.GetString("NextSceneName");
        }
        else
        {
            LoadSceneStatic.nextSceneName = "SampleScene";
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{   
    public Image LoadingFill;
    void Start()
    {
        if (!string.IsNullOrEmpty(LoadSceneStatic.nextSceneName))
        {
            StartCoroutine(LoadSceneAsync(LoadSceneStatic.nextSceneName));
        }
        else
        {
            Debug.LogError("No scene name to load!");
            StartCoroutine(LoadSceneAsync("SampleScene"));
        }
        
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        //while (!asyncLoad.isDone)
        //{
        //    float progress = Mathf.Clamp01(asyncLoad.progress / 4f);   
        //    LoadingFill.fillAmount = progress;
        //    yield return null;
        //}
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;
        float duration = 5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration); // chạy từ 0 đến 1 trong 5 giây
            LoadingFill.fillAmount = progress;
            yield return null;
        }

        // Cho phép scene được kích hoạt sau khi progress chạy đủ 5 giây
        asyncLoad.allowSceneActivation = true;

        // Đợi cho scene thật sự load xong (thường xảy ra ngay sau khi cho phép)
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Telepot : MonoBehaviour
{
    bool isPlayer;
    [SerializeField] GameObject Text;
    Vector3 Offset = new Vector3(0, 2,0);
    [SerializeField] string NameScene;
    [SerializeField] Vector3 PosPlayer;
    void Update()
    {
        if (isPlayer)
        {
            Text.SetActive(true);
            Text.transform.position = Camera.main.WorldToScreenPoint(transform.position + Offset);
            if (Input.GetKeyDown(KeyCode.F))
            {
                //LoadSceneStatic.PosPlayer = PosPlayer;
                LoadSceneStatic.nextSceneName = NameScene;
                Gamemanager.instance.SaveData(PosPlayer);
                SceneManager.LoadScene("LoadingScene");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayer = false;
            Text.SetActive(false);
        }
    }
}

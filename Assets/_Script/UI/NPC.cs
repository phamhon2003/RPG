using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    bool isplayer = false;   
    [SerializeField] GameObject conversation;
    public Vector3 offset = new Vector3(0.5f, 1f, 0);
    DialogueManager _DialogueManager;
    private void Start()
    {
        _DialogueManager=GetComponent<DialogueManager>();
    }
    private void Update()
    {
        if (isplayer && !UIManager.Instance.IsOpenIventoryItem) {
            conversation.SetActive(true);
            conversation.transform.position = Camera.main.WorldToScreenPoint(transform.position+offset);
            if (Input.GetKeyDown(KeyCode.E) && !_DialogueManager.isConversation)
            {
                _DialogueManager.StartDialogue();
            }
        }
        else conversation.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isplayer =true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isplayer = false;
        }
    }
}

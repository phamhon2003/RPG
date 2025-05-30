using System.Collections;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox,trade;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    public string[] lines;
    private int currentLineIndex;
    private bool isTyping = false;
    public bool isConversation=false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    public void StartDialogue()
    {
        isConversation=true;
        currentLineIndex = 0;
        dialogueBox.SetActive(true);
        StartTypingCurrentLine();
    }

    void Update()
    {
        if (dialogueBox.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                ShowFullLine();
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartTypingCurrentLine()
    {
        typingCoroutine = StartCoroutine(TypeLine(lines[currentLineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void ShowFullLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = lines[currentLineIndex];
        isTyping = false;
    }

    void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < lines.Length)
        {
            StartTypingCurrentLine();
        }
        else
        {
            isConversation = false;
            trade.SetActive(true);
        }
    }
}

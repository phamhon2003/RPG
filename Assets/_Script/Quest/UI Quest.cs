using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class UIQuest : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public RectTransform questPanel; 
    public Button toggleButton;
    public float hiddenXPosition = -1200f; 
    public float visibleXPosition = -696.5654f; 
    public float slideDuration = 0.5f;    

    private bool isVisible = true; 
    private void Start()
    {
        toggleButton.onClick.AddListener(ToggleQuestPanel);
    }
    private void Update()
    {
        UpdateQuestDisplay();
    }

    void UpdateQuestDisplay()
    {
        Quest currentQuest = QuestManager.Instance.CurrentQuest;

        if (currentQuest != null && !currentQuest.isCompleted)
        {
            questText.text = $"{currentQuest.questName} ({currentQuest.goal.currentAmount}/{currentQuest.goal.requiredAmount})";
        }
        else
        {
            questText.text = ""; 
        }
    }
    void ToggleQuestPanel()
    {
        if (isVisible)
        {
            questPanel.DOAnchorPosX(hiddenXPosition, slideDuration).SetEase(Ease.InBack);
        }
        else
        {
            questPanel.DOAnchorPosX(visibleXPosition, slideDuration).SetEase(Ease.OutBack);
        }
        isVisible = !isVisible;
    }
}

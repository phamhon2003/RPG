using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> allQuests; // List tất cả quest
    private int currentQuestIndex = 0; // Index quest đang active

    public Quest CurrentQuest
    {
        get
        {
            if (currentQuestIndex < allQuests.Count)
                return allQuests[currentQuestIndex];
            else
                return null;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        // Nếu quest hiện tại đã xong => chuyển sang quest tiếp theo
        if (CurrentQuest != null && CurrentQuest.goal.IsReached() && !CurrentQuest.isCompleted)
        {
            CompleteCurrentQuest();
        }
    }
    public void CompleteCurrentQuest()
    {
        CurrentQuest.isCompleted = true;
        GiveReward(CurrentQuest);

        currentQuestIndex++;
    }

    private void GiveReward(Quest quest)
    {
        // Ví dụ: Cộng vàng
        //PlayerInventory.Instance.AddGold(quest.rewardGold);

        // Hoặc cho thêm item
        if (quest.rewardItem != null)
        {
            InventoryManager.instance.addItem(quest.rewardItem,1);
        }
    }
    public void OnItemCollected(Item item)
    {
        
        if (CurrentQuest != null && !CurrentQuest.isCompleted)
        {
            if (item==CurrentQuest.rewardItem) 
            {
                if (CurrentQuest.goal.goalType == GoalType.Gather)
                {
                    CurrentQuest.goal.AddProgress(1);
                } 
            }
        }
    }
}

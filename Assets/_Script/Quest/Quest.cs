using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public bool isCompleted;

    public int rewardGold;
    public Item rewardItem; // Nếu game bạn có hệ thống item

    public QuestGoal goal; // Mục tiêu của quest
}
[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int requiredAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return currentAmount >= requiredAmount;
    }

    public void AddProgress(int amount)
    {
        currentAmount += amount;
    }
}

public enum GoalType
{
    Gather,    // Thu thập đồ
    Kill,      // Tiêu diệt quái
    Explore,   // Khám phá chỗ mới
}
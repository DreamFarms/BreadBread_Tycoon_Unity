using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string QuestName {  get; private set; }
    private IQuestGoal goal;
    private Reward reward;

    public Quest(string name, IQuestGoal questGoal, Reward reward)
    {
        this.QuestName = name;
        this.goal = questGoal;
        this.reward = reward;
    }

    public void Progress(params object[] args)
    {
        goal.UpdateProgress(args);

        if(goal.IsCompleted())
        {
            Debug.Log($"Quest {QuestName} Completed!");
            goal.ResetGoal();
            Debug.Log($"Okay. Next Goal set!");
        }
    }

    private void GiveReward()
    {
        Debug.Log("Reward를 제공합니다.");
        RewardManager.Instance.ApplyReward(reward);
    }

    
    public bool GetGoalProgress(out int current, out int target)
    {
        if (goal is IProgressQuestGoal progressGoal)
        {
            current = progressGoal.CurrentProgress;
            target = progressGoal.TargetProgress;
            return true;
        }

        current = 0;
        target = 0;
        return false;
    }
}

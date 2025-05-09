using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellBreadGoal : IQuestGoal, IProgressQuestGoal
{
    private int targetCount;
    private int currentCount;

    public int CurrentProgress {  get; private set; }
    public int TargetProgress { get; private set; }

    public SellBreadGoal(int initialTarget)
    {
        this.targetCount = initialTarget;
        CurrentProgress = 0;
    }
    public void UpdateProgress(params object[] args)
    {
        int soldCount = (int)args[0];
        currentCount += soldCount;
        CurrentProgress = 0;
    }

    public bool IsCompleted()
    {
        return currentCount >= targetCount;
    }


    public void ResetGoal()
    {
        currentCount = 0;
        targetCount += (targetCount * 2) + 5;
    }
}

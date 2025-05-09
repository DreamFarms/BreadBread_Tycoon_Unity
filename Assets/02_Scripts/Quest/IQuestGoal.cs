using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestGoal
{
    bool IsCompleted();
    void UpdateProgress(params object[] args);
    void ResetGoal();
}

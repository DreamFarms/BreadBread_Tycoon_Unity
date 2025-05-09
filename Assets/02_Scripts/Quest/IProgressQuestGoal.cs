using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressQuestGoal
{
    int CurrentProgress {  get; }
    int TargetProgress {  get; }
}

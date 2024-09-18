using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string questName;
    public bool isCompleted;
    public int reward;

    public Quest(string questName, int reward)
    {
        this.questName = questName;
        isCompleted = false;
        this.reward = reward;
    }
}

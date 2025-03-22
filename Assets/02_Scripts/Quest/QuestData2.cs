using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData2
{
    public string questName;
    public int[] npcId;

    public QuestData2(string questName, int[] npcId)
    {
        this.questName = questName;
        this.npcId = npcId;
    }
}

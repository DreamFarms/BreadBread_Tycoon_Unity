using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager2 : MonoBehaviour
{
    public int questId;

    Dictionary<int, QuestData2> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData2>();
        GenerateData();
    }

    private void GenerateData()
    {
        questList.Add(1, new QuestData2("시작", new int[] {1000})); // 복실이 npc의 id가 1000
    }
    
    public int GetQuestTalkIndex(int id)
    {
        return questId;
    }
}

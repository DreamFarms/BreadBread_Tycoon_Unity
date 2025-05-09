using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    private List<Quest> activeQuests = new List<Quest>();

    private void Start()
    {
        Reward reward1 = new Reward(200);
        Quest quest1 = new Quest("빵을 판매하자!", new SellBreadGoal(10), reward1);
        RegisterQuest(quest1);

        Reward reward2 = new Reward(100);
        Quest quest2 = new Quest("매출을 달성하자!", new MakeMoneyGoal(1000), reward2);
        RegisterQuest(quest2);
    }

    public void RegisterQuest(Quest quest)
    {
        activeQuests.Add(quest);
    }

    public void UpdateQuestProgress(string questName, params object[] args) // parmas 여러 값을 한번에 넘길 수 있음
    {
        Quest quest = activeQuests.Find(q => q.QuestName == questName); // p는 activeQuests 변수의 각 Quest 개체를 담는 지역변수
        quest?.Progress(args); // quest가 null이 아니면 Progress(args)를 호출
    }
}

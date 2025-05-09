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
        Quest quest1 = new Quest("���� �Ǹ�����!", new SellBreadGoal(10), reward1);
        RegisterQuest(quest1);

        Reward reward2 = new Reward(100);
        Quest quest2 = new Quest("������ �޼�����!", new MakeMoneyGoal(1000), reward2);
        RegisterQuest(quest2);
    }

    public void RegisterQuest(Quest quest)
    {
        activeQuests.Add(quest);
    }

    public void UpdateQuestProgress(string questName, params object[] args) // parmas ���� ���� �ѹ��� �ѱ� �� ����
    {
        Quest quest = activeQuests.Find(q => q.QuestName == questName); // p�� activeQuests ������ �� Quest ��ü�� ��� ��������
        quest?.Progress(args); // quest�� null�� �ƴϸ� Progress(args)�� ȣ��
    }
}

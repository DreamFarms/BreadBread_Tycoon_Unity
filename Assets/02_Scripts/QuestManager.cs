using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> quests = new List<Quest>();
    [SerializeField] private GameObject questUIPrefab;
    [SerializeField] private Transform content;
    private Stack<Quest> questStack = new Stack<Quest>();

    void Start()
    {
        // �׽�Ʈ��
        quests.Add(new Quest("ù �α��� ����", 100));
        quests[0].isCompleted = true;

        quests.Add(new Quest("ù �� �Ǹ� ����", 100));
        quests[1].isCompleted = true;

        quests.Add(new Quest("������ 10�� ã��", 150));
        quests.Add(new Quest("���� 5�� ����", 50));

        foreach (Quest quest in quests)
        {
            GameObject go = Instantiate(questUIPrefab, content);
            QuestUI questUI = go.GetComponent<QuestUI>();
            questUI.questName.text = quest.questName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

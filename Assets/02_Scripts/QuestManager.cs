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
        // 테스트용
        quests.Add(new Quest("첫 로그인 보상", 100));
        quests[0].isCompleted = true;

        quests.Add(new Quest("첫 빵 판매 보상", 100));
        quests[1].isCompleted = true;

        quests.Add(new Quest("레시피 10개 찾기", 150));
        quests.Add(new Quest("만두 5개 빗기", 50));

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

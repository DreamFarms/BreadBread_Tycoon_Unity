using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private Transform questListParent;
    [SerializeField] private QuestItem questItemPrefab;

    private Dictionary<Quest, QuestItem> questItemDic = new Dictionary<Quest, QuestItem>();

    public void RegisterQuestUI(Quest quest, Sprite backgroundSprite, Sprite questIconSprite)
    {
        QuestItem item = Instantiate(questItemPrefab, questListParent);
        item.Setup(quest, backgroundSprite, questIconSprite);
        questItemDic.Add(quest, item);
    }

    public void UpdateQuestUI(Quest quest)
    {
        if(questItemDic.TryGetValue(quest, out QuestItem item))
        {
            item.UpdateProgress();
        }
    }

    public void CompleteQuestUI(Quest quest)
    {
        if(questItemDic.TryGetValue(quest, out QuestItem item))
        {
            Destroy(item.gameObject);
            questItemDic.Remove(quest);
        }
    }
}

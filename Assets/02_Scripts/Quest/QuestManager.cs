using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static QuestManager _instance;
    public static QuestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("QuestManager");
                _instance = go.AddComponent<QuestManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // 모든 퀘스트 목록
    private Dictionary<string, Quest> _quests = new Dictionary<string, Quest>();

    // 활성화된 퀘스트 목록
    private List<Quest> _activeQuests = new List<Quest>();

    // 완료된 퀘스트 목록
    private List<Quest> _completedQuests = new List<Quest>();

    // 퀘스트 등록
    public void RegisterQuest(Quest quest)
    {
        if (!_quests.ContainsKey(quest.QuestId))
        {
            _quests.Add(quest.QuestId, quest);
            Debug.Log($"퀘스트 등록됨: {quest.Title}");
        }
        else
        {
            Debug.LogWarning($"이미 등록된 퀘스트 ID입니다: {quest.QuestId}");
        }
    }

    // 퀘스트 시작
    public void StartQuest(string questId)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            quest.StartQuest();
            if (!_activeQuests.Contains(quest))
            {
                _activeQuests.Add(quest);
            }
        }
        else
        {
            Debug.LogWarning($"존재하지 않는 퀘스트 ID입니다: {questId}");
        }
    }

    // 퀘스트 진행도 업데이트
    public void UpdateQuestProgress(string questId, object target, int amount)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            quest.UpdateProgress(target, amount);
        }
    }

    // 특정 타입의 모든 활성 퀘스트 진행도 업데이트
    public void UpdateQuestProgressByType(QuestType type, object target, int amount)
    {
        foreach (var quest in _activeQuests)
        {
            if (quest.Type == type)
            {
                quest.UpdateProgress(target, amount);
            }
        }
    }

    // 퀘스트 완료 및 보상 지급
    public void CompleteQuest(string questId)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            quest.CompleteQuest();
            if (_activeQuests.Contains(quest))
            {
                _activeQuests.Remove(quest);
                _completedQuests.Add(quest);
            }
        }
    }

    // 활성화된 모든 퀘스트 가져오기
    public List<Quest> GetActiveQuests()
    {
        return _activeQuests;
    }

    // 완료된 모든 퀘스트 가져오기
    public List<Quest> GetCompletedQuests()
    {
        return _completedQuests;
    }

    // 특정 퀘스트 가져오기
    public Quest GetQuest(string questId)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            return quest;
        }
        return null;
    }
}

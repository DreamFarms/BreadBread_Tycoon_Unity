using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
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

    // ��� ����Ʈ ���
    private Dictionary<string, Quest> _quests = new Dictionary<string, Quest>();

    // Ȱ��ȭ�� ����Ʈ ���
    private List<Quest> _activeQuests = new List<Quest>();

    // �Ϸ�� ����Ʈ ���
    private List<Quest> _completedQuests = new List<Quest>();

    // ����Ʈ ���
    public void RegisterQuest(Quest quest)
    {
        if (!_quests.ContainsKey(quest.QuestId))
        {
            _quests.Add(quest.QuestId, quest);
            Debug.Log($"����Ʈ ��ϵ�: {quest.Title}");
        }
        else
        {
            Debug.LogWarning($"�̹� ��ϵ� ����Ʈ ID�Դϴ�: {quest.QuestId}");
        }
    }

    // ����Ʈ ����
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
            Debug.LogWarning($"�������� �ʴ� ����Ʈ ID�Դϴ�: {questId}");
        }
    }

    // ����Ʈ ���൵ ������Ʈ
    public void UpdateQuestProgress(string questId, object target, int amount)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            quest.UpdateProgress(target, amount);
        }
    }

    // Ư�� Ÿ���� ��� Ȱ�� ����Ʈ ���൵ ������Ʈ
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

    // ����Ʈ �Ϸ� �� ���� ����
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

    // Ȱ��ȭ�� ��� ����Ʈ ��������
    public List<Quest> GetActiveQuests()
    {
        return _activeQuests;
    }

    // �Ϸ�� ��� ����Ʈ ��������
    public List<Quest> GetCompletedQuests()
    {
        return _completedQuests;
    }

    // Ư�� ����Ʈ ��������
    public Quest GetQuest(string questId)
    {
        if (_quests.TryGetValue(questId, out Quest quest))
        {
            return quest;
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;           // ����Ʈ UI ������
    [SerializeField] private Transform questContainer;         // ����Ʈ ����� ǥ���� �����̳�

    private Dictionary<string, QuestUIItem> _questUIItems = new Dictionary<string, QuestUIItem>();

    private void Start()
    {
        // Ȱ��ȭ�� ����Ʈ UI �ʱ�ȭ
        RefreshQuestList();
    }

    // ����Ʈ ��� ���ΰ�ħ
    public void RefreshQuestList()
    {
        // ���� UI ����
        foreach (Transform child in questContainer)
        {
            Destroy(child.gameObject);
        }
        _questUIItems.Clear();

        // Ȱ��ȭ�� ����Ʈ UI ����
        List<Quest> activeQuests = QuestManager.Instance.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            CreateQuestUIItem(quest);
        }
    }

    // ���� ����Ʈ UI ����
    private void CreateQuestUIItem(Quest quest)
    {
        GameObject questObject = Instantiate(questPrefab, questContainer);
        QuestUIItem questUIItem = questObject.GetComponent<QuestUIItem>();

        if (questUIItem != null)
        {
            questUIItem.Initialize(quest);
            _questUIItems.Add(quest.QuestId, questUIItem);

            // ����Ʈ ���� ���� �̺�Ʈ ����
            quest.OnQuestStatusChanged += OnQuestStatusChanged;
            quest.OnProgressChanged += OnQuestProgressChanged;
        }
    }

    // ����Ʈ ���� ���� �̺�Ʈ �ڵ鷯
    private void OnQuestStatusChanged(Quest quest)
    {
        if (_questUIItems.TryGetValue(quest.QuestId, out QuestUIItem questUIItem))
        {
            if (quest.Status == QuestStatus.Rewarded)
            {
                // �Ϸ�� ����Ʈ UI ����
                quest.OnQuestStatusChanged -= OnQuestStatusChanged;
                quest.OnProgressChanged -= OnQuestProgressChanged;

                Destroy(questUIItem.gameObject);
                _questUIItems.Remove(quest.QuestId);
            }
            else
            {
                // ���� ������Ʈ
                questUIItem.UpdateStatus();
            }
        }
    }

    // ����Ʈ ���൵ ���� �̺�Ʈ �ڵ鷯
    private void OnQuestProgressChanged(Quest quest)
    {
        if (_questUIItems.TryGetValue(quest.QuestId, out QuestUIItem questUIItem))
        {
            questUIItem.UpdateProgress();
        }
    }
}

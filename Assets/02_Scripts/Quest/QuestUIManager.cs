using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;           // 퀘스트 UI 프리팹
    [SerializeField] private Transform questContainer;         // 퀘스트 목록을 표시할 컨테이너

    private Dictionary<string, QuestUIItem> _questUIItems = new Dictionary<string, QuestUIItem>();

    private void Start()
    {
        // 활성화된 퀘스트 UI 초기화
        RefreshQuestList();
    }

    // 퀘스트 목록 새로고침
    public void RefreshQuestList()
    {
        // 기존 UI 삭제
        foreach (Transform child in questContainer)
        {
            Destroy(child.gameObject);
        }
        _questUIItems.Clear();

        // 활성화된 퀘스트 UI 생성
        List<Quest> activeQuests = QuestManager.Instance.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            CreateQuestUIItem(quest);
        }
    }

    // 단일 퀘스트 UI 생성
    private void CreateQuestUIItem(Quest quest)
    {
        GameObject questObject = Instantiate(questPrefab, questContainer);
        QuestUIItem questUIItem = questObject.GetComponent<QuestUIItem>();

        if (questUIItem != null)
        {
            questUIItem.Initialize(quest);
            _questUIItems.Add(quest.QuestId, questUIItem);

            // 퀘스트 상태 변경 이벤트 구독
            quest.OnQuestStatusChanged += OnQuestStatusChanged;
            quest.OnProgressChanged += OnQuestProgressChanged;
        }
    }

    // 퀘스트 상태 변경 이벤트 핸들러
    private void OnQuestStatusChanged(Quest quest)
    {
        if (_questUIItems.TryGetValue(quest.QuestId, out QuestUIItem questUIItem))
        {
            if (quest.Status == QuestStatus.Rewarded)
            {
                // 완료된 퀘스트 UI 제거
                quest.OnQuestStatusChanged -= OnQuestStatusChanged;
                quest.OnProgressChanged -= OnQuestProgressChanged;

                Destroy(questUIItem.gameObject);
                _questUIItems.Remove(quest.QuestId);
            }
            else
            {
                // 상태 업데이트
                questUIItem.UpdateStatus();
            }
        }
    }

    // 퀘스트 진행도 변경 이벤트 핸들러
    private void OnQuestProgressChanged(Quest quest)
    {
        if (_questUIItems.TryGetValue(quest.QuestId, out QuestUIItem questUIItem))
        {
            questUIItem.UpdateProgress();
        }
    }
}

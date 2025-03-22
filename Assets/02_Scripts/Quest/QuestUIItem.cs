using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIItem : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;           // 퀘스트 제목
    [SerializeField] private TMP_Text descriptionText;     // 퀘스트 설명
    [SerializeField] private TMP_Text progressText;        // 퀘스트 진행도
    [SerializeField] private Button completeButton;    // 완료 버튼

    private Quest _quest;

    // 퀘스트 UI 초기화
    public void Initialize(Quest quest)
    {
        _quest = quest;

        // titleText.text = quest.Title;
        descriptionText.text = quest.Description;
        progressText.text = quest.GetProgressText();

        // 완료 버튼 클릭 이벤트 설정
        completeButton.onClick.AddListener(OnCompleteButtonClicked);

        // 초기 상태 업데이트
        UpdateStatus();
    }

    // 퀘스트 상태 업데이트
    public void UpdateStatus()
    {
        // 퀘스트가 완료되었을 때만 버튼 활성화
        completeButton.interactable = (_quest.Status == QuestStatus.Completed);
    }

    // 퀘스트 진행도 업데이트
    public void UpdateProgress()
    {
        progressText.text = _quest.GetProgressText();
        UpdateStatus();
    }

    // 완료 버튼 클릭 이벤트 처리
    private void OnCompleteButtonClicked()
    {
        QuestManager.Instance.CompleteQuest(_quest.QuestId);
    }

    // 컴포넌트 제거 시 이벤트 리스너 해제
    private void OnDestroy()
    {
        completeButton.onClick.RemoveListener(OnCompleteButtonClicked);
    }
}

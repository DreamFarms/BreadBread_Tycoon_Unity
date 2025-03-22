using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Collection, // 수집 퀘스트
    Sale, // 판매 퀘스트
    Interaction // 상호작용 퀘스트
}

public enum QuestStatus
{
    NotStarted, // 시작되지 않음
    InProgress, // 진행 중
    Completed, // 완료됨
    Rewarded // 보상을 받음
}

public abstract class Quest : MonoBehaviour
{
    public string QuestId { get; protected set; }       // 퀘스트 고유 ID
    public string Title { get; protected set; }         // 퀘스트 제목
    public string Description { get; protected set; }   // 퀘스트 설명
    public QuestType Type { get; protected set; }       // 퀘스트 타입
    public QuestStatus Status { get; protected set; }   // 퀘스트 상태

    public int CurrentProgress { get; protected set; }  // 현재 진행도
    public int RequiredProgress { get; protected set; } // 목표 진행도

    public List<QuestReward> Rewards { get; protected set; } // 퀘스트 보상 목록

    public event Action<Quest> OnQuestStatusChanged;    // 퀘스트 상태 변경 이벤트
    public event Action<Quest> OnProgressChanged;       // 진행도 변경 이벤트

    // 생성자
    // 퀘스트 아이디, 이름, 설명, 퀘스트 타입, 목표 진행도
    protected Quest(string id, string title, string description, QuestType type, int requiredProgress)
    {
        QuestId = id;
        Title = title;
        Description = description;
        Type = type;
        RequiredProgress = requiredProgress;
        CurrentProgress = 0;
        Status = QuestStatus.NotStarted;
        Rewards = new List<QuestReward>();
    }

    // 퀘스트 시작
    public virtual void StartQuest()
    {
        if (Status == QuestStatus.NotStarted)
        {
            Status = QuestStatus.InProgress;
            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"퀘스트를 시작합니다 : {Title}");
        }
    }

    // 퀘스트 진행도 업데이트 (추상 메서드로 자식 클래스에서 구현)
    public abstract void UpdateProgress(object target, int amount);

    
    // 퀘스트 진행 상태 확인
    protected void CheckCompletion()
    {
        if (CurrentProgress >= RequiredProgress && Status == QuestStatus.InProgress)
        {
            Status = QuestStatus.Completed;
            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"퀘스트를 완료했습니다 : {Title}");
        }
    }

    // 퀘스트 완료 및 보상 지급
    public virtual void CompleteQuest()
    {
        if (Status == QuestStatus.Completed)
        {
            Status = QuestStatus.Rewarded;

            // 보상 지급
            foreach (var reward in Rewards)
            {
                reward.GiveReward();
            }

            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"퀘스트 보상 지급 완료: {Title}");
        }
        else
        {
            Debug.Log($"아직 퀘스트를 완료할 수 없습니다: {Title}, 현재 상태: {Status}");
        }
    }

    // 퀘스트 진행도 문자열 반환 (UI 표시용)
    public virtual string GetProgressText()
    {
        return $"{CurrentProgress}/{RequiredProgress}";
    }

    // 보상 추가
    public void AddReward(QuestReward reward)
    {
        Rewards.Add(reward);
    }
}

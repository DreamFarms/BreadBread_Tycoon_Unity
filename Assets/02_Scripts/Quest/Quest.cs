using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Collection, // ���� ����Ʈ
    Sale, // �Ǹ� ����Ʈ
    Interaction // ��ȣ�ۿ� ����Ʈ
}

public enum QuestStatus
{
    NotStarted, // ���۵��� ����
    InProgress, // ���� ��
    Completed, // �Ϸ��
    Rewarded // ������ ����
}

public abstract class Quest : MonoBehaviour
{
    public string QuestId { get; protected set; }       // ����Ʈ ���� ID
    public string Title { get; protected set; }         // ����Ʈ ����
    public string Description { get; protected set; }   // ����Ʈ ����
    public QuestType Type { get; protected set; }       // ����Ʈ Ÿ��
    public QuestStatus Status { get; protected set; }   // ����Ʈ ����

    public int CurrentProgress { get; protected set; }  // ���� ���൵
    public int RequiredProgress { get; protected set; } // ��ǥ ���൵

    public List<QuestReward> Rewards { get; protected set; } // ����Ʈ ���� ���

    public event Action<Quest> OnQuestStatusChanged;    // ����Ʈ ���� ���� �̺�Ʈ
    public event Action<Quest> OnProgressChanged;       // ���൵ ���� �̺�Ʈ

    // ������
    // ����Ʈ ���̵�, �̸�, ����, ����Ʈ Ÿ��, ��ǥ ���൵
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

    // ����Ʈ ����
    public virtual void StartQuest()
    {
        if (Status == QuestStatus.NotStarted)
        {
            Status = QuestStatus.InProgress;
            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"����Ʈ�� �����մϴ� : {Title}");
        }
    }

    // ����Ʈ ���൵ ������Ʈ (�߻� �޼���� �ڽ� Ŭ�������� ����)
    public abstract void UpdateProgress(object target, int amount);

    
    // ����Ʈ ���� ���� Ȯ��
    protected void CheckCompletion()
    {
        if (CurrentProgress >= RequiredProgress && Status == QuestStatus.InProgress)
        {
            Status = QuestStatus.Completed;
            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"����Ʈ�� �Ϸ��߽��ϴ� : {Title}");
        }
    }

    // ����Ʈ �Ϸ� �� ���� ����
    public virtual void CompleteQuest()
    {
        if (Status == QuestStatus.Completed)
        {
            Status = QuestStatus.Rewarded;

            // ���� ����
            foreach (var reward in Rewards)
            {
                reward.GiveReward();
            }

            OnQuestStatusChanged?.Invoke(this);
            Debug.Log($"����Ʈ ���� ���� �Ϸ�: {Title}");
        }
        else
        {
            Debug.Log($"���� ����Ʈ�� �Ϸ��� �� �����ϴ�: {Title}, ���� ����: {Status}");
        }
    }

    // ����Ʈ ���൵ ���ڿ� ��ȯ (UI ǥ�ÿ�)
    public virtual string GetProgressText()
    {
        return $"{CurrentProgress}/{RequiredProgress}";
    }

    // ���� �߰�
    public void AddReward(QuestReward reward)
    {
        Rewards.Add(reward);
    }
}

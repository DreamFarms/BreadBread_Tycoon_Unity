using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleQuest : Quest
{
    private string _targetItemId; // 수집 대상의 아이템 ID

    public SaleQuest(string id, string title, string description, string targetItemId, int requiredAmount)
        : base(id, title, description, QuestType.Collection, requiredAmount)
    {
        _targetItemId = targetItemId;
    }

    // 수집 퀘스트 진행도 업데이트
    public override void UpdateProgress(object target, int amount)
    {
        if (Status != QuestStatus.InProgress) return;

        string itemId = target as string;
        if (itemId != null && itemId == _targetItemId)
        {
            CurrentProgress += amount;
            // OnProgressChanged?.Invoke(this);
            Debug.Log($"수집 퀘스트 진행도 업데이트: {Title}, 현재: {CurrentProgress}/{RequiredProgress}");
            CheckCompletion();
        }
    }
}

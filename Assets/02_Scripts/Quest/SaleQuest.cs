using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleQuest : Quest
{
    private string _targetItemId; // ���� ����� ������ ID

    public SaleQuest(string id, string title, string description, string targetItemId, int requiredAmount)
        : base(id, title, description, QuestType.Collection, requiredAmount)
    {
        _targetItemId = targetItemId;
    }

    // ���� ����Ʈ ���൵ ������Ʈ
    public override void UpdateProgress(object target, int amount)
    {
        if (Status != QuestStatus.InProgress) return;

        string itemId = target as string;
        if (itemId != null && itemId == _targetItemId)
        {
            CurrentProgress += amount;
            // OnProgressChanged?.Invoke(this);
            Debug.Log($"���� ����Ʈ ���൵ ������Ʈ: {Title}, ����: {CurrentProgress}/{RequiredProgress}");
            CheckCompletion();
        }
    }
}

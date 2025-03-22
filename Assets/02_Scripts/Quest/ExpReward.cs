using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpReward : QuestReward
{
    public int ExpAmount { get; private set; }

    public ExpReward(string id, string name, string description, int expAmount)
        : base(id, name, description)
    {
        ExpAmount = expAmount;
    }

    // 경험치 보상 지급
    public override void GiveReward()
    {
        // 실제 게임에서는 플레이어에게 경험치 추가 로직 구현
        Debug.Log($"경험치 보상 지급: {ExpAmount}");

        // 예시: 플레이어 시스템이 있다고 가정
        // PlayerManager.Instance.AddExperience(ExpAmount);
    }
}

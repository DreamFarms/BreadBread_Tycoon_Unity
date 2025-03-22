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

    // ����ġ ���� ����
    public override void GiveReward()
    {
        // ���� ���ӿ����� �÷��̾�� ����ġ �߰� ���� ����
        Debug.Log($"����ġ ���� ����: {ExpAmount}");

        // ����: �÷��̾� �ý����� �ִٰ� ����
        // PlayerManager.Instance.AddExperience(ExpAmount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestReward
{
    public string RewardId { get; protected set; }
    public string RewardName { get; protected set; }
    public string RewardDescription { get; protected set; }

    protected QuestReward(string id, string name, string description)
    {
        RewardId = id;
        RewardName = name;
        RewardDescription = description;
    }

    // ���� ���� (�߻� �޼���)
    public abstract void GiveReward();
}

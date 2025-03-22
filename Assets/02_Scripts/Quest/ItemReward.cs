using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReward : QuestReward
{
    public string ItemId { get; private set; }
    public int Amount { get; private set; }

    public ItemReward(string id, string name, string description, string itemId, int amount)
        : base(id, name, description)
    {
        ItemId = itemId;
        Amount = amount;
    }

    public override void GiveReward()
    {
        throw new System.NotImplementedException();
    }
}

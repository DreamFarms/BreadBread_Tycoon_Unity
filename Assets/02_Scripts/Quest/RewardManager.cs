using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public void ApplyReward(Reward reward)
    {
        if(reward.Gold > 0)
        {
            // 골드를 지급하는 메서드
        }

        if(reward.Exp > 0)
        {
            // 경험치를 제공하는 메서드
        }

        if(!string.IsNullOrEmpty(reward.ItemName) && reward.ItemCount > 0)
        {
            // 아이템을 개수에 맞게 제공하는 메서드
        }
    }
}

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
            // ��带 �����ϴ� �޼���
        }

        if(reward.Exp > 0)
        {
            // ����ġ�� �����ϴ� �޼���
        }

        if(!string.IsNullOrEmpty(reward.ItemName) && reward.ItemCount > 0)
        {
            // �������� ������ �°� �����ϴ� �޼���
        }
    }
}

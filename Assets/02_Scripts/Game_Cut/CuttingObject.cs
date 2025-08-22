using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingObject : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite[] cuttingStages; // �ڸ��� �̹��� ���� (3�ܰ�)
    public string fruitName { get; private set; } // ���� �̸� ����

    [SerializeField] private Sprite[] blueberry;
    [SerializeField] private Sprite[] strawberry;
    [SerializeField] private Sprite[] banana;
    [SerializeField] private Sprite[] apple;
    [SerializeField] private Sprite[] peach;

    public void ApplyCuttingStage(int stage)
    {
        if (stage - 1 < cuttingStages.Length)
        {
            spriteRenderer.sprite = cuttingStages[stage - 1];
        }
    }

    public void SetFruit(int index)
    {
        if (index == 0)
        {
            cuttingStages = blueberry;
        }
        else if (index == 1)
        {
            cuttingStages = strawberry;
        }
        else if (index == 2)
        {
            cuttingStages = banana;
        }
        else if (index == 3)
        {
            cuttingStages = apple;
        }
        else
        {
            cuttingStages = peach;
        }
        string[] fruitNames = { "FreshBlueberry", "FreshStrawberry", "FreshBanana", "FreshApple", "FreshPeach" };
        fruitName = fruitNames[index]; // ���� �̸� ����
        spriteRenderer.sprite = cuttingStages[0];
    }

    public void Reset()
    {
        spriteRenderer.sprite = cuttingStages[0];
    }
}

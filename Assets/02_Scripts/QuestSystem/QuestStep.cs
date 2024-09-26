using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishQuestStep()
    {
        if(isFinished)
        {
            isFinished = true;

            // todo - ����Ʈ�� �Ϸ�Ǹ� ���� �� ����
            Debug.Log("������ ��� ��ҽ��ϴ�.");

            Destroy(this.gameObject);
        }
    }
}

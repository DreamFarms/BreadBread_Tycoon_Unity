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

            // todo - 퀘스트가 완료되면 진행 될 행위
            Debug.Log("우유를 모두 모았습니다.");

            Destroy(this.gameObject);
        }
    }
}

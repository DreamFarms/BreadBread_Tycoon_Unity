using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    private void Update()
    {
        // �����̽��� ������ ������ �����ϵ��� �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameEventManager.instance.ingredientEvent.MilkCollected();
        }
    }

}

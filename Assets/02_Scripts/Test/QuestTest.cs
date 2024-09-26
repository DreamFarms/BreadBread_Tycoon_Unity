using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    private void Update()
    {
        // 스페이스를 누르면 우유가 증가하도록 테스트
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameEventManager.instance.ingredientEvent.MilkCollected();
        }
    }

}

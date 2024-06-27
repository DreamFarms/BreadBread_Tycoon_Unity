using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeController : MonoBehaviour
{

    private Vector2 touchBeganPos; // 터치 시작
    private Vector2 touchEndPos; // 터치 끝
    private Vector2 touchDir; // 스와이프 방향

    public void Swipe()
    {
        // 화면을 터치하면
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                
            }
        }
    }
}

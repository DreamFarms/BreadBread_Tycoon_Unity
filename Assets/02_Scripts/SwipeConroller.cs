using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Vector2 touchBeganPos; // 터치 시작
    private Vector2 touchEndPos; // 터치 끝
    private bool isSwiping; // 스와이프 여부 확인
    [SerializeField] private float minSwipeDis; // 최소 스와이프 거리

    private void Update()
    {
        //#if UNITY_EDITOR
        //        HandleMouseInput();
        //#else
        //        Debug.Log("Iphone");
        //#endif

        HandleMouseInput();
    }

    // mouse input
    private void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            touchBeganPos = Input.mousePosition;
            isSwiping = true;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            if(isSwiping)
            {
                touchEndPos = Input.mousePosition;
                DetectSwipe();
                isSwiping = false;
            }
        }
    }

    // 스와이프 감지
    private void DetectSwipe()
    {
        Vector2 swipeDrection = touchEndPos - touchBeganPos; // 방향 = 끝 - 시작

        if(swipeDrection.magnitude > minSwipeDis)
        {
            swipeDrection.Normalize(); // 정규화

            // 좌우로 스와이프
            if(Mathf.Abs(swipeDrection.x) > Mathf.Abs(swipeDrection.y))
            {
               if(swipeDrection.x > 0) // 오른쪽
                {
                    Debug.Log("swipe right");
                    BerryPickerManager.Instance.MoveBerry(SwipeDir.RIGHT);
                }

                else // 왼쪽
                {
                    Debug.Log("swipe left");
                    BerryPickerManager.Instance.MoveBerry(SwipeDir.LEFT);
                }
            }
            // 상하로 스와이프
            else
            {
                if(swipeDrection.y > 0)
                {
                    Debug.Log("swipe up");
                }
                else
                {
                    Debug.Log("swipe down");
                }
            }
        }


    }
 }

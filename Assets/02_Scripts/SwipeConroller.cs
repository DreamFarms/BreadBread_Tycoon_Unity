using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Vector2 touchBeganPos; // ��ġ ����
    private Vector2 touchEndPos; // ��ġ ��
    private bool isSwiping; // �������� ���� Ȯ��
    [SerializeField] private float minSwipeDis; // �ּ� �������� �Ÿ�

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

    // �������� ����
    private void DetectSwipe()
    {
        Vector2 swipeDrection = touchEndPos - touchBeganPos; // ���� = �� - ����

        if(swipeDrection.magnitude > minSwipeDis)
        {
            swipeDrection.Normalize(); // ����ȭ

            // �¿�� ��������
            if(Mathf.Abs(swipeDrection.x) > Mathf.Abs(swipeDrection.y))
            {
               if(swipeDrection.x > 0) // ������
                {
                    Debug.Log("swipe right");
                    BerryPickerManager.Instance.MoveBerry(SwipeDir.RIGHT);
                }

                else // ����
                {
                    Debug.Log("swipe left");
                    BerryPickerManager.Instance.MoveBerry(SwipeDir.LEFT);
                }
            }
            // ���Ϸ� ��������
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

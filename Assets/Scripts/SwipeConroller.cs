using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeController : MonoBehaviour
{

    private Vector2 touchBeganPos; // ��ġ ����
    private Vector2 touchEndPos; // ��ġ ��
    private Vector2 touchDir; // �������� ����

    public void Swipe()
    {
        // ȭ���� ��ġ�ϸ�
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scrolling : MonoBehaviour
{
    [SerializeField] private Transform[] scrollingSprites; // ��ũ���� ��������Ʈ �迭
    [SerializeField] private bool isLeft;

    private float leftPosX; // ���� �� ��ġ
    private float rightPosX; // ������ �� ��ġ
    private float xScreenHalfSize; // ȭ���� X��ǥ ����

    private void Start()
    {
        // ȭ���� ���� ũ�� ���
        float yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect; // ���� ���� * ��Ⱦ��(����)

        if(isLeft)
        {
            // ���ʰ� ������ �� ��ġ ���
            leftPosX = -xScreenHalfSize;
            rightPosX = xScreenHalfSize * 2 * (scrollingSprites.Length - 1); // ��� ��������Ʈ�� ����� ������ �� ��ġ
        }
        else
        {
            // �����ʰ� ���� �� ��ġ ���
            rightPosX = xScreenHalfSize * 2;
            leftPosX = -xScreenHalfSize * (scrollingSprites.Length + 1);
        }
    }

    private void Update()
    {
        // ��������Ʈ �̵�
        for (int i = 0; i < scrollingSprites.Length; i++)
        {
            if(isLeft)
            {
                // ���� ���� �Ѿ�� ������ ������ �̵�
                if (scrollingSprites[i].position.x < leftPosX * 2)
                {
                    Vector3 nextPos = scrollingSprites[i].position;
                    nextPos.x += rightPosX; // ������ ������ �̵�
                    scrollingSprites[i].position = nextPos;
                }
            }    
            else
            {
                // ������ ���� �Ѿ�� ���� ������ �̵�
                if (scrollingSprites[i].position.x > rightPosX * 1.5)
                {
                    Vector3 nextPos = scrollingSprites[i].position;
                    nextPos.x += leftPosX; // ���� ������ �̵�
                    scrollingSprites[i].position = nextPos;
                }
            }
        }
    }
}

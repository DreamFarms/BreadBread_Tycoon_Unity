using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scrolling : MonoBehaviour
{
    [SerializeField] private Transform[] scrollingSprites; // 스크롤할 스프라이트 배열
    [SerializeField] private bool isLeft;

    private float leftPosX; // 왼쪽 끝 위치
    private float rightPosX; // 오른쪽 끝 위치
    private float xScreenHalfSize; // 화면의 X좌표 절반

    private void Start()
    {
        // 화면의 절반 크기 계산
        float yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect; // 세로 절반 * 종횡비(비율)

        if(isLeft)
        {
            // 왼쪽과 오른쪽 끝 위치 계산
            leftPosX = -xScreenHalfSize;
            rightPosX = xScreenHalfSize * 2 * (scrollingSprites.Length - 1); // 모든 스프라이트를 고려한 오른쪽 끝 위치
        }
        else
        {
            // 오른쪽과 왼쪽 끝 위치 계산
            rightPosX = xScreenHalfSize * 2;
            leftPosX = -xScreenHalfSize * (scrollingSprites.Length + 1);
        }
    }

    private void Update()
    {
        // 스프라이트 이동
        for (int i = 0; i < scrollingSprites.Length; i++)
        {
            if(isLeft)
            {
                // 왼쪽 끝을 넘어가면 오른쪽 끝으로 이동
                if (scrollingSprites[i].position.x < leftPosX * 2)
                {
                    Vector3 nextPos = scrollingSprites[i].position;
                    nextPos.x += rightPosX; // 오른쪽 끝으로 이동
                    scrollingSprites[i].position = nextPos;
                }
            }    
            else
            {
                // 오른쪽 끝을 넘어가면 왼쪽 끝으로 이동
                if (scrollingSprites[i].position.x > rightPosX * 1.5)
                {
                    Vector3 nextPos = scrollingSprites[i].position;
                    nextPos.x += leftPosX; // 왼쪽 끝으로 이동
                    scrollingSprites[i].position = nextPos;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkGameManager : MonoBehaviour
{
    private bool isTouchEnabled;
    [SerializeField] private float targetTime;
    private float currentTime;

    [Header("rail")]
    [SerializeField] private GameObject topRail;
    [SerializeField] private GameObject bottomRail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance; // -6.5f

    [SerializeField] private SpawnGameObject spawnGameObject; // assign

    void Update()
    {
        Timer();

        // 타이머가 끝나면
        // 터치 막고, 레일 이동, 시간 초기화
        if(currentTime >= targetTime)
        {
            isTouchEnabled = false;
            currentTime = 0;
            Debug.Log("시간을 초기화합니다");
            
            // 레일 이동
            // 이동이 끝나면 터치 활성화
            MoveRail();
        }


        // 클릭 + 레일 이동 가능하면 -> 우유 채우기
        // 타이머 끝나면 -> 터치 막고, 레일 이동
        // 레일 이동 끝나면 -> 터치 풀기
        if (Input.GetMouseButtonDown(0) && isTouchEnabled)
        {
            Debug.Log("클릭합니다.");
        }

    }

    private void Timer()
    {
        currentTime += Time.deltaTime;
    }

    private void MoveRail()
    {
        StartCoroutine(CoMoveRail());
    }

    IEnumerator CoMoveRail()
    {
        // 목표 위치를 잡고
        // 그 위치에 도달하면 반복문 탈출
        // 그리고 우유 생성
        float targetTopRailPosition = topRail.transform.position.x + moveDistance; // 목표 x 위치

        while (topRail.transform.position.x >= targetTopRailPosition) // 목표 위치에 도달하면 반복문 탈출
        {
            Vector2 currentTopPosition = topRail.transform.position;
            currentTopPosition.x -= moveSpeed * Time.deltaTime;
            topRail.transform.position = currentTopPosition;
           
            Vector2 currentBottomPosition = bottomRail.transform.position;
            currentBottomPosition.x += moveSpeed * Time.deltaTime;
            bottomRail.transform.position = currentBottomPosition;

            yield return null;
        }
        isTouchEnabled = true;
        spawnGameObject.SpawnMilkGlass();

    }

}

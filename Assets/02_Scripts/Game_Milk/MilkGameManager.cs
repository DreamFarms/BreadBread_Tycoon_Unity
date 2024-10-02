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

        // Ÿ�̸Ӱ� ������
        // ��ġ ����, ���� �̵�, �ð� �ʱ�ȭ
        if(currentTime >= targetTime)
        {
            isTouchEnabled = false;
            currentTime = 0;
            Debug.Log("�ð��� �ʱ�ȭ�մϴ�");
            
            // ���� �̵�
            // �̵��� ������ ��ġ Ȱ��ȭ
            MoveRail();
        }


        // Ŭ�� + ���� �̵� �����ϸ� -> ���� ä���
        // Ÿ�̸� ������ -> ��ġ ����, ���� �̵�
        // ���� �̵� ������ -> ��ġ Ǯ��
        if (Input.GetMouseButtonDown(0) && isTouchEnabled)
        {
            Debug.Log("Ŭ���մϴ�.");
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
        // ��ǥ ��ġ�� ���
        // �� ��ġ�� �����ϸ� �ݺ��� Ż��
        // �׸��� ���� ����
        float targetTopRailPosition = topRail.transform.position.x + moveDistance; // ��ǥ x ��ġ

        while (topRail.transform.position.x >= targetTopRailPosition) // ��ǥ ��ġ�� �����ϸ� �ݺ��� Ż��
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

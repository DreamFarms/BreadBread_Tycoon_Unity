using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkGameManager : MonoBehaviour
{
    #region instance
    private static MilkGameManager _instance;

    public static MilkGameManager Instance
    {
        get { return _instance; }
    }
    #endregion
    private bool isTouchEnabled;
    [SerializeField] private float targetTime;
    private float currentTime;

    [Header("milk")]
    [SerializeField] private MilkDrop milkDrop; // milk_drop�� milk drop.cs assign
    public MilkFill milkFill; // milk_drop�� trigger enter���� �ڵ�� assign

    [Header("rail")]
    [SerializeField] private GameObject moveGroup;
    [SerializeField] private GameObject bottomRail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance; // -6.5f

    [SerializeField] private SpawnGameObject spawnGameObject; // assign

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    void Update()
    {
        Timer();

        // Ÿ�̸Ӱ� ������
        // ��ġ ����, ���� �̵�, �ð� �ʱ�ȭ
        if(currentTime >= targetTime)
        {
            isTouchEnabled = false;
            currentTime = 0;
            
            // ���� �̵�
            // �̵��� ������ ��ġ Ȱ��ȭ
            MoveRail();

            // UI Ȱ��ȭ
            if(milkFill.IsComplete)
            {
                // ����
                MilkUIManager.Instance.ActiveO();
            }
            else
            {
                // ����
                MilkUIManager.Instance.ActiveX();
            }
        }


        // Ŭ�� + ���� �̵� �����ϸ� -> ���� ä���
        // Ÿ�̸� ������ -> ��ġ ����, ���� �̵�
        // ���� �̵� ������ -> ��ġ Ǯ��
        if (Input.GetMouseButtonDown(0) && isTouchEnabled)
        {
            milkDrop.DropMilk();
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
        float targetTopRailPosition = moveGroup.transform.position.x + moveDistance; // ��ǥ x ��ġ

        while (moveGroup.transform.position.x >= targetTopRailPosition) // ��ǥ ��ġ�� �����ϸ� �ݺ��� Ż��
        {
            Vector2 currentTopPosition = moveGroup.transform.position;
            currentTopPosition.x -= moveSpeed * Time.deltaTime;
            moveGroup.transform.position = currentTopPosition;
           
            Vector2 currentBottomPosition = bottomRail.transform.position;
            currentBottomPosition.x += moveSpeed * Time.deltaTime;
            bottomRail.transform.position = currentBottomPosition;

            yield return null;
        }
        isTouchEnabled = true;
        spawnGameObject.SpawnMilkGlass();

    }

    public void FillMilkBottle()
    {
        milkFill.FillMilk();
    }

}

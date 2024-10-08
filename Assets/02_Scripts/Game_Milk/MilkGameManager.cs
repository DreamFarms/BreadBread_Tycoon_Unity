using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private bool isStart;

    public bool IsStart
    {
        get { return isStart; }
        set { isStart = value; }
    }

    [Header("milk")]
    [SerializeField] private MilkDrop milkDrop; // milk_drop�� milk drop.cs assign
    [SerializeField] private MilkFill milkFill; // ������ : milk_bottle ���� assign, ������ : milk_drop�� trigger enter���� �ڵ�� assign

    public MilkFill MilkFill
    {
        set { milkFill = value; }
    }    

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

    private void Start()
    {
        PlayMusic();
    }

    void Update()
    {
        if(isStart)
        {
            PlayGame();
        }
    }
    public void ChangeGameState()
    {
        StartCoroutine(CoChangeGameState());   
    }

    private IEnumerator CoChangeGameState()
    {
        MilkUIManager.Instance.StartCountDown();
        yield return new WaitForSeconds(4.0f);
        isStart = !isStart;
        isTouchEnabled = true; // �̰��� ���⼭ ȣ���ϴ� ���� �´����� ���� ���...
    }

    private void PlayGame()
    {
        
        Timer();

        // Ÿ�̸Ӱ� ������
        // ��ġ ����, ���� �̵�, �ð� �ʱ�ȭ
        if (currentTime >= targetTime)
        {
            isTouchEnabled = false;
            currentTime = 0;

            // ���� �̵�
            // �̵��� ������ ��ġ Ȱ��ȭ
            MoveRail();

            // UI Ȱ��ȭ
            bool result = milkFill.GetResult();
            Debug.Log(result);
            if (result)
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

    private void PlayMusic()
    {
        AudioManager.Instance.PlayBGM(BGM.Milk);
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

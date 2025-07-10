using DG.Tweening;
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

    [Header("game")]
    [SerializeField] private float targetPlayTime; // inspector���� ����
    private float currentPlayTime;

    [Header("milk")]
    [SerializeField] private MilkDrop milkDrop; // milk_drop�� milk drop.cs assign
    [SerializeField] private MilkFill milkFill; // ������ : milk_bottle ���� assign, ������ : milk_drop�� trigger enter���� �ڵ�� assign

    [Header("Spawn")]
    [SerializeField] private GameObject milkGlassPrefab;
    [SerializeField] private Transform targetSpawnPoint;

    public MilkFill MilkFill
    {
        set { milkFill = value; }
    }    

    [Header("rail")]
    [SerializeField] private GameObject moveGroup;
    [SerializeField] private GameObject bottomRail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance; // -6.5f

    [Header("reward")]
    [SerializeField] public int currectMilkCount;
    [SerializeField] private MilkConnection milkConnection; // assign




    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        milkConnection = GameObject.Find("MilkConnection").GetComponent<MilkConnection>();
    }

    private void Start()
    {
        PlayMusic();
        StartGame();
    }

    void Update()
    {
        if(isStart)
        {
            PlayingGame();
        }

        if(currentPlayTime >= targetPlayTime && isStart)
        {
            FinishGame();
        }
    }
    public void StartGame()
    {
        MilkUIManager.Instance.InitUI();
        StartCoroutine(CoChangeGameState());   
    }

    private IEnumerator CoChangeGameState()
    {
        MilkUIManager.Instance.StartCountDown();
        yield return new WaitForSeconds(4.0f);
        isStart = !isStart; // ���� ����
        isTouchEnabled = true; // �̰��� ���⼭ ȣ���ϴ� ���� �´����� ���� ���...
    }

    private void PlayingGame()
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
            if (result)
            {
                // ����
                MilkUIManager.Instance.ActiveO();
                currectMilkCount++;
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

    private void FinishGame()
    {
        IsStart = false;
        MilkUIManager.Instance.ActiveRewardUI(currectMilkCount);
    }

    public void RequestConnection()
    {
        milkConnection.RewardSaveRequest();
    }

    private void PlayMusic()
    {
        AudioManager.Instance.PlayBGM(BGM.Milk);
    }

    private void Timer()
    {
        currentTime += Time.deltaTime;
        currentPlayTime += Time.deltaTime;
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
        SpawnMilkGlass();

    }

    public void SpawnMilkGlass()
    {
        Vector2 spawnPosition = targetSpawnPoint.position;
        GameObject go = Instantiate(milkGlassPrefab, moveGroup.transform);
        go.transform.position = spawnPosition;
    }


    public void FillMilkBottle()
    {
        milkFill.FillMilk();
    }

}

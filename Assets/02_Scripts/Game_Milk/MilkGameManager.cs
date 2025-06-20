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
    [SerializeField] private float targetPlayTime; // inspector에서 설정
    private float currentPlayTime;

    [Header("milk")]
    [SerializeField] private MilkDrop milkDrop; // milk_drop에 milk drop.cs assign
    [SerializeField] private MilkFill milkFill; // 실행전 : milk_bottle 직접 assign, 실행후 : milk_drop의 trigger enter에서 코드로 assign

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
        isStart = !isStart; // 게임 시작
        isTouchEnabled = true; // 이것을 여기서 호출하는 것이 맞는지에 대한 고민...
    }

    private void PlayingGame()
    {
        
        Timer();

        // 타이머가 끝나면
        // 터치 막고, 레일 이동, 시간 초기화
        if (currentTime >= targetTime)
        {
            isTouchEnabled = false;
            currentTime = 0;

            // 레일 이동
            // 이동이 끝나면 터치 활성화
            MoveRail();

            // UI 활성화
            bool result = milkFill.GetResult();
            if (result)
            {
                // 성공
                MilkUIManager.Instance.ActiveO();
                currectMilkCount++;
            }
            else
            {
                // 실패
                MilkUIManager.Instance.ActiveX();
            }
        }

        // 클릭 + 레일 이동 가능하면 -> 우유 채우기
        // 타이머 끝나면 -> 터치 막고, 레일 이동
        // 레일 이동 끝나면 -> 터치 풀기
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
        // 목표 위치를 잡고
        // 그 위치에 도달하면 반복문 탈출
        // 그리고 우유 생성
        float targetTopRailPosition = moveGroup.transform.position.x + moveDistance; // 목표 x 위치

        while (moveGroup.transform.position.x >= targetTopRailPosition) // 목표 위치에 도달하면 반복문 탈출
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

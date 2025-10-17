using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrillGameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TimerUI timer;            // 타이머 UI
    [SerializeField] private SubmitZone submitZone;    // 위에서 이벤트 추가한 SubmitZone
    [SerializeField] private float duration = 60f;     // 게임 시간

    [Header("Reward UI")]
    [SerializeField] private GameObject rewardPanel;   // 결과 패널(비활성 시작)
    [SerializeField] private GameObject failRewardPanel;
    [SerializeField] private TextMeshProUGUI sausageCountText; // "x N" 텍스트
    [SerializeField] private string sausageLabel = "Sausages"; // 라벨 문구
    [SerializeField] private Dictionary<string, int> grillRewardDic = new Dictionary<string, int>();

    [Header("Gameplay Lock (선택)")]
    [SerializeField] private CanvasGroup gameplayCanvas; // 끝나면 인터랙션 잠글 곳

    [Header("Connection")]
    [SerializeField] private RewardConnection cutConnection;

    private int grilledCount = 0;    //  성공 제출 누적
    private bool gameEnded = false;

    private void OnEnable()
    {
        if (submitZone) submitZone.OnSausageSubmittedSuccess += HandleSausageSubmitted;
        if (timer) timer.OnTimerFinished += HandleTimerFinished;
    }

    private void OnDisable()
    {
        if (submitZone) submitZone.OnSausageSubmittedSuccess -= HandleSausageSubmitted;
        if (timer) timer.OnTimerFinished -= HandleTimerFinished;
    }

    private void Start()
    {
        grilledCount = 0;
        gameEnded = false;
        if (rewardPanel) rewardPanel.SetActive(false);

        // 타이머 시작
        if (timer) timer.StartTimer(duration);
    }

    private void HandleSausageSubmitted()
    {
        if (gameEnded) return;
        grilledCount++;

        if (grillRewardDic.ContainsKey(sausageLabel))
            grillRewardDic[sausageLabel] += grilledCount;
        else
            grillRewardDic[sausageLabel] = grilledCount;

        Debug.Log($"{grillRewardDic} +{grilledCount}, 총 {grillRewardDic[sausageLabel]}개");
    }

    private void HandleTimerFinished()
    {
        gameEnded = true;

        // 조작 잠그기(선택)
        if (gameplayCanvas)
        {
            gameplayCanvas.interactable = false;
            gameplayCanvas.blocksRaycasts = false;
        }

        // 보상 UI 열고 개수 표시
        if (grilledCount == 0)
        {
            failRewardPanel.SetActive(true);
            return;
        }
        if (rewardPanel) rewardPanel.SetActive(true);
        if (sausageCountText) sausageCountText.text = $"{grilledCount}" + "개";
        //RequestConnection();
    }

    public void RequestConnection()
    {
        cutConnection.RewardSaveRequest(grillRewardDic);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_Grill");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Map");
    }
}

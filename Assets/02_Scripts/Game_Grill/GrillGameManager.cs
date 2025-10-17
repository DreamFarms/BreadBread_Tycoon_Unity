using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrillGameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TimerUI timer;            // Ÿ�̸� UI
    [SerializeField] private SubmitZone submitZone;    // ������ �̺�Ʈ �߰��� SubmitZone
    [SerializeField] private float duration = 60f;     // ���� �ð�

    [Header("Reward UI")]
    [SerializeField] private GameObject rewardPanel;   // ��� �г�(��Ȱ�� ����)
    [SerializeField] private GameObject failRewardPanel;
    [SerializeField] private TextMeshProUGUI sausageCountText; // "x N" �ؽ�Ʈ
    [SerializeField] private string sausageLabel = "Sausages"; // �� ����
    [SerializeField] private Dictionary<string, int> grillRewardDic = new Dictionary<string, int>();

    [Header("Gameplay Lock (����)")]
    [SerializeField] private CanvasGroup gameplayCanvas; // ������ ���ͷ��� ��� ��

    [Header("Connection")]
    [SerializeField] private RewardConnection cutConnection;

    private int grilledCount = 0;    //  ���� ���� ����
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

        // Ÿ�̸� ����
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

        Debug.Log($"{grillRewardDic} +{grilledCount}, �� {grillRewardDic[sausageLabel]}��");
    }

    private void HandleTimerFinished()
    {
        gameEnded = true;

        // ���� ��ױ�(����)
        if (gameplayCanvas)
        {
            gameplayCanvas.interactable = false;
            gameplayCanvas.blocksRaycasts = false;
        }

        // ���� UI ���� ���� ǥ��
        if (grilledCount == 0)
        {
            failRewardPanel.SetActive(true);
            return;
        }
        if (rewardPanel) rewardPanel.SetActive(true);
        if (sausageCountText) sausageCountText.text = $"{grilledCount}" + "��";
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

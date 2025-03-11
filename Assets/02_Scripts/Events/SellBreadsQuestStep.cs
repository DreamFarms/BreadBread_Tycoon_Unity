using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellBreadsQuestStep : QuestStep
{
    private int breadSold = 0;
    private int breadToComplete = 5;

    private void Start()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected += CoinCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.miscEvents.onCoinCollected -= CoinCollected;
    }

    private void CoinCollected()
    {
        if (breadSold < breadToComplete)
        {
            breadSold++;
            UpdateState();
        }

        if (breadSold < breadToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = breadSold.ToString();
        string status = "Collected " + breadSold + " / " + breadToComplete + " coins.";
        ChangeState(state, status);
    }

    protected override void SetQuestStepState(string state)
    {
        this.breadSold = System.Int32.Parse(state);
        UpdateState();
    }
}

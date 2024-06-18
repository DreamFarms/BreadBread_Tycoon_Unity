using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager instance;

    private List<Card> allCards = new List<Card>();

    private Card flippedCard; // 뒤집힌 카드 저장하는 변수

    private bool isFlipping;

    [SerializeField]
    private Slider timeoutSlider;

    [SerializeField]
    private float timeLimit = 60f;

    private float currentTime;

    [SerializeField]
    private TextMeshProUGUI timeoutText;

    [SerializeField]
    private GameObject gameoverPanel;
    private bool isGameOver;


    private int totalMatched = 10; // 10쌍의 카드 찾아야..
    private int matchedsFound = 0; // 지금까지 몇쌍의 카드를 찾았는지?

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCard();

        currentTime = timeLimit;
        SetCurrentTimeText();

        StartCoroutine("FlipAllcardsRoutine");
    }

    private void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime); // 올림
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllcardsRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(3f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f); // 카드가 뒤집히는 시간 기다려주기
        isFlipping = false;

        // 위 작업 끝낸 후 게임 시작됨
        // 게임 시작되었으니 타이머 줄이기
        yield return StartCoroutine("CountDownTimerRoutine");
    }

    IEnumerator CountDownTimerRoutine()
    {
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 다른 성능의 pc에서도 동일한 시간이 빠지게 됨
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null;
        }
        // 타임아웃 났으니 게임 오버임,,
        GameOver(false);
    }

    private void GameOver(bool success)
    {
        if(!isGameOver) // 게임 끝난 상태가 아니면
        {
            isGameOver = true; // 게임이 끝난 상태임을 업데이트
            StopCoroutine("CountDownTimerRoutine"); // 코루틴 정지시키고

            if (success)
            {

            }
            else
            {

            }

            Invoke("ShowGameOverPanel", 2.0f);
        }


    }

    void ShowGameOverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    void FlipAllCards()
    {
        foreach(Card card in allCards)
        {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card)
    {
        if(isFlipping || isGameOver)
        {
            return;
        }

        card.FlipCard(); // 카드를 선택하면 해당 카드 뒤집기

        // 이미 선택된 카드가 없으면
        // 현재 선택된 카드에 나를 저장
        if(flippedCard == null)
        {
            flippedCard = card;
        }
        else
        {
            // 카드 일치하는지 확인
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)
    {
        // 체크하는 중엔 다른 카드를 선택 불가능
        isFlipping = true;

        // 일치
        if(card1.cardId == card2.cardId)
        {
            card1.SetMatched();
            card2.SetMatched();

            matchedsFound++;
            
            if(matchedsFound == totalMatched)
            {
                GameOver(true); // 성공적으로 게임이 끝났음을 노출
            }
        }
        else
        {
            Debug.Log("different card");

            yield return new WaitForSeconds(1f);
            card1.FlipCard();
            card2.FlipCard();
            yield return new WaitForSeconds(0.5f);
        }

        isFlipping = false;

        // 카드 비교가 끝났으니
        // 뒤집힌 카드 초기화
        flippedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_Card");
    }

}

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

    private Card flippedCard; // ������ ī�� �����ϴ� ����

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


    private int totalMatched = 10; // 10���� ī�� ã�ƾ�..
    private int matchedsFound = 0; // ���ݱ��� ����� ī�带 ã�Ҵ���?

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
        int timeSec = Mathf.CeilToInt(currentTime); // �ø�
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllcardsRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(3f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f); // ī�尡 �������� �ð� ��ٷ��ֱ�
        isFlipping = false;

        // �� �۾� ���� �� ���� ���۵�
        // ���� ���۵Ǿ����� Ÿ�̸� ���̱�
        yield return StartCoroutine("CountDownTimerRoutine");
    }

    IEnumerator CountDownTimerRoutine()
    {
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime; // �ٸ� ������ pc������ ������ �ð��� ������ ��
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null;
        }
        // Ÿ�Ӿƿ� ������ ���� ������,,
        GameOver(false);
    }

    private void GameOver(bool success)
    {
        if(!isGameOver) // ���� ���� ���°� �ƴϸ�
        {
            isGameOver = true; // ������ ���� �������� ������Ʈ
            StopCoroutine("CountDownTimerRoutine"); // �ڷ�ƾ ������Ű��

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

        card.FlipCard(); // ī�带 �����ϸ� �ش� ī�� ������

        // �̹� ���õ� ī�尡 ������
        // ���� ���õ� ī�忡 ���� ����
        if(flippedCard == null)
        {
            flippedCard = card;
        }
        else
        {
            // ī�� ��ġ�ϴ��� Ȯ��
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)
    {
        // üũ�ϴ� �߿� �ٸ� ī�带 ���� �Ұ���
        isFlipping = true;

        // ��ġ
        if(card1.cardId == card2.cardId)
        {
            card1.SetMatched();
            card2.SetMatched();

            matchedsFound++;
            
            if(matchedsFound == totalMatched)
            {
                GameOver(true); // ���������� ������ �������� ����
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

        // ī�� �񱳰� ��������
        // ������ ī�� �ʱ�ȭ
        flippedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_Card");
    }

}

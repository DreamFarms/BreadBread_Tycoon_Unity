using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    private static CardGameManager _instance;

    public static CardGameManager Instance { get { return _instance; } }

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

    public List<Sprite> cardSprites = new List<Sprite>(); // ���� ��������Ʈ
    private Dictionary<Sprite, int> dicRewards = new Dictionary<Sprite, int>();
    [SerializeField] private List<Image> rewordImages = new List<Image>(); // ���� ��������Ʈ�� ��� �̹���
    [SerializeField] private List<TMP_Text> rewordCountText = new List<TMP_Text>(); // ���� ��������Ʈ ����

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
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
        GameOver(true);
    }

    private void GameOver(bool success)
    {
        if(!isGameOver) // ���� ���� ���°� �ƴϸ�
        {
            isGameOver = true; // ������ ���� �������� ������Ʈ
            StopCoroutine("CountDownTimerRoutine"); // �ڷ�ƾ ������Ű��

            if (success)
            {
                Invoke("ShowGameOverPanel", 1.0f);

            }
            else
            {

            }

        }
    }

    void ShowGameOverPanel()
    {
        SettingRewardsAtUI(); // UI�����ϰ�
        gameoverPanel.SetActive(true); // UI Ȱ��ȭ
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

            // ������ ����
            GenerateReward();


            if (matchedsFound == totalMatched)
            {
                GameOver(true); // ���������� ������ �������� ����
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            card1.FlipCard();
            card2.FlipCard();
            yield return new WaitForSeconds(0.5f);
        }

        isFlipping = false;

        // ī�� �񱳰� �������� ������ ī�� �ʱ�ȭ
        flippedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_Card");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Map");
    }

    // ������ �ϳ��� �������� �߰�
    private void GenerateReward()
    {
        int index = UnityEngine.Random.Range(0, cardSprites.Count);
        if (dicRewards.ContainsKey(cardSprites[index])) // �����尡 �����ϸ�
        {
            int value = dicRewards[cardSprites[index]];
            value++;
            dicRewards[cardSprites[index]] = value;
        }
        else // �����尡 �������� ������
        {
            dicRewards.Add(cardSprites[index], 1);
        }
    }

    // ������ ������ ������ ����
    private void SettingRewardsAtUI()
    {
        int index = 0;
        foreach(var sprite in dicRewards.Keys)
        {
            rewordImages[index].sprite = sprite;
            string s = dicRewards[sprite].ToString() + "��";
            rewordCountText[index].text = s;
            rewordImages[index].transform.parent.gameObject.SetActive(true);
            index++;
        }
    }

}

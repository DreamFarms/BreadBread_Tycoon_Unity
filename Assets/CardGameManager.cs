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

    public List<Sprite> cardSprites = new List<Sprite>(); // 보상 스프라이트
    private Dictionary<Sprite, int> dicRewards = new Dictionary<Sprite, int>();
    [SerializeField] private Dictionary<string, int> cardRewardDic = new Dictionary<string, int>();
    [SerializeField] private List<Image> rewordImages = new List<Image>(); // 보상 스프라이트를 담는 이미지
    [SerializeField] private List<TMP_Text> rewardCountText = new List<TMP_Text>(); // 보상 스프라이트 개수

    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private GameObject rewardFailPrefab;

    [SerializeField] private Transform rewardGroupTr;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Card);
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
        GameOver(true);
    }

    private void GameOver(bool success)
    {
        if(!isGameOver) // 게임 끝난 상태가 아니면
        {
            isGameOver = true; // 게임이 끝난 상태임을 업데이트
            StopCoroutine("CountDownTimerRoutine"); // 코루틴 정지시키고

            if (success)
            {
                Invoke("ShowGameOverPanel", 0.5f);

            }
            else
            {

            }

        }
    }

    void ShowGameOverPanel()
    {
        SettingRewardsAtUI(); // UI셋팅하고
        gameoverPanel.SetActive(true); // UI 활성화
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

            // 리워드 판정
            GenerateReward();


            if (matchedsFound == totalMatched)
            {
                GameOver(true); // 성공적으로 게임이 끝났음을 노출
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

        // 카드 비교가 끝났으니 뒤집힌 카드 초기화
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

    // 리워드 하나를 랜덤으로 추가
    private void GenerateReward()
    {
        int index = UnityEngine.Random.Range(0, rewordImages.Count);
        if (dicRewards.ContainsKey(cardSprites[index])) // 리워드가 존재하면
        {
            int value = dicRewards[cardSprites[index]];
            value++;
            dicRewards[cardSprites[index]] = value;
        }
        else // 리워드가 존재하지 않으면
        {
            dicRewards.Add(cardSprites[index], 1);
        }
    }

    // 게임이 끝나면 리워드 노출
    private void SettingRewardsAtUI()
    {
        if(dicRewards.Keys.Count <= 0)
        {
            rewardFailPrefab.SetActive(true);
            return;
        }

        rewardGroupTr.gameObject.SetActive(true);
        int index = 0;
        foreach(var sprite in dicRewards.Keys)
        {
            GameObject rewardGo = Instantiate(rewardPrefab, rewardGroupTr);
            rewardGo.SetActive(true);
            RewardUI rewardUi = rewardGo.GetComponent<RewardUI>();
            rewardUi.itemImage.sprite = sprite;
            string count = dicRewards[sprite].ToString() + "개";
            rewardUi.countText.text = count; 

            
            // rewordImages[index].transform.parent.gameObject.SetActive(true);
            cardRewardDic.Add(sprite.name, dicRewards[sprite]); 
            index++;
        }


        RewardConnection.Instance.RewardSaveRequest(cardRewardDic);
    }

}

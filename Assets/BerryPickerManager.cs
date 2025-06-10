using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SwipeDir
{
    RIGHT,
    LEFT,
}
public class BerryInfo
{
    private string name;
    private GameObject gameObject;

    // 프로퍼티
    public String Name { get { return name; } }

    public GameObject GameObject { get { return gameObject; } }

    // 생성자
    public BerryInfo(string name, GameObject gameObject)
    {
        this.name = name;
        this.gameObject = gameObject;
    }
}

public class BerryPickerManager : MonoBehaviour
{
    private static BerryPickerManager _instance;

    public static BerryPickerManager Instance
    { get 
        { 
            return _instance; 
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private int min, max; // 랜덤 최소, 최대 범위. sprite 개수만큼 설정
    [SerializeField] private Transform[] transformArr; // 과일이 배치 될 위치 배열

    private Sprite[] fruitsSprites;

    private BerryInfo[] fruits; // 과일 배열
    private bool isSwipe; // true가 되면 과일 이동하기
    private int pivot = 0; // 제일 앞에 와있는 과일의 인덱스 저장하는 변수

    private Dictionary<string, Sprite> berryDictionary; // 딸기, 상한 딸기

    // UI
    [SerializeField] private GameObject rewardBGImageGO;

    [SerializeField] private Image[] rewardItemImageGo;
    [SerializeField] private TMP_Text[]  rewardItemsTexts;
    public Dictionary<string, Sprite> rewardItemSpriteDic = new Dictionary<string, Sprite>(); // 0번은 무슨 sprite? 1번은 무슨 sprite? .. 추후 random을 어쩌고 하기 위해서
    private Dictionary<string, int> rewardItemDic = new Dictionary<string, int>(); // 어떤 메뉴가 몇 개?

    [SerializeField] private CustomButton checkButton;

    private (string, int)[] scores;

    private int totalCount;

    public float moveSpeed;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.BerryPicker);

        rewardBGImageGO.SetActive(false);

        fruits = new BerryInfo[transformArr.Length]; // 6 ( 0 ~ 5 )

        checkButton.onClick.AddListener(() => SceneController.Instance.ChangeScene("Map"));

        SetRewordItemDic();

        InitBerrySprites();
        min = 0;
        max = fruitsSprites.Length;
        scores = new (string, int)[((int)(fruitsSprites.Length/2))+1]; // [ (딸기, n개), (바나나, n개) ] // 임시... 홀수라면? 어떡하지?
        InitScores();

        InitBerryArray();
        //ChangeSprite();
    }

    private void InitScores()
    {
        int index = 0;
        for (int i = 0; i < fruitsSprites.Length; i++)
        {
            
            if(fruitsSprites[i].name.StartsWith("Spoiled"))
            {
                continue;
            }
            else
            {
                scores[index].Item1 = fruitsSprites[i].name;
                scores[index].Item2 = 0;
                index++;
            }
        }
    }

    private void SetRewordItemDic()
    {
        Sprite[] ingredientSprites = Resources.LoadAll<Sprite>("RewardFruits");

        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            string name = ingredientSprites[i].name;
            rewardItemSpriteDic.Add(name, ingredientSprites[i]);
        }
    }

    private void Update()
    {
        //if(isSwipe)
        //{
        //    MoveBerry();
        //}  
    }

    // sprite 리스트 초기화
    private void InitBerrySprites()
    {
         fruitsSprites = Resources.LoadAll<Sprite>("Fruits");
    }

    // 초기 베리 설정 단계
    private void InitBerryArray()
    {
        for (int i = 0; i < transformArr.Length-1; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max); // 0 또는 1을 뽑아서
            GameObject gameObject = transformArr[i].gameObject; 
            gameObject.GetComponent<SpriteRenderer>().sprite = fruitsSprites[randomValue];
            fruits[i] = new BerryInfo(name, gameObject); // sprite이름과 초기 위치로 과일 배열을 생성
        }
    }

    private void PickRandomBerry()
    {
        int randomValue = UnityEngine.Random.Range(min, max); // 0 또는 1을 뽑아서
        GameObject gameObject = transformArr[transformArr.Length-1].gameObject; // 제일 마지막 위치
        gameObject.GetComponent<SpriteRenderer>().sprite = fruitsSprites[randomValue];

        fruits[transformArr.Length - 1] = new BerryInfo(name, gameObject); // sprite 이름과 초기 위치로 과일 배열을 생성
    }

    // 베리 sprite 변경해서 한칸 앞으로 이동
    private void SortBerries()
    {
        for(int i = 0; i < transformArr.Length-1; i++)
        {
            fruits[i].GameObject.GetComponent<SpriteRenderer>().sprite = fruits[i+1].GameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void MoveBerry(SwipeDir dir)
    {
        StartCoroutine(nameof(CoMoveBeryy), dir);

        // 과일 이동이 끝나면 제일 뒤로 이동해서, 새로운 과일을 지정 받음
        PickRandomBerry();
    }

    private SwipeDir inpuedDir; // 어디로 이동했는지?
    IEnumerator CoMoveBeryy(SwipeDir dir) // RIGHT / LEFT
    {
        float speed = moveSpeed;
        var target = fruits[pivot];

        var originPosition = target.GameObject.transform.localPosition;

        Evaluate(fruits[pivot].GameObject.GetComponent<SpriteRenderer>().sprite.name, dir);

        switch (dir)
        {
            case SwipeDir.RIGHT:
                while (target.GameObject.transform.localPosition.x < 7.6)
                {
                    fruits[pivot].GameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    yield return null;
                }
                break;

            case SwipeDir.LEFT:
                while (target.GameObject.transform.localPosition.x > 0.15)
                {
                    fruits[pivot].GameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    yield return null;
                }
                break;
        }

        // dir방향으로 가있던 tranform 원위치
        fruits[pivot].GameObject.transform.localPosition = originPosition;

        // sprite를 재정렬해서 모든 과일들이 트레일러 한 칸 앞으로 이동
        SortBerries();

        isSwipe = false;
    }

    // 판정
    private void Evaluate(string spriteName, SwipeDir dir)
    {
        if (spriteName.Contains("Spoiled") && dir == SwipeDir.RIGHT)
        {
            Debug.Log("상한 과일을 오른쪽으로 보냄");
        }
        else if(spriteName.Contains("Fresh") && dir == SwipeDir.LEFT)
        {
            Debug.Log("싱싱한 과일을 왼쪽으로 보냄");
            totalCount++;
        }
        else
        {
            Debug.Log("틀림;;");
        }

    }

    // 게임 끝
    public void GameOver()
    {
        // 리워드 UI 나타내기
        rewardBGImageGO.SetActive(true);

        // 리워드 계산하기
        EvaluateRewordItem();
    }


    // 리워드 할 아이템 판정 메서드
    private void EvaluateRewordItem()
    {
        // 랜덤으로 타깃을 찾아서 리워드 결정하기
        // 중복된다면 int를 올려주기
        // dictionary로 관리중
        for (int i = 0; i < totalCount; i++)
        {
            // 랜덤으로 target을 찾기
            int target = UnityEngine.Random.Range(0, rewardItemSpriteDic.Count);
            string name = fruitsSprites[target].name;

            if (rewardItemDic.ContainsKey(name))
            {
                int temp = rewardItemDic[name];
                temp++;
                rewardItemDic[name] = temp;
            }
            else
            {
                rewardItemDic.Add(name, 1);
            }
        }

        // 리워드 UI 활성화 및 sprite, text 적용
        int index = -1;
        foreach(var item in rewardItemDic)
        {
            index++;
            Sprite targetSprite = rewardItemSpriteDic[item.Key];
            rewardItemImageGo[index].GetComponent<Image>().sprite = targetSprite;

            int targetCount = item.Value;
            rewardItemsTexts[index].text = targetCount.ToString();

            rewardItemImageGo[index].transform.parent.gameObject.SetActive(true);
            rewardItemImageGo[index].gameObject.SetActive(true);

        }

        // 게임 끝 통신
        RewardConnection.Instance.RewardSaveRequest(rewardItemDic);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_BerryPicker");
    }

}

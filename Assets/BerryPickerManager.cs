using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor;
using UnityEditorInternal;
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
    [SerializeField] private GameObject rewordBgImg;

    [SerializeField] private Image[] rewordItems;

    private Dictionary<int, Sprite> rewordItemDic = new Dictionary<int, Sprite>();

    private (string, int)[] scores;

    private void Start()
    {
        rewordBgImg.SetActive(false);

        fruits = new BerryInfo[transformArr.Length]; // 6 ( 0 ~ 5 )

        SetRewordItemDic();

        InitBerrySprites();
        min = 0;
        max = fruitsSprites.Length;
        scores = new (string, int)[(int)(fruitsSprites.Length/2)]; // [ (딸기, n개), (바나나, n개) ]
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
        Sprite[] ingredientSprites = Resources.LoadAll<Sprite>("Ingredients");

        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            rewordItemDic.Add(i, ingredientSprites[i]);
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
        float speed = 10f;
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
        if(spriteName == "SpoiledStrawberry" && dir == SwipeDir.RIGHT)
        {
            Debug.Log("상한 딸기가 오른쪽으로 감");
        }
        else if(spriteName == "Strawberry" && dir == SwipeDir.LEFT)
        {
            Debug.Log("싱싱한 딸기가 왼쪽으로 감");
        }
        else
        {
            Debug.Log("틀렸습니다.");
        }

    }

    // 게임 끝
    public void GameOver()
    {
        // 리워드 UI 나타내기
        rewordBgImg.SetActive(true);

        // 리워드 계산하기
        EvaluateRewordItem();
    }


    // 리워드 할 아이템 판정 메서드
    private void EvaluateRewordItem()
    {
        // 임시로 3번 반복 할 것
        for(int i = 0; i < 3; i++)
        {
            int target = UnityEngine.Random.Range(0, rewordItemDic.Count);
            rewordItems[i].transform.parent.gameObject.SetActive(true);
            rewordItems[i].gameObject.SetActive(true);
            rewordItems[i].GetComponent<Image>().sprite = rewordItemDic[target];
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_BerryPicker");
    }

}

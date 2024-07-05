using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
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

    [SerializeField] private int min, max; // 랜덤 최소, 최대 범위. sprite 개수만큼 설정
    [SerializeField] private Transform[] transformArr; // 과일이 배치 될 위치 배열

    private Sprite[] berrySprites;

    private BerryInfo[] berries; // 과일 배열
    private bool isSwipe; // true가 되면 과일 이동하기
    private int pivot = 0; // 제일 앞에 와있는 과일의 인덱스 저장하는 변수

    private Dictionary<string, Sprite> berryDictionary; // 딸기, 상한 딸기
    private void Start()
    {
        berries = new BerryInfo[transformArr.Length]; // 6 ( 0 ~ 5 )

        InitBerrySprites();
        InitBerryArray();
        //ChangeSprite();
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
         berrySprites = Resources.LoadAll<Sprite>("Fruites");
    }

    // 초기 베리 설정 단계
    private void InitBerryArray()
    {
        for (int i = 0; i < transformArr.Length-1; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max); // 0 또는 1을 뽑아서
            GameObject gameObject = transformArr[i].gameObject; 
            gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue]; // 0번째 또는 1번째 스프라이트를 담는다.
            berries[i] = new BerryInfo(name, gameObject); // sprite이름과 초기 위치로 과일 배열을 생성
        }
    }

    private void PickRandomBerry()
    {
        int randomValue = UnityEngine.Random.Range(min, max); // 0 또는 1을 뽑아서
        GameObject gameObject = transformArr[transformArr.Length-1].gameObject; // 제일 마지막 위치
        gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue];

        berries[transformArr.Length - 1] = new BerryInfo(name, gameObject); // sprite 이름과 초기 위치로 과일 배열을 생성
    }

    // 베리 sprite 변경해서 한칸 앞으로 이동
    private void SortBerries()
    {
        for(int i = 0; i < transformArr.Length-1; i++)
        {
            berries[i].GameObject.GetComponent<SpriteRenderer>().sprite = berries[i+1].GameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void MoveBerry(SwipeDir dir)
    {
        StartCoroutine(nameof(CoMoveBeryy), dir);

        // 과일 이동이 끝나면 제일 뒤로 이동해서, 새로운 과일을 지정 받음
        PickRandomBerry();


    }

    IEnumerator CoMoveBeryy(SwipeDir dir) // RIGHT / LEFT
    {
        float speed = 5f;
        var target = berries[pivot];

        var originPosition = target.GameObject.transform.localPosition;

        switch (dir)
        {
            case SwipeDir.RIGHT:
                while (target.GameObject.transform.localPosition.x < 7.6)
                {
                    berries[pivot].GameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    yield return null;
                }
                break;

            case SwipeDir.LEFT:
                while (target.GameObject.transform.localPosition.x > 0.15)
                {
                    berries[pivot].GameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    yield return null;
                }
                break;
        }

        // dir방향으로 가있던 tranform 원위치
        berries[pivot].GameObject.transform.localPosition = originPosition;

        // sprite를 재정렬해서 모든 과일들이 트레일러 한 칸 앞으로 이동
        SortBerries();

        isSwipe = false;
    }


}

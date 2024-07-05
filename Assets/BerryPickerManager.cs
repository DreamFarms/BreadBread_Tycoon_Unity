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
        berries = new BerryInfo[transformArr.Length];

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
        for (int i = 0; i < transformArr.Length; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max);
            GameObject gameObject = transformArr[i].gameObject;
            gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue];
            berries[i] = new BerryInfo(name, gameObject); // sprite이름과 초기 위치로 과일 배열을 생성
        }
    }

    // 베리 sprite 변경하기
    private void ChangeSprite()
    {
        for(int i = 0; i < transformArr.Length; i++)
        {
            // 램던 과일이 담긴 배열에서 이름을 추출해서
            // resources 폴더에서 동일한 이름의 sprite를 찾아 setting
            string name = berries[i].Name;
            Sprite sprite = Resources.Load<Sprite>($"Fruites/{name}");
            berries[i].GameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void MoveBerry(SwipeDir dir)
    {
        StartCoroutine(nameof(CoMoveBeryy), dir);
    }

    IEnumerator CoMoveBeryy(SwipeDir dir) // RIGHT / LEFT
    {
        float speed = 5f;
        var target = berries[pivot];

        switch(dir)
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


        pivot++;
        isSwipe = false;
        
    }


}

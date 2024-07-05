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

    // ������Ƽ
    public String Name { get { return name; } }

    public GameObject GameObject { get { return gameObject; } }

    // ������
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

    [SerializeField] private int min, max; // ���� �ּ�, �ִ� ����. sprite ������ŭ ����
    [SerializeField] private Transform[] transformArr; // ������ ��ġ �� ��ġ �迭

    private Sprite[] berrySprites;

    private BerryInfo[] berries; // ���� �迭
    private bool isSwipe; // true�� �Ǹ� ���� �̵��ϱ�
    private int pivot = 0; // ���� �տ� ���ִ� ������ �ε��� �����ϴ� ����

    private Dictionary<string, Sprite> berryDictionary; // ����, ���� ����
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

    // sprite ����Ʈ �ʱ�ȭ
    private void InitBerrySprites()
    {
         berrySprites = Resources.LoadAll<Sprite>("Fruites");
    }

    // �ʱ� ���� ���� �ܰ�
    private void InitBerryArray()
    {
        for (int i = 0; i < transformArr.Length-1; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max); // 0 �Ǵ� 1�� �̾Ƽ�
            GameObject gameObject = transformArr[i].gameObject; 
            gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue]; // 0��° �Ǵ� 1��° ��������Ʈ�� ��´�.
            berries[i] = new BerryInfo(name, gameObject); // sprite�̸��� �ʱ� ��ġ�� ���� �迭�� ����
        }
    }

    private void PickRandomBerry()
    {
        int randomValue = UnityEngine.Random.Range(min, max); // 0 �Ǵ� 1�� �̾Ƽ�
        GameObject gameObject = transformArr[transformArr.Length-1].gameObject; // ���� ������ ��ġ
        gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue];

        berries[transformArr.Length - 1] = new BerryInfo(name, gameObject); // sprite �̸��� �ʱ� ��ġ�� ���� �迭�� ����
    }

    // ���� sprite �����ؼ� ��ĭ ������ �̵�
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

        // ���� �̵��� ������ ���� �ڷ� �̵��ؼ�, ���ο� ������ ���� ����
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

        // dir�������� ���ִ� tranform ����ġ
        berries[pivot].GameObject.transform.localPosition = originPosition;

        // sprite�� �������ؼ� ��� ���ϵ��� Ʈ���Ϸ� �� ĭ ������ �̵�
        SortBerries();

        isSwipe = false;
    }


}

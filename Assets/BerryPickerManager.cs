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
        berries = new BerryInfo[transformArr.Length];

        InitBerrySprites();
        InitBerryArray();
        //ChangeSprite();
    }


    private void Update()
    {
        if(isSwipe)
        {
            MoveBerry();
        }  
    }

    // sprite ����Ʈ �ʱ�ȭ
    private void InitBerrySprites()
    {
         berrySprites = Resources.LoadAll<Sprite>("Fruites");
    }

    // �ʱ� ���� ���� �ܰ�
    private void InitBerryArray()
    {
        for (int i = 0; i < transformArr.Length; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max);
            GameObject gameObject = transformArr[i].gameObject;
            gameObject.GetComponent<SpriteRenderer>().sprite = berrySprites[randomValue];
            berries[i] = new BerryInfo(name, gameObject); // sprite�̸��� �ʱ� ��ġ�� ���� �迭�� ����
        }
    }

    // ���� sprite �����ϱ�
    private void ChangeSprite()
    {
        for(int i = 0; i < transformArr.Length; i++)
        {
            // ���� ������ ��� �迭���� �̸��� �����ؼ�
            // resources �������� ������ �̸��� sprite�� ã�� setting
            string name = berries[i].Name;
            Sprite sprite = Resources.Load<Sprite>($"Fruites/{name}");
            berries[i].GameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    private bool isMove; // 
    private bool isRight; // �¿� ����
    public void MoveBerry()
    {
        StartCoroutine(nameof(CoMoveBeryy));


        // �������� �ϸ� �¿� �Ǵܿ� ���� tranform�� �̵�


        // pivot�� �迭�� �ʰ��ϸ�
        //if(pivot >= berries.Length)
        //{
        //    pivot = 0; // pivot ��ġ �ʱ�ȭ
        //}




        //pivot++; // �������� pivot ��ġ ������Ʈ


    }

    IEnumerator CoMoveBeryy()
    {
        float speed = 3f;
        var target = berries[pivot];

        while (target.GameObject.transform.localPosition.x > 0.15)
        {
            berries[pivot].GameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
            yield return null;
        }

        pivot++;
        isSwipe = false;
        
    }
}

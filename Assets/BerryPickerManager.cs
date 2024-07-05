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

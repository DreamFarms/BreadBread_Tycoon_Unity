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

    private int min, max; // ���� �ּ�, �ִ� ����. sprite ������ŭ ����
    [SerializeField] private Transform[] transformArr; // ������ ��ġ �� ��ġ �迭

    private Sprite[] fruitsSprites;

    private BerryInfo[] fruits; // ���� �迭
    private bool isSwipe; // true�� �Ǹ� ���� �̵��ϱ�
    private int pivot = 0; // ���� �տ� ���ִ� ������ �ε��� �����ϴ� ����

    private Dictionary<string, Sprite> berryDictionary; // ����, ���� ����

    // UI
    [SerializeField] private GameObject rewardBGImageGO;

    [SerializeField] private Image[] rewardItemImageGo;
    [SerializeField] private TMP_Text[]  rewardItemsTexts;
    public Dictionary<string, Sprite> rewardItemSpriteDic = new Dictionary<string, Sprite>(); // 0���� ���� sprite? 1���� ���� sprite? .. ���� random�� ��¼�� �ϱ� ���ؼ�
    private Dictionary<string, int> rewardItemDic = new Dictionary<string, int>(); // � �޴��� �� ��?

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
        scores = new (string, int)[((int)(fruitsSprites.Length/2))+1]; // [ (����, n��), (�ٳ���, n��) ] // �ӽ�... Ȧ�����? �����?
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

    // sprite ����Ʈ �ʱ�ȭ
    private void InitBerrySprites()
    {
         fruitsSprites = Resources.LoadAll<Sprite>("Fruits");
    }

    // �ʱ� ���� ���� �ܰ�
    private void InitBerryArray()
    {
        for (int i = 0; i < transformArr.Length-1; i++)
        {
            int randomValue = UnityEngine.Random.Range(min, max); // 0 �Ǵ� 1�� �̾Ƽ�
            GameObject gameObject = transformArr[i].gameObject; 
            gameObject.GetComponent<SpriteRenderer>().sprite = fruitsSprites[randomValue];
            fruits[i] = new BerryInfo(name, gameObject); // sprite�̸��� �ʱ� ��ġ�� ���� �迭�� ����
        }
    }

    private void PickRandomBerry()
    {
        int randomValue = UnityEngine.Random.Range(min, max); // 0 �Ǵ� 1�� �̾Ƽ�
        GameObject gameObject = transformArr[transformArr.Length-1].gameObject; // ���� ������ ��ġ
        gameObject.GetComponent<SpriteRenderer>().sprite = fruitsSprites[randomValue];

        fruits[transformArr.Length - 1] = new BerryInfo(name, gameObject); // sprite �̸��� �ʱ� ��ġ�� ���� �迭�� ����
    }

    // ���� sprite �����ؼ� ��ĭ ������ �̵�
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

        // ���� �̵��� ������ ���� �ڷ� �̵��ؼ�, ���ο� ������ ���� ����
        PickRandomBerry();
    }

    private SwipeDir inpuedDir; // ���� �̵��ߴ���?
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

        // dir�������� ���ִ� tranform ����ġ
        fruits[pivot].GameObject.transform.localPosition = originPosition;

        // sprite�� �������ؼ� ��� ���ϵ��� Ʈ���Ϸ� �� ĭ ������ �̵�
        SortBerries();

        isSwipe = false;
    }

    // ����
    private void Evaluate(string spriteName, SwipeDir dir)
    {
        if (spriteName.Contains("Spoiled") && dir == SwipeDir.RIGHT)
        {
            Debug.Log("���� ������ ���������� ����");
        }
        else if(spriteName.Contains("Fresh") && dir == SwipeDir.LEFT)
        {
            Debug.Log("�̽��� ������ �������� ����");
            totalCount++;
        }
        else
        {
            Debug.Log("Ʋ��;;");
        }

    }

    // ���� ��
    public void GameOver()
    {
        // ������ UI ��Ÿ����
        rewardBGImageGO.SetActive(true);

        // ������ ����ϱ�
        EvaluateRewordItem();
    }


    // ������ �� ������ ���� �޼���
    private void EvaluateRewordItem()
    {
        // �������� Ÿ���� ã�Ƽ� ������ �����ϱ�
        // �ߺ��ȴٸ� int�� �÷��ֱ�
        // dictionary�� ������
        for (int i = 0; i < totalCount; i++)
        {
            // �������� target�� ã��
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

        // ������ UI Ȱ��ȭ �� sprite, text ����
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

        // ���� �� ���
        RewardConnection.Instance.RewardSaveRequest(rewardItemDic);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_BerryPicker");
    }

}

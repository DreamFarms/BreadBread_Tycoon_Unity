using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe
{
    // id�� int�� ���� ����
    string[] ingredients;

    // ������
    public Recipe(string[] ingredients)
    {
        this.ingredients = new string[ingredients.Length];
        this.ingredients = ingredients;
    }
}

public class RecipeGameManager : MonoBehaviour
{
    private static RecipeGameManager _instance;

    public static RecipeGameManager Instance
    {
        get { return _instance; }
    }

    

    [SerializeField] private RecipeConnection _connection;

    private Dictionary<string, string> _indexInfoDic = new Dictionary<string, string>(); // �ε���, �� ���� : �ѱ���(�и� �ʿ�)
    private Dictionary<string, string> _ingredientInfoDic = new Dictionary<string, string>(); // ��� ���� : �ѱ���
    private Dictionary<string, int> _ingredientCountDic = new Dictionary<string, int>(); // ��� ���� : ����
    
    [SerializeField] private BallPositions _ballPositions; // assign
    [SerializeField] public Dictionary<string, int> selectedIngredientDic = new Dictionary<string, int>(); // ��� : n�� ��ġ
    public Dictionary<string, ItemInfo> itemInfoDic = new Dictionary<string, ItemInfo>(); // string, iteminfo... ball��ġ�� iteminfo�� �ʿ��ѵ� �װ� ���� string�� �ֱ� ������ ������ ����.. �̰� �ּ��ΰ��� ���� ���

    public Dictionary<string, string> breadNameInfoDic = new Dictionary<string, string>(); // �� �̸� ���� : �ѱ���
    public List<string> findedRecipes = new List<string>(); // ã�� ������

    public Button submitButton; // ui
    public RecipeBookImage recipeBookImage; // ui

    public GameObject doughGo; // assign

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        doughGo.SetActive(false);

    }

    private void Start()
    {
        InitSetting();
    }

    private void InitSetting()
    {
        _connection.StartRecipeConnection(); // ����� ���, ������ ���� ���
        //SetInitScrollUI();
        //Invoke("SetInitScrollUI", 1.5f); // UI ����.. ������ ���� ����� ���� �� ���� �Ǿ�� ��

    }
    public void SetInitScrollUI()
    {
        RecipeUIManager.Instance.SetInitScrollUI();

        // �Ʒ� �ΰ��� ��ġ �ű� �ʿ䰡 �־��
        AudioManager.Instance.PlayBGM(BGM.Recipe);
        RecipeUIManager.Instance.roading.SetActive(false);
    }

    public Dictionary<string, string> IndexInfoDic
    {
        get { return _indexInfoDic; }
        set { _indexInfoDic = value; }
    }

    public Dictionary<string, string> IngredientInfoDic
    {
        get { return _ingredientInfoDic;}
        set { _ingredientInfoDic = value; }
    }

    public Dictionary<string, int> IngredientCountDic { get => _ingredientCountDic; set => _ingredientCountDic = value; }

    //public void SetIngredientInfoDic(string enName, string koName)
    //{
    //    if(!_ingredientInfoDic.ContainsKey(enName))
    //    {
    //        _ingredientInfoDic[enName] = koName;
    //    }
    //}
    


    public void PuntIngredientInBall(ItemInfo itemInfo, Sprite sprite)
    {
        _ballPositions.PutIngredient(itemInfo, sprite);
    }

    public void SubmitIngredient()
    {
        if (selectedIngredientDic.Count <= 0) // ���⼭ ��ȿ�� �˻縦 �ϴ°� �´��� �ǹ�
        {
            RecipeUIManager.Instance.ActiveFailUI();
            return;
        }

        RemoveIngredientInBall();
        PlayDoughAnimation();
        Invoke("RequestIngredientConnection", 1.5f);
    }

    private void RequestIngredientConnection()
    {
        List<string> selectedIngredients = new List<string>();

        foreach (var ingredient in selectedIngredientDic.Keys)
        {
            selectedIngredients.Add(ingredient);
        }
        selectedIngredientDic.Clear();

        _connection.RecipeGameResultConnection(selectedIngredients);

        // ������ �Ͽ� ���� ����ϴ� �޼���
        // ui manager�� �ű� �ʿ�
        // recipeBookImage.AddRecipeOnRecipeBook();
    }

    private void RemoveIngredientInBall()
    {
        foreach (Transform tr in _ballPositions.ingredientPositions)
        {
            tr.GetComponent<SpriteRenderer>().sprite = null;
            tr.gameObject.SetActive(false);
        }
    }

    public void PlayDoughAnimation()
    {
        doughGo.SetActive(true);

        Dough dough = doughGo.GetComponent<Dough>();
        dough.PlayAnimation();
    }

    public void StopDoughAnimation()
    {
        Dough dough = doughGo.GetComponent<Dough>();
        dough.StopAnimation();

        doughGo.SetActive(false);
    }
}

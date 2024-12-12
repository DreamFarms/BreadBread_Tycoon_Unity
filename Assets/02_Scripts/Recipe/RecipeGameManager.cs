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

    private Dictionary<string, string> _indexInfoDic = new Dictionary<string, string>();
    private Dictionary<string, string> _ingredientInfoDic = new Dictionary<string, string>();
    [SerializeField] private BallPositions _ballPositions; // assign
    [SerializeField] public Dictionary<string, int> selectedIngredientDic = new Dictionary<string, int>(); // ��� : n�� ��ġ

    // test
    public List<string> findedRecipes = new List<string>(); // ã�� �����ǵ�
    public Dictionary<string, Recipe> findedRecipeDic = new Dictionary<string, Recipe>(); // �� �̸� : ������
    public Button submitButton; // ui
    public RecipeBookImage recipeBookImage; // ui

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

        submitButton.onClick.AddListener(() => SubmitIngredient());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _connection.StartRecipeConnection();
        }
        if(Input.GetKeyDown(KeyCode.S)) 
        {
            _connection.RecipeGameResultConnection();
        }

        // �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.D)) 
        {
            // dic�� ������ ���
            string flour = "Flour";
            string sugar = "Sugar";
            string blueberry = "FreshBlueBerry";

            string[] ingredients = new string[3];

            ingredients[0] = flour;
            ingredients[1] = sugar;
            ingredients[2] = blueberry;

            Recipe recipe = new Recipe(ingredients);

            findedRecipeDic["����Ÿ��Ʈ"] = recipe;

            // list�� ã�� ������ ���
            findedRecipes.Add("����Ÿ��Ʈ");

            Debug.Log("�׽�Ʈ�� �۾��� ���ƽ��ϴ�. \n �а���, ����, ��纣���� ��� ���� ��ư�� ��������.");
        }

    }

    public Dictionary<string, string> IndexInfoDic
    {
        get { return _indexInfoDic; }
        set { _indexInfoDic = value; }
    }

    public Dictionary<string, string> IngredientInfoDic
    {
        get { return _ingredientInfoDic;}
    }

    public void SetIngredientInfoDic(string enName, string koName)
    {
        if(!_ingredientInfoDic.ContainsKey(enName))
        {
            _ingredientInfoDic[enName] = koName;
        }
    }
    
    public void SetInitScrollUI()
    {
        RecipeUIManager.Instance.SetInitScrollUI();
    }

    public void PuntIngredientInBall(Sprite sprite)
    {
        _ballPositions.PutIngredient(sprite);
    }

    // test
    // �˸°� ���� �ʿ�
    public void SubmitIngredient()
    {

        Debug.Log("�����Ǹ� ã�ҽ��ϴ�. ������ ���� ������.");
        foreach(Transform tr in _ballPositions.ingredientPositions)
        {
            tr.GetComponent<SpriteRenderer>().sprite = null;
            tr.gameObject.SetActive(false);
        }
        selectedIngredientDic.Clear();
        recipeBookImage.AddRecipeOnRecipeBook();
    }
}

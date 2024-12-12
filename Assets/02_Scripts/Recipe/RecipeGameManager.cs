using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe
{
    // id인 int로 변경 권장
    string[] ingredients;

    // 생성자
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
    [SerializeField] public Dictionary<string, int> selectedIngredientDic = new Dictionary<string, int>(); // 재료 : n번 위치

    // test
    public List<string> findedRecipes = new List<string>(); // 찾은 레시피들
    public Dictionary<string, Recipe> findedRecipeDic = new Dictionary<string, Recipe>(); // 빵 이름 : 레시피
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

        // 테스트
        if(Input.GetKeyDown(KeyCode.D)) 
        {
            // dic에 레시피 등록
            string flour = "Flour";
            string sugar = "Sugar";
            string blueberry = "FreshBlueBerry";

            string[] ingredients = new string[3];

            ingredients[0] = flour;
            ingredients[1] = sugar;
            ingredients[2] = blueberry;

            Recipe recipe = new Recipe(ingredients);

            findedRecipeDic["딸기타르트"] = recipe;

            // list에 찾은 레시피 등록
            findedRecipes.Add("딸기타르트");

            Debug.Log("테스트용 작업을 마쳤습니다. \n 밀가루, 설탕, 블루베리를 담고 제출 버튼을 누르세요.");
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
    // 알맞게 수정 필요
    public void SubmitIngredient()
    {

        Debug.Log("레시피를 찾았습니다. 레시피 북을 여세요.");
        foreach(Transform tr in _ballPositions.ingredientPositions)
        {
            tr.GetComponent<SpriteRenderer>().sprite = null;
            tr.gameObject.SetActive(false);
        }
        selectedIngredientDic.Clear();
        recipeBookImage.AddRecipeOnRecipeBook();
    }
}

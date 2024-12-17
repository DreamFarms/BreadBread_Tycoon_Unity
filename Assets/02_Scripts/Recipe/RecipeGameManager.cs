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

    private Dictionary<string, string> _indexInfoDic = new Dictionary<string, string>(); // 인덱스, 빵 영어 : 한국어(분리 필요)
    private Dictionary<string, string> _ingredientInfoDic = new Dictionary<string, string>(); // 재료 영어 : 한국어
    private Dictionary<string, int> _ingredientCountDic = new Dictionary<string, int>(); // 재료 영어 : 개수
    
    [SerializeField] private BallPositions _ballPositions; // assign
    [SerializeField] public Dictionary<string, int> selectedIngredientDic = new Dictionary<string, int>(); // 재료 : n번 위치

    public Dictionary<string, string> breadNameInfoDic = new Dictionary<string, string>(); // 빵 이름 영어 : 한국어
    public List<string> findedRecipes = new List<string>(); // 찾은 레시피

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

        InitSetting();
        
    }

    private void OnEnable()
    {
        submitButton.onClick.AddListener(() => SubmitIngredient());
        
    }

    private void Update()
    {

    }

    private void InitSetting()
    {
        _connection.StartRecipeConnection(); // 사용자 재료, 레시피 정보 통신
        Invoke("SetInitScrollUI", 3f); // UI 셋팅.. 레시피 정보 통신이 끝난 후 진행 되어야 함
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
        List<string> selectedIngredients = new List<string>();

        foreach(var ingredient in selectedIngredientDic.Keys)
        {
            selectedIngredients.Add(ingredient);
        }

        foreach(Transform tr in _ballPositions.ingredientPositions)
        {
            tr.GetComponent<SpriteRenderer>().sprite = null;
            tr.gameObject.SetActive(false);
        }

        selectedIngredientDic.Clear();

        _connection.RecipeGameResultConnection(selectedIngredients);

        // 레시피 북에 사진 등록하는 메서드
        // ui manager로 옮길 필요
        // recipeBookImage.AddRecipeOnRecipeBook();
    }
}

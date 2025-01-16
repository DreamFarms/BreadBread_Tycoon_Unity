using System;
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
    public Dictionary<string, ItemInfo> itemInfoDic = new Dictionary<string, ItemInfo>(); // string, iteminfo... ball배치시 iteminfo가 필요한데 그게 없어 string만 있기 때문에 생성한 변수.. 이게 최선인가에 대한 고민

    public Dictionary<string, string> breadNameInfoDic = new Dictionary<string, string>(); // 빵 이름 영어 : 한국어
    public List<string> findedRecipes = new List<string>(); // 찾은 레시피

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
        _connection.StartRecipeConnection(); // 사용자 재료, 레시피 정보 통신
        //SetInitScrollUI();
        //Invoke("SetInitScrollUI", 1.5f); // UI 셋팅.. 레시피 정보 통신이 끝난 후 진행 되어야 함

    }
    public void SetInitScrollUI()
    {
        RecipeUIManager.Instance.SetInitScrollUI();

        // 아래 두개는 위치 옮길 필요가 있어보임
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
        if (selectedIngredientDic.Count <= 0) // 여기서 유효성 검사를 하는게 맞는지 의문
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

        // 레시피 북에 사진 등록하는 메서드
        // ui manager로 옮길 필요
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    private static RecipeUIManager _instance;

    public static RecipeUIManager Instance
    {
        get { return _instance; }
    }


    [Header("Roading")]
    [SerializeField] public GameObject roading; // assign

    [Header("관리")]
    [SerializeField] private RecipeGameManager recipeGameManager; // assign
    [SerializeField] private RecipeConnection recipeConnection; // go assign

    [Header("버튼")]
    [SerializeField] private Button submitButton; // ui assign

    [Header("재료")]
    [SerializeField] private Transform indexTr;
    [SerializeField] private GameObject ingredientIndexButton;
    [SerializeField] public string[] indexNames; // Inspector에서 설정
    [SerializeField] private string currentIndexName;

    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private Transform viewPartTr;
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject ingredientPrefab;

    [Header("레시피 북")]
    [SerializeField] private RecipeBookImage recipeBookImage; // recipeBookImage
    [SerializeField] private Button recipeBookButton; // 레시피 북 버튼
    [SerializeField] private GameObject recipeBookGroup; // 레시피 북 이미지
    [SerializeField] private Button recipeBookCloseBtn; // 레시피 북 닫기 버튼
    [SerializeField] private List<GameObject> categories = new List<GameObject>(); // 카테고리 리스트
    [SerializeField] private Button _nextButton; // ui
    private int _currentPage;
    private GameObject _currentCategory;

    [Header("믹싱볼")]
    [SerializeField] private GameManager ball; // 믹싱볼

    [Header("리워드")]
    [SerializeField] private GameObject rewordGroup; // reword bg img
    [SerializeField] private RewordUI rewordUI; // reword bg img
    [SerializeField] private GameObject rewordFailGroup; // resord bg img fail

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

        roading.SetActive(true);
    }


    private void OnEnable()
    {
        // 버튼
        submitButton.onClick.AddListener(() =>
        {
            RecipeGameManager.Instance.SubmitIngredient();
        });


        // 레시피 북
        recipeBookGroup.SetActive(false);

        recipeBookButton.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookGroup);
        });

        recipeBookCloseBtn.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookGroup);
        });

        _nextButton.onClick.AddListener(() =>
        {
            recipeBookImage.OnClickNextPageButton();
        });

        //rewordUIGo.SetActive(false);
    }

    private void ActiveGameobject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    // 재료 영역 생성
    internal void SetInitScrollUI()
    {
        foreach (string name in indexNames)
        {
            // 스크롤 인덱스 버튼 생성
            GameObject indexButton = Instantiate(ingredientIndexButton, indexTr);
            indexButton.name = name;

            string koName = GameManager.Instance.indexInfoDic[name];
            indexButton.transform.GetChild(0).GetComponent<TMP_Text>().text = koName;

            GameObject content = Instantiate(contentPrefab, viewPartTr);
            content.name = name + "Content";

            Sprite[] sprites = Resources.LoadAll<Sprite>("Ingredients/" + name);

            foreach(Sprite sprite in sprites)
            {
                GameObject go = Instantiate(ingredientPrefab, content.transform);
                go.name = sprite.name;
                go.GetComponent<ItemInfo>().rewordItemImage.sprite = sprite;

                string ingredientKoName = RecipeGameManager.Instance.IngredientInfoDic[sprite.name];
                int ingredientCount = 0;
                if (recipeGameManager.IngredientCountDic.ContainsKey(sprite.name))
                {
                    ingredientCount = RecipeGameManager.Instance.IngredientCountDic[sprite.name];
                }

                go.GetComponent<ItemInfo>().rewordItemName.text = ingredientKoName;
                go.GetComponent<ItemInfo>().rewordItemCount.text = ingredientCount.ToString();

                // 이벤트 등록 : 택한 재료가 ball에 배치됨
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // ItemInfo는 PutIngredientInBall에 전달되어 재료를 선택하면 -, 선택 해제하면 +가 됨
                    ItemInfo itemInfo = go.GetComponent<ItemInfo>();
                    recipeGameManager.PuntIngredientInBall(itemInfo, sprite);

                });
            }



            // 이벤트 등록
            indexButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                string currentIndexName = RecipeUIManager.Instance.currentIndexName;
                if (currentIndexName != indexButton.name)
                {
                    content.SetActive(true);
                    viewPartTr.Find(currentIndexName + "Content").gameObject.SetActive(false);
                    RecipeUIManager.Instance.ingredientScroll.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
                    RecipeUIManager.Instance.currentIndexName = indexButton.name;
                }
                else
                {
                    return;
                }
            });

            // 첫번째 IngredientContent 제외하고 비활성화
            string firstContent = viewPartTr.GetChild(0).gameObject.name;
            if (content.name == firstContent)
            {
                currentIndexName = indexButton.name;
                ingredientScroll.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
            }
            else
            {
                content.SetActive(false);
            }
        }

        recipeBookImage.InitRecipeBook();
    }

    public  void ActiveRewordUI(string findedBreadName, int findedState)
    {
        switch(findedState)
        {
            case 1:
                rewordGroup.SetActive(true);
                rewordUI.title.text = "성공!";
                rewordUI.rewordItemImage.sprite = Resources.Load<Sprite>("Breads/" + findedBreadName);
                rewordUI.rewordItemName.text = GameManager.Instance.indexInfoDic[findedBreadName];
                recipeBookImage.AddRecipeOnRecipeBook(findedBreadName);
                break;
            case 0:
                rewordFailGroup.SetActive(true);
                RewordUI failUI = rewordFailGroup.GetComponent<RewordUI>();
                failUI.uiGo.SetActive(true);
                failUI.rewordItemName.text = "찾는 레시피가 없습니다.";
                break;
            case -1:
                rewordGroup.SetActive(true);
                rewordUI.title.text = "이미 알고있는 레시피";
                rewordUI.title.fontSize = 50;
                rewordUI.rewordItemImage.sprite = Resources.Load<Sprite>("Breads/" + findedBreadName);
                rewordUI.rewordItemName.text = GameManager.Instance.indexInfoDic[findedBreadName];
                break;
        }

    }

    public void ActiveFailUI()
    {
        rewordFailGroup.SetActive(true);
        RewordUI rewordUI = rewordFailGroup.GetComponent<RewordUI>();
        rewordUI.rewordItemName.text = "재료를 담아주세요.";
    }
}

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


    }

    [Header("관리")]
    [SerializeField] private RecipeGameManager recipeGameManager; // assign

    [Header("재료")]
    [SerializeField] private Transform indexTr;
    [SerializeField] private GameObject ingredientIndexButton;
    [SerializeField] public string[] indexNames; // Inspector에서 설정
    [SerializeField] private string currentIndexName;

    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private Transform viewPartTr;
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject ingredientPrefab;

    [SerializeField] private Button recipeBookButton; // 레시피 북 버튼
    [SerializeField] private GameObject recipeBookImage; // 레시피 북 이미지
    [SerializeField] private Button recipeBookCloseBtn; // 레시피 북 닫기 버튼

    [SerializeField] private GameManager ball; // 믹싱볼

    [Header("레시피 북")]
    [SerializeField] private List<GameObject> categories = new List<GameObject>(); // 카테고리 리스트
    [SerializeField] private Button _nextButton; // ui
    private int _currentPage;
    private GameObject _currentCategory;

    private void OnEnable()
    {
        recipeBookImage.SetActive(false);

        recipeBookButton.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImage);
        });

        recipeBookCloseBtn.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImage);
        });

        _nextButton.onClick.AddListener(() =>
        {
            Debug.Log("다음 페이지를 넘겼습니다.");
        });
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

            string koName = RecipeGameManager.Instance.IndexInfoDic[name];
            indexButton.transform.GetChild(0).GetComponent<TMP_Text>().text = koName;

            GameObject content = Instantiate(contentPrefab, viewPartTr);
            content.name = name + "Content";

            Sprite[] sprites = Resources.LoadAll<Sprite>("Ingredients/" + name);
            foreach(Sprite sprite in sprites)
            {
                GameObject go = Instantiate(ingredientPrefab, content.transform);
                go.name = sprite.name;
                go.GetComponent<ItemInfo>().rewordItemImage.sprite = sprite;
                string IngredientKoName = RecipeGameManager.Instance.IngredientInfoDic[sprite.name];
                go.GetComponent<ItemInfo>().rewordItemName.text = IngredientKoName;

                
                go.GetComponent<Button>().onClick.AddListener(() => recipeGameManager.PuntIngredientInBall(sprite));
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
    }
}

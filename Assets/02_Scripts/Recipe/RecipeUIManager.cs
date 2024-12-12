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

    [Header("����")]
    [SerializeField] private RecipeGameManager recipeGameManager; // assign

    [Header("���")]
    [SerializeField] private Transform indexTr;
    [SerializeField] private GameObject ingredientIndexButton;
    [SerializeField] public string[] indexNames; // Inspector���� ����
    [SerializeField] private string currentIndexName;

    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private Transform viewPartTr;
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject ingredientPrefab;

    [SerializeField] private Button recipeBookButton; // ������ �� ��ư
    [SerializeField] private GameObject recipeBookImage; // ������ �� �̹���
    [SerializeField] private Button recipeBookCloseBtn; // ������ �� �ݱ� ��ư

    [SerializeField] private GameManager ball; // �ͽ̺�

    [Header("������ ��")]
    [SerializeField] private List<GameObject> categories = new List<GameObject>(); // ī�װ� ����Ʈ
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
            Debug.Log("���� �������� �Ѱ���ϴ�.");
        });
    }


    private void ActiveGameobject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    // ��� ���� ����
    internal void SetInitScrollUI()
    {
        foreach (string name in indexNames)
        {
            // ��ũ�� �ε��� ��ư ����
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



            // �̺�Ʈ ���
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

            // ù��° IngredientContent �����ϰ� ��Ȱ��ȭ
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

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
    [SerializeField] private RecipeConnection recipeConnection; // go assign

    [Header("��ư")]
    [SerializeField] private Button submitButton; // ui assign

    [Header("���")]
    [SerializeField] private Transform indexTr;
    [SerializeField] private GameObject ingredientIndexButton;
    [SerializeField] public string[] indexNames; // Inspector���� ����
    [SerializeField] private string currentIndexName;

    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private Transform viewPartTr;
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject ingredientPrefab;

    [Header("������ ��")]
    [SerializeField] private RecipeBookImage recipeBookImage; // recipeBookImage
    [SerializeField] private Button recipeBookButton; // ������ �� ��ư
    [SerializeField] private GameObject recipeBookImageGo; // ������ �� �̹���
    [SerializeField] private Button recipeBookCloseBtn; // ������ �� �ݱ� ��ư
    [SerializeField] private List<GameObject> categories = new List<GameObject>(); // ī�װ� ����Ʈ
    [SerializeField] private Button _nextButton; // ui
    private int _currentPage;
    private GameObject _currentCategory;

    [Header("�ͽ̺�")]
    [SerializeField] private GameManager ball; // �ͽ̺�

    [Header("������")]
    [SerializeField] private GameObject rewordUIGo; // reword bg img
    [SerializeField] private RewordUI rewordUI; // reword bg img
    [SerializeField] private GameObject rewordFailUIGo; // resord bg img fail




    private void OnEnable()
    {
        // ��ư
        submitButton.onClick.AddListener(() => RecipeGameManager.Instance.SubmitIngredient());


        // ������ ��
        recipeBookImageGo.SetActive(false);

        recipeBookButton.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImageGo);
        });

        recipeBookCloseBtn.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImageGo);
        });

        _nextButton.onClick.AddListener(() =>
        {
            Debug.Log("���� �������� �Ѱ���ϴ�.");
        });

        //rewordUIGo.SetActive(false);
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

                string ingredientKoName = RecipeGameManager.Instance.IngredientInfoDic[sprite.name];
                int ingredientCount = 0;
                if (recipeGameManager.IngredientCountDic.ContainsKey(sprite.name))
                {
                    ingredientCount = RecipeGameManager.Instance.IngredientCountDic[sprite.name];
                }

                go.GetComponent<ItemInfo>().rewordItemName.text = ingredientKoName;
                go.GetComponent<ItemInfo>().rewordItemCount.text = ingredientCount.ToString() + "��";

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

        recipeBookImage.InitRecipeBook();
    }

    public  void ActiveRewordUI(string findedBreadName, bool isFinded)
    {

        if(findedBreadName == "")
        {
            rewordFailUIGo.SetActive(true);
            return;
        }

        rewordUIGo.SetActive(true);
        rewordUI.rewordItemImage.sprite = Resources.Load<Sprite>("Breads/" + findedBreadName);
        rewordUI.rewordItemName.text = RecipeGameManager.Instance.IndexInfoDic[findedBreadName];
        
        // �̹� ã�� ������
        if(isFinded)
        {
            rewordUI.title.text = "�̹� �˰��ִ� ������";
            rewordUI.title.fontSize = 50;
        }

    }
}

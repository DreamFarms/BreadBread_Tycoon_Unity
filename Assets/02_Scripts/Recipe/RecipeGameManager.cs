using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

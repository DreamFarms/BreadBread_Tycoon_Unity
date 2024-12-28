using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    [Header("Connection")]
    [SerializeField] private string _url = "";

    [Header("Info")]
    public string nickName;
    public int coin;
    public int cash;
    public float timer;
    public Dictionary<string, int> userIngredient = new Dictionary<string, int>();

    [Header("ExcelReader")]
    public Dictionary<string, ExcelReader2.Bread> breadInfoDic = new Dictionary<string, ExcelReader2.Bread>();
    public Dictionary<string, string> indexInfoDic = new Dictionary<string, string>(); // index : 인덱스
    public Dictionary<string, string> ingredientInfoDic = new Dictionary<string, string>(); // four : 밀가루

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timer = 300;
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Main"))
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
    }





    public string Url
    {
        get { return _url; }
    }




}

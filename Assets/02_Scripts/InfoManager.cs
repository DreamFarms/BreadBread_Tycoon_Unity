using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    private static InfoManager _instance;

    public Dictionary<string, string> enKoMappingDic = new Dictionary<string, string>();
    private Dictionary<string, string> _ingredientInfoDic = new Dictionary<string, string>();

    public static InfoManager Instance
    { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        //if (_instance == null)
        //{
        //    _instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("dsdsd");
            foreach (var pair in enKoMappingDic)
            {
                Debug.Log("in_foreach");
                Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
            }
            print(enKoMappingDic.Count);
        }
    }

    public void SetIngredientInfoDic(string enName, string koName)
    {
        if (!_ingredientInfoDic.ContainsKey(enName))
        {
            _ingredientInfoDic[enName] = koName;
        }
    }

    public void SetEnKoInfoDic(string enName, string koName)
    {
        print(enName);
        enKoMappingDic[enName] = koName;
    }

    public void SetEnKoInfoDicTest(Dictionary<string, string> Dic)
    {
        enKoMappingDic = Dic;
    }

    public void SetIngredientInfoDicTest(Dictionary<string,string> Dic)
    {
        _ingredientInfoDic = Dic;
    }

    // test
    //public void SetInfoManager(string key, string value)
    //{
    //    enKoMappingDic.Add(key, value);
    //}

    //public string nickName;
    //public int coin;
    //public int cash;
    //public float timer;
    //public Dictionary<string, int> userIngredient = new Dictionary<string, int>();

    //private void Start()
    //{
    //    timer = 300;
    //}

    //private void Update()
    //{
    //    if (!SceneManager.GetActiveScene().name.Equals("Main"))
    //    {
    //        timer -= Time.deltaTime;
    //        if (timer <= 0)
    //        {
    //            timer = 0;
    //        }
    //    }
    //}
}

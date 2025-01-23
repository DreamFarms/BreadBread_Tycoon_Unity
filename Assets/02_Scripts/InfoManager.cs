using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    private static InfoManager _instance;

    public Dictionary<string, string> enKoMappingDic = new Dictionary<string, string>();
    private Dictionary<string, string> _ingredientInfoDic = new Dictionary<string, string>();

    public static InfoManager Instance
    { get { return _instance; } }

    public string googleToken { get; private set; }
    public string accessToken { get; private set; }
    public string refreshToken { get; private set; }

    private string path;

    private class SaveData
    {
        public string accessToken;
        public string refreshToken;
    }

    private void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "database.json");
#elif UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "database.json");
#endif
        JsonLoad();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var pair in enKoMappingDic)
            {
                Debug.Log("in_foreach");
                Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
            }
            print(enKoMappingDic.Count);
        }
    }

    public bool JsonLoad()
    {
        SaveData saveData;

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                accessToken = saveData.accessToken;
                refreshToken = saveData.refreshToken;
                return true;
            }
        }
        return false;
    }

    public void JsonSave()
    {
        SaveData save = new SaveData();
        save.accessToken = googleToken;
        save.refreshToken = refreshToken;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(path, json);
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

    public void SetGoogleToken(string token)
    {
        googleToken = token;
    }

    public void SetToken(string accessToken, string refeshToken)
    {
        this.accessToken = accessToken;
        this.refreshToken = refeshToken;
        JsonSave();
        SceneManager_BJH.Instance.ChangeScene("01_Scenes/Map");
    }
}

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
    public string accessTokenTest { get; private set; } = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxMDg4ODA3MzE5MDExNDE0MjE2MzMiLCJpYXQiOjE3NDY3OTM1MTcsImV4cCI6MTc0OTM4NTUxN30.E-FdjINT7HSH82i32Dq7i65Ft2Duziq_ZBkCBkxnwdI";
    public string refreshTokenTest { get; private set; } = "gBzEqzdnNWR394ncl12fTRW7uwRIfC7CiDMoO8NhaI2yjJOYHxi92z3fYKLF5GrG8+5l3fbHP9lOTfeTuHGCEENw6ee490xDvx/i1bcvmyb7MSJu+/5bgzdyuwEmFM9zuQ0/KgNWlpIM+66528sXW2RQTAgiPwjP3f+0BQBdhB0iCyHNB7j/HQadyLDFABOi9hnPupNjkw/R1QCfbM97LwD67cxtHifRONeBIlwailQ=";
    public static InfoManager Instance { get { return _instance; } }

    public string googleToken { get; private set; }
    public string accessToken { get; private set; }
    public string refreshToken { get; private set; }

    private string path;

    // top ui
    public string NickName { get; private set; }
    public int Gold { get; private set; }  
    public int Cash { get; private set; }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnCashChanged;

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
        Debug.Log("accessToken: " + accessToken);
        SceneController.Instance.ChangeScene("01_Scenes/Map");
    }


    public void AddGold(int amount)
    {
        this.Gold += amount;
        amount = Math.Max(0, amount); // 음수 방지
        OnGoldChanged?.Invoke(Gold);
    }

    public bool SubtractGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            OnCashChanged?.Invoke(Cash);
            return true;
        }
        Debug.LogError("차감 할 골드가 부족합니다.");
        return false;

    }

    public void AddCash(int amount)
    {
        this.Cash += amount;
        amount = Math.Max(0, amount); // 음수 방지
        OnCashChanged?.Invoke(Cash);
    }

    public bool SubtractCash(int amount)
    {
        if (Cash >= amount)
        {
            Cash -= amount;
            OnCashChanged?.Invoke(Cash);
            return true;
        }
        Debug.LogError("차감 할 캐시가 부족합니다.");
        return false;
    }


    public void SetTopUI(string nickName, int gold, int cash)
    {
        this.NickName = nickName;
        this.Gold = gold;
        this.Cash = cash;
    }
}

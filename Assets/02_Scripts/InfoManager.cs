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
    public string accessTokenTest { get; private set; } = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4MjUwM2E1ZmQ1NmU5ZjczNGRmYmE1YzUwZDdiZjQ4ZGIyODRhZTkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIyNDgyNTI3OTg3MjUtYWlhMnBrMDE3ZGRzcTUzbGI2dmF1dm9qYnVycjUxZmwuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyNDgyNTI3OTg3MjUtbTAwbzQ4a2lyaThvZ2JlNmkxaGk3YjVnbjFtOWFoYW4uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDg4ODA3MzE5MDExNDE0MjE2MzMiLCJpYXQiOjE3NTEwMjU2OTIsImV4cCI6MTc1MTAyOTI5Mn0.tQugk7FzUAeE6Dbf2oGGJGapS3_3jTO-PJ0ZuZr98LE9h4O3P1S75czYzAfKLmeBZ1uqv1nPAf-fzFN_imMJKmXhHBYJtDy0iM9Qa_eFimL-RebCwBYS39SZTr7gT3mR_uB7ojJPXvTl1vNaudOEvLU5xvVmSWV-YjrL1hxUOCwiWfnkWNS_c0hNp4PJks2rqY2_pRfwS_G7LwurpiiPTJ8YdNVazmTc8DUEstssA2ew1vkGmpjFicfzwMxnmo1wFZNjMUhiYKr3andWbfWfeN86Ou2leZu1jQuj37LCUOhvDX-PEwsp2OBaL0_eKtgltqiJDJy6I1hDjYBnfRE-6Q";
    public string refreshTokenTest { get; private set; } = "gBzEqzdnNWR394ncl12fTRW7uwRIfC7CiDMoO8NhaI2yjJOYHxi92z3fYKLF5GrG8+5l3fbHP9lOTfeTuHGCEENw6ee490xDvx/i1bcvmyb7MSJu+/5bgzdyuwEmFM9zUFVUcIEu5mpduo5mL7G9b7+nlbTln9dPDoqb2m8LpRoOOXuBEyWcdX4DF8bE4O0lLcDOT5CfYN1EtZyBjb2K3jZyWUi08O5VeRYwjjn5RqQ=";
    public static InfoManager Instance { get { return _instance; } }

    // User 정보
    public string googleToken { get; private set; }
    public string accessToken { get; private set; }
    public string refreshToken { get; private set; }

    
    public long UserNo { get; private set; }

    private string path;

    // top ui
    public string NickName { get; private set; }
    public int Gold { get; private set; }  
    public int Cash { get; private set; }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnCashChanged;

    public string connectionPoint = "";

    private class SaveData
    {
        public string accessToken;
        public string refreshToken;
    }

    private void Awake()
    {
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
        //JsonLoad();
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
                Debug.Log($"자동 로그인 : {accessToken}");
                refreshToken = saveData.refreshToken;
                Debug.Log($"자동 로그인 : {refreshToken}");

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
        Debug.Log(accessToken + " " + refeshToken);
        this.accessToken = accessToken;
        this.refreshToken = refeshToken;
        //JsonSave();
        SceneController.Instance.LoadSceneWithLoading(SceneName.Map);
        Debug.Log("accessToken: " + accessToken);
    }

    public void SetUserNo(long userNo)
    {
        this.UserNo = userNo;
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

    public void SetNickName(string nickName)
    {
        this.NickName = nickName;
    }
}

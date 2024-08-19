using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class RewardSaveRequest
{
    public List<RewardInfo> rewards;
}

[System.Serializable]
public class RewardInfo
{
    public string name;
    public int count;
}



public class RewardSaveConnection
{
    string url;
    Dictionary<string, int> dicRewardInfo = new Dictionary<string, int>();

    public RewardSaveConnection(string url, Dictionary<string, int> dicRewardInfo)
    {
        this.url = url;
        this.dicRewardInfo = dicRewardInfo;

        CreateJson();
    }

    public void CreateJson()
    {
        RewardSaveRequest request = new RewardSaveRequest();
        
        request.rewards = new List<RewardInfo>();
        
        // 리스트 초기화
        foreach(var key in dicRewardInfo.Keys)
        {
            request.rewards.Add(new RewardInfo { name = key, count = dicRewardInfo[key] });
        }

        string createdJsonData = JsonUtility.ToJson(request, true);

        OnGetRequest(createdJsonData);
    }

    private void OnGetRequest(string jsonData)
    {
        HttpRequester requester = new HttpRequester(RequestType.POST, url, jsonData);
        requester.onComplete = OnGetComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.Instance.SendRequest(requester);
    }
    private void OnGetComplete(DownloadHandler handler)
    {
        Debug.Log("성공");


    }

    private void OnGetFailed(DownloadHandler handler)
    {
        Debug.Log("실패");
    }

}

public class HttpConnection : MonoBehaviour
{
    private static HttpConnection _instance;
    public static HttpConnection Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start() // 임시
    {
        string url = "https://b073-115-136-106-231.ngrok-free.app/api/v1/user/reward/save";
        Dictionary<string, int> dic = new Dictionary<string, int>();

        dic["strawberry"] = 2;
        dic["hiServer"] = 1;
        dic["youAlone"] = 0;

        RewardSaveConnection rewardSaveConnection = new RewardSaveConnection(url, dic);
    }
}

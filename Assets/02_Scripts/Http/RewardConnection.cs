using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class RewardSaveRequest
{
    public string nickname;
    public List<RewardInfo> rewards;
}

[System.Serializable]
public class RewardInfo
{
    public string name;
    public int count;
}


public class RewardConnection : MonoBehaviour
{
    private static RewardConnection _instance;
    public static RewardConnection Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            return;
        }
    }

    public void RewardSaveRequest(Dictionary<string, int> rewardDic)
    {
        string url = "https://b0cd-115-136-106-231.ngrok-free.app/api/v1/user/reward/save";

        RewardSaveRequest request = new RewardSaveRequest();
        request.nickname = GameManager.Instance.nickName;
        Debug.Log(request.nickname);
        request.rewards = new List<RewardInfo>();

        // 리스트 초기화
        foreach (var key in rewardDic.Keys)
        {
            request.rewards.Add(new RewardInfo { name = key, count = rewardDic[key] });
        }
        string createdJsonData = JsonUtility.ToJson(request, true);

        // requester
        HttpRequester requester = new HttpRequester(RequestType.POST, url, createdJsonData);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MilkConnection : MonoBehaviour
{
    public void RewardSaveRequest()
    {
        string url = "https://b0cd-115-136-106-231.ngrok-free.app/api/v1/user/reward/save";

        RewardSaveRequest request = new RewardSaveRequest();
        // request.nickname = GameManager.Instance.nickName;
        request.rewards = new List<RewardInfo>();

        request.rewards.Add(new RewardInfo { name = "Milk", count = MilkGameManager.Instance.currectMilkCount });
        
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

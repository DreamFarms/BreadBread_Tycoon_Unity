using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class InventoryRequest
{
    public string nickname;
}

[Serializable]
public class InventoryResponse
{
    public string resultCode;
    public List<InventoryResponseMessage> message;
}

[Serializable]
public class InventoryResponseMessage
{
    public int code;
    public string name;
    public int count;
}

public class InventoryConnection : MonoBehaviour
{
    public void StartInventoryConnection()
    {
        InventoryRequest request = new InventoryRequest();

        //request.nickname = GameManager.Instance.nickName;
        request.nickname = "웨지감자";

        string json = JsonUtility.ToJson(request);

        string url = "https://ec00-115-136-106-231.ngrok-free.app/api/v1/inventory/load?nickname=" + request.nickname;
        HttpRequester requester = new HttpRequester(RequestType.GET, url, json);
        requester.onComplete = OnCompleteInventoryConnection;
        requester.onFailed = OnFailedInventoryConnection;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnCompleteInventoryConnection(DownloadHandler result)
    {
        Debug.Log("인벤토리 통신 성공");
        InventoryResponse response = JsonUtility.FromJson<InventoryResponse>(result.text);
        for (int i = 0; i < response.message.Count; i++)
        {
            ItemManager.Instance.PlusItemCount(response.message[i].code, response.message[i].count);
        }
        Debug.Log("인벤토리 통신 끝");
    }

    public void OnFailedInventoryConnection(DownloadHandler result)
    {
        Debug.Log("인벤토리 통신 실패");
    }
}

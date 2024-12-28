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
    [SerializeField] private string inventoryPoint = "inventory/load?nickname=";

    public void StartInventoryConnection()
    {
        InventoryRequest request = new InventoryRequest();

        request.nickname = GameManager.Instance.nickName;

        string url = GameManager.Instance.Url + inventoryPoint + request.nickname;

        HttpRequester requester = new HttpRequester(RequestType.GET, url);
        requester.onComplete = OnComplete<InventoryResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        Debug.Log("인벤토리 통신 성공");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch(typeClass)
        {
            case InventoryResponse _:
                InventoryResponse response =  typeClass as InventoryResponse;
                if (response != null)
                {
                    for (int i = 0; i < response.message.Count; i++)
                    {
                        ItemManager.Instance.PlusItemCount(response.message[i].code, response.message[i].count);
                    }
                    Debug.Log("인벤토리 통신 끝");
                }
                break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("인벤토리 통신 실패");
    }
}

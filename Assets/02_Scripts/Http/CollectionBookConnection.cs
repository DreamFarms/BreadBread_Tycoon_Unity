using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class CollectionRequest
{
    public string nickname;
}

[Serializable]
public class CollectionResponse
{ 
    public List<CollectionResponseMessage> message;    
}

[System.Serializable]
public class CollectionResponseMessage
{
    public string name;
    public string intro;
    public List<string> preferredBreads;
    public bool preferredBreadsVisible;
    public int visitCount;
    public bool unlocked;
}

public class CollectionBookConnection : MonoBehaviour
{
    [SerializeField] private CollectionBookManager bookManager;

    private string collectionPoint = "/api/v1/npc/collection?nickname=";

    public void CollectionRequest()
    {
        CollectionRequest request = new CollectionRequest();

        request.nickname = GameManager.Instance.nickName;

        string url = GameManager.Instance.Url + collectionPoint + request.nickname;

        HttpRequester requester = new HttpRequester(RequestType.GET, url);
        requester.onComplete = OnGetComplete<CollectionResponse>;
        requester.onFailed = OnGetFailed;

        HttpManager.Instance.SendRequest(requester);

    }

    public void OnGetComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch (typeClass)
        {
            case CollectionResponse _:
                CollectionResponse response = typeClass as CollectionResponse;
                Debug.Log(response.message.Count);
                if (response != null)
                {
                    for (int i = 0; i < response.message.Count; i++)
                    {
                        bookManager.AddCustomer(response.message[i]);
                        Debug.Log(response.message[i].name);
                    }
                }
                break;
        }
    }

    private void OnGetFailed(DownloadHandler handler)
    {
        Debug.LogError("½ÇÆÐ");
    }
}

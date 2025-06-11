using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    private static HttpManager _instance;
    public static HttpManager Instance
    {
        get { return _instance; }
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
    }

    public void SendRequest(HttpRequester requester)
    {
        StartCoroutine(CoSendProcess(requester));
    }

    IEnumerator CoSendProcess(HttpRequester requester)
    {
        if(requester.url == null)
        {
            requester.url = GameManager.Instance.Url;
        }
        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.json);
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.json);
                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.SetRequestHeader("Content-Type", "application/json");
                break;
        }

        Debug.Log("통신 대기");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("통신 완료");
            requester.OnComplete(request.downloadHandler);
        }
        else
        {
            Debug.LogError("통신 실패");
            Debug.LogError("실패 사유 : " + request.error);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager1 : MonoBehaviour
{
    private static HttpManager1 _instance;
    public static HttpManager1 Instance
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

    public void SendRequest(HttpRequester1 requester)
    {
        StartCoroutine(CoSendProcess(requester));
    }

    IEnumerator CoSendProcess(HttpRequester1 requester)
    {
        if(requester.url == null)
        {
            requester.url = GameManager.Instance.Url;
        }
        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType1.GET:
                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType1.POST:
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
            Debug.Log("통신 실패");
            Debug.Log(request.error);
        }
    }
}

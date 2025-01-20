using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GoogleLoginRequest
{
    public string idToken;
}

[Serializable]
public class GoogleLoginResponse
{
    public string resultCode;
    public GoogleResponseMessage message;
}

[Serializable]
public class GoogleResponseMessage
{
    public string jwt;
    public string message;
    public string status;
}

public class GoogleLoginConnection : MonoBehaviour
{
    [SerializeField] private string googlePoint = "";

    public void StartGoogleLoginConnection()
    {
        GoogleLoginRequest request = new GoogleLoginRequest();

        request.idToken = InfoManager.Instance.googleToken;

        string json = JsonUtility.ToJson(request);

        string url = googlePoint;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete<GoogleLoginResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }


    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        Debug.Log("���� ��ū ��� ����");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch(typeClass)
        {
            case GoogleLoginResponse _:
                GoogleLoginResponse response = typeClass as GoogleLoginResponse;
                if (response != null)
                {
                    InfoManager.Instance.SetGoogleToken(response.message.jwt);
                    Debug.Log("�޼���" + response.message.message);
                    Debug.Log("�������ͽ�" + response.message.status);
                    Debug.Log("����" + response.message.jwt);
                }
                Debug.Log("���� ��ū ��� ��");
                break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("���� ��ū ��� ����");
    }
}

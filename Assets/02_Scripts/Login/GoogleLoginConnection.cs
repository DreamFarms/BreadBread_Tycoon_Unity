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
    public string status;
    public string message;
    public string accessToken;
    public string refreshToken;
}

public class GoogleLoginConnection : MonoBehaviour
{
    [SerializeField] private string googlePoint = "";
    private string login = "google-login/login";

    public void StartGoogleLoginConnection()
    {
        GoogleLoginRequest request = new GoogleLoginRequest();

        request.idToken = InfoManager.Instance.googleToken;

        string json = JsonUtility.ToJson(request);

        string url = googlePoint + login;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete<GoogleLoginResponse>;
        requester.onFailed = OnFailed;

        print(url);
        print(request.idToken);

        HttpManager.Instance.SendRequest(requester);
    }


    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        Debug.Log("구글 토큰 통신 성공");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch(typeClass)
        {
            case GoogleLoginResponse _:
                GoogleLoginResponse response = typeClass as GoogleLoginResponse;
                if (response != null)
                {
                    InfoManager.Instance.SetToken(response.message.accessToken, response.message.refreshToken);
                    Debug.Log("메세지" + response.message.message);
                    Debug.Log("스테이터스" + response.message.status);
                    Debug.Log("성공" + response.message.accessToken);
                    Debug.Log("성공" + response.message.refreshToken);
                }
                Debug.Log("구글 토큰 통신 끝");
                break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("구글 토큰 통신 실패");
    }
}

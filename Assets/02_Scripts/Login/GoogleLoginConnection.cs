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
    public long userNo;
    public string nickName;
    public int gold;
    public int cash;
    public ResponseInventory inventory;
    public ResponseUnlockedRecipes unlockedRecipes;
}

[Serializable]
public class ResponseUnlockedRecipes
{
    public int code;
    public string name;
    public int count;
}

[Serializable]
public class ResponseInventory
{
    public string foodName;
    public string category;
}

public class GoogleLoginConnection : MonoBehaviour
{
    private string login = "/google-login/login";
    private bool isFirst = false;

    public void StartGoogleLoginConnection()
    {
        GoogleLoginRequest request = new GoogleLoginRequest();

        request.idToken = InfoManager.Instance.googleToken;
        Debug.Log("서버로 요청 할 토큰 : " + request.idToken);

        string json = JsonUtility.ToJson(request);

        string url = InfoManager.Instance.connectionPoint + login;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete<GoogleLoginResponse>;
        requester.onFailed = OnFailed;

        Debug.Log(url);
        Debug.Log(request.idToken);

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
                    InfoManager.Instance.SetUserNo(response.message.userNo);
                    Debug.Log($"유저의 번호는 {response.message.userNo} 입니다.");
                    Debug.Log("메세지" + response.message.message);
                    Debug.Log("스테이터스" + response.message.status);
                    Debug.Log("성공" + response.message.accessToken);
                    Debug.Log("성공" + response.message.refreshToken);

                    // Top ui 설정
                    GoogleResponseMessage message = response.message;
                    InfoManager.Instance.SetTopUI(message.nickName, message.gold, message.cash);
                    Debug.Log($"{message.nickName}님의 골드는 {message.gold} 그리고 캐시는 {message.cash} 입니다.");
                    

                    if (message.nickName == "빵빵빵의신규빵집")
                    {
                        isFirst = true;
                        Debug.Log("닉네임 바꾸기" + message.nickName);
                        Debug.Log("isFirst = " + isFirst);
                        GoogleTest test = FindAnyObjectByType<GoogleTest>();
                        test.nickNameLogin.SetActive(true);
                        Debug.Log("구글 토큰 통신 끝");

                        break;
                    }
                    else
                    {
                        SceneController.Instance.LoadSceneWithLoading(SceneName.Map);
                        
                        Debug.Log("구글 토큰 통신 끝2");
                    }
                }
                 break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("구글 토큰 통신 실패");
    }
}

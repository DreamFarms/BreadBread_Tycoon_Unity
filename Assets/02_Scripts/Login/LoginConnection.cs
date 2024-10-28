using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class LoginRequest
{
    public string nickname;
}

[Serializable]
public class LoginResponse
{
    public string resultcode;
    public LoginMessage message;
}

[Serializable]
public class LoginMessage
{
    public string nickname;
    public string message;
}


public class LoginConnection : MonoBehaviour
{
    [SerializeField] private string loginUrl = "api/v1/user/save";

    public void LoginRequest(string inputNickname)
    {
        LoginRequest request = new LoginRequest();
        request.nickname = inputNickname;

        string json = JsonUtility.ToJson(request);

        string url = GameManager.Instance.Url + loginUrl;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);

    }
    public void OnComplete(DownloadHandler handler)
    {
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(handler.text);
        GameManager.Instance.nickName = response.message.nickname;

        LoginUIManager.Instance.alertImageGo.SetActive(true);
        // 로그인이라면?
        if (response.message.message.Contains("Login"))
        {
            LoginUIManager.Instance.alertText.text = "존재하는 계정\n 본인이 맞다면 로그인\n 아니라면 회원가입하세요.";
            LoginUIManager.Instance.alerBackButton.gameObject.SetActive(true);

        }
        else
        {
            LoginUIManager.Instance.alertText.text = "존재하지 않는 계정\n 회원가입 합니다.";
            LoginUIManager.Instance.alerBackButton.gameObject.SetActive(false);
        }
    }

    private void OnFailed(DownloadHandler handler)
    {
        Debug.Log("로그인 실패");
    }
}

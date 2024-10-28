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
        // �α����̶��?
        if (response.message.message.Contains("Login"))
        {
            LoginUIManager.Instance.alertText.text = "�����ϴ� ����\n ������ �´ٸ� �α���\n �ƴ϶�� ȸ�������ϼ���.";
            LoginUIManager.Instance.alerBackButton.gameObject.SetActive(true);

        }
        else
        {
            LoginUIManager.Instance.alertText.text = "�������� �ʴ� ����\n ȸ������ �մϴ�.";
            LoginUIManager.Instance.alerBackButton.gameObject.SetActive(false);
        }
    }

    private void OnFailed(DownloadHandler handler)
    {
        Debug.Log("�α��� ����");
    }
}

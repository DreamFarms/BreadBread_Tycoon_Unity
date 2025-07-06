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
        Debug.Log("������ ��û �� ��ū : " + request.idToken);

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
        Debug.Log("���� ��ū ��� ����");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch(typeClass)
        {
            case GoogleLoginResponse _:
                GoogleLoginResponse response = typeClass as GoogleLoginResponse;
                if (response != null)
                {
                    InfoManager.Instance.SetToken(response.message.accessToken, response.message.refreshToken);
                    InfoManager.Instance.SetUserNo(response.message.userNo);
                    Debug.Log($"������ ��ȣ�� {response.message.userNo} �Դϴ�.");
                    Debug.Log("�޼���" + response.message.message);
                    Debug.Log("�������ͽ�" + response.message.status);
                    Debug.Log("����" + response.message.accessToken);
                    Debug.Log("����" + response.message.refreshToken);

                    // Top ui ����
                    GoogleResponseMessage message = response.message;
                    InfoManager.Instance.SetTopUI(message.nickName, message.gold, message.cash);
                    Debug.Log($"{message.nickName}���� ���� {message.gold} �׸��� ĳ�ô� {message.cash} �Դϴ�.");
                    

                    if (message.nickName == "�������ǽűԻ���")
                    {
                        isFirst = true;
                        Debug.Log("�г��� �ٲٱ�" + message.nickName);
                        Debug.Log("isFirst = " + isFirst);
                        GoogleTest test = FindAnyObjectByType<GoogleTest>();
                        test.nickNameLogin.SetActive(true);
                        Debug.Log("���� ��ū ��� ��");

                        break;
                    }
                    else
                    {
                        SceneController.Instance.LoadSceneWithLoading(SceneName.Map);
                        
                        Debug.Log("���� ��ū ��� ��2");
                    }
                }
                 break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("���� ��ū ��� ����");
    }
}

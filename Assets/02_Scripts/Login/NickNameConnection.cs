using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class NickNameRequest
{
    public string nickname;
}

[Serializable]
public class NickNameResponse
{
    public string resultCode;
    public NickNameResponseMessage message;
}

[Serializable]
public class NickNameResponseMessage
{
    public string nickName;
    public string message;
}

public class NickNameConnection : MonoBehaviour
{
    private string nickNameUrl = "/api/v1/user/nickname/save";

    public void StartNickNameConnection(string nickName)
    {
        NickNameRequest request = new NickNameRequest();

        request.nickname = nickName;

        string json = JsonUtility.ToJson(request);

        string url = InfoManager.Instance.connectionPoint + nickNameUrl;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete<NickNameResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }


    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        Debug.Log("구글 토큰 통신 성공");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch (typeClass)
        {
            case NickNameResponse _:
                NickNameResponse response = typeClass as NickNameResponse;
                if (response != null)
                {
                    NickNameResponseMessage message = response.message;
                    InfoManager.Instance.SetNickName(message.nickName);
                }
                SceneController.Instance.LoadSceneWithLoading(SceneName.Map);
                break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("구글 토큰 통신 실패");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;


// ������ ��û���� ���� Ȯ�ο����� ���۵� Ŭ�����Դϴ�.
// ���� ���忡�� ������� �ʽ��ϴ�.

[Serializable]
public class TopUIResponse
{
    public string resultCode;
    public TopUIMessage message;
}

[Serializable]
public class TopUIMessage
{
    public string nickName;
    public int cash;
    public int gold;
}

public class TopUITestConnection : MonoBehaviour
{
    [SerializeField] private string url;

    private void Start()
    {
        StartTopUIConnection();
    }

    private void StartTopUIConnection()
    {
        CreateJson();
    }

    public void CreateJson()
    {
        string userNo = InfoManager.Instance.UserNo.ToString();
        string finalUrl = url + $"/api/v1/user/{userNo}/info";

        HttpRequester requester = new HttpRequester(RequestType.GET, finalUrl);
        requester.onComplete = OnComplete<TopUIResponse>;

        HttpManager.Instance.SendRequest(requester);
    }

    private void OnComplete<T> (DownloadHandler handler) where T : new()
    {
        T typeClass = new T();
        typeClass = JsonUtility.FromJson<T>(handler.text);

        TopUIResponse resultResonse = typeClass as TopUIResponse;

        string nickName = resultResonse.message.nickName;
        int cash = resultResonse.message.cash;
        int gold = resultResonse.message.gold;
        Debug.Log($"������� �г��� : {nickName}   GOLD : {gold}   CASH : {cash}");

        InfoManager.Instance.SetTopUI(nickName, gold, cash);
        
    }
}

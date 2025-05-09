using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class BreadSellRequest
{
    public string nickname;
    public string name;
    public int quantity;
}

[Serializable]
public class BreadSellResponse
{
    public string resultCode;
    public string message;
}

public class BreadSellConnection : MonoBehaviour
{
    [SerializeField] private string breadSellPoint = "";

    public void StartBreadSellConnection(Stack bread)
    {
        BreadSellRequest request = new BreadSellRequest();

        request.nickname = GameManager.Instance.nickName;
        myBread myBread = (myBread)bread.Pop();
        print(myBread.breadName);
        request.name = myBread.breadName;
        request.quantity = 1;
        MoneyManager.Instance.SetMoney(myBread.money, MoneyManager.CalculateState.Plus);

        string json = JsonUtility.ToJson(request);

        string url = GameManager.Instance.Url + breadSellPoint;
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete<BreadSellResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }


    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        Debug.Log("�� �ȱ� ��� ����");
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch (typeClass)
        {
            case BreadSellResponse _:
                BreadSellResponse response = typeClass as BreadSellResponse;
                if (response != null)
                {
                    Debug.Log("�� �Ǹ� �亯�� �����ϴ�.");
                }
                Debug.Log("�� �Ǹ� ��� ��");
                break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("�� �Ǹ� ��� ����");
    }
}

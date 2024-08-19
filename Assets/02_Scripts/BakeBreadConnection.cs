using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region ��� Ŭ����
[Serializable]
public class FoodCheckRequest
{
    public string nickname;
    public string foodName;
}

[Serializable]
public class FoodCheckResponse
{
    public string resultCode;
    public FoodCheckResponseMessage foodCheckResponseMessage;
}

[Serializable]
public class FoodCheckResponseMessage
{
    public string message;
    public List<FoodIngredients> foodIngredients;
    public List<UserRewards> userRewards;
}

[Serializable]
public class UserRewards
{
    public string name;
    public int quantity;
}

[Serializable]
public class FoodIngredients
{
    public string rewardName;
    public int rewardCount;
}
#endregion

public class BakeBreadConnection : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            StartBakeBreadConnection();
        }

    }

    // �ӽ÷� ���� ���� ��� ��
    // �� ���Ƿ� ����� ��� ����
    public void StartBakeBreadConnection()
    {
        FoodCheckRequest request = new FoodCheckRequest();
        request.nickname = "kny";
        request.foodName = "Chocolate Roll Cake";

        string jsonData = JsonUtility.ToJson(request, true);

        string url = "http://ec2-13-124-19-125.ap-northeast-2.compute.amazonaws.com:8081/api/v1/food/check";
        HttpRequester requester = new HttpRequester(RequestType.POST, url, jsonData);
        requester.onComplete = OnComplete;
        requester.onComplete = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete(DownloadHandler result)
    {
        Debug.Log("�����߽��ϴ�");

        FoodCheckResponse response = JsonUtility.FromJson<FoodCheckResponse>(result.text);
        Debug.Log(response.resultCode);
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("�����߽��ϴ�.");
    }
}

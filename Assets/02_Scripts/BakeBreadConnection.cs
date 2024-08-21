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
    public FoodCheckResponseMessage message;
}

[Serializable]
public class FoodCheckResponseMessage
{
    public string message;
    public List<FoodIngredients> foodIngredients;
    public List<UserRewards> userRewards;
}

[Serializable]
public class FoodIngredients
{
    public string name;
    public int quantity;
}

[Serializable]
public class UserRewards
{
    public string rewardName;
    public int rewardCount;
}
#endregion

public class BakeBreadConnection : MonoBehaviour
{
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    StartBakeBreadConnection();
        //}

    }

    // �ӽ÷� ���� ���� ��� ��
    // �� ���Ƿ� ����� ��� ����
    public void StartBakeBreadConnection()
    {
        FoodCheckRequest request = new FoodCheckRequest();
        request.nickname = "kny";
        request.foodName = BakeBreadManager.Instance.selectedBreadName;

        string jsonData = JsonUtility.ToJson(request, true);

        string url = "http://ec2-13-124-19-125.ap-northeast-2.compute.amazonaws.com:8081/api/v1/food/check";
        HttpRequester requester = new HttpRequester(RequestType.POST, url, jsonData);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete(DownloadHandler result)
    {
        Debug.Log("�� ���� ���� ����� �����߽��ϴ�");

        FoodCheckResponse response = JsonUtility.FromJson<FoodCheckResponse>(result.text);

        Debug.Log(response.message.userRewards.ToString());
        
        // �� ���� ���ɽ�
        if(response.message.userRewards.Count > 0)
        {
            if(BakeBreadManager.Instance.targetIngredientDic.Count > 0)
            {
                BakeBreadManager.Instance.targetIngredientDic.Clear();
            }

            if (BakeBreadManager.Instance.userIngredientDic.Count > 0)
            {
                BakeBreadManager.Instance.userIngredientDic.Clear();
            }

            // �� ������
            foreach (var foodIngredient in response.message.foodIngredients)
            {
                string ingredientName = foodIngredient.name;
                int ingredientCount = foodIngredient.quantity;
                BakeBreadManager.Instance.targetIngredientDic.Add(ingredientName, ingredientCount); // �� 1���� ���� �� �ʿ��� ��� : ����
            }

            // ���� ������ ���� ���
            foreach(var userIngredient in response.message.userRewards)
            {
                string rewardName = userIngredient.rewardName;
                int rewardCount = userIngredient.rewardCount;
                BakeBreadManager.Instance.userIngredientDic.Add(rewardName, rewardCount); // ���� ������ ���� ��� : ����
            }
            UIManager.Instance.blackBGImage.SetActive(false);
            BakeBreadManager.Instance.StartBakeBreadGame();
        }
        else // �� ���� �Ұ��ɽ�
        {
            Debug.Log("��ᰡ �����մϴ�. �� ������");
            UIManager.Instance.SetErrorImage("��ᰡ �����Ͽ�\n ���� ���� �� �����ϴ�.", "��!");
            UIManager.Instance.errorImage.SetActive(true);
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("�����߽��ϴ�.");
    }
}

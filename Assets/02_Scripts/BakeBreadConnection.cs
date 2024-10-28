using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

[Serializable]
public class FoodUpdateRequest
{
    public string nickname;
    public string foodName;
    public List<FoodUpdateRemainingIngredients> remainingIngredients;
    public int breadCount;
}

[Serializable]
public class FoodUpdateRemainingIngredients
{
    public string ingredientName;
    public int remainingQuantity;
}

[Serializable]
public class FoodUpdateResponse
{
    public string resultCode;
    public FoodUpdateResponseMessage message;
}

[Serializable]
public class FoodUpdateResponseMessage
{
    public DateTime saveTime;
    public bool saved;
}

public class BakeBreadConnection : MonoBehaviour
{
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    StartBakeBreadConnection();
        //}

    }

    #region �� ����� �� ���
    // �ӽ÷� ���� ���� ��� ��
    // �� ���Ƿ� ����� ��� ����
    public void StartBakeBreadConnection()
    {
        FoodCheckRequest request = new FoodCheckRequest();
        request.nickname = InfoManager.Instance.nickName;
        request.foodName = BakeBreadManager.Instance.selectedBreadName;

        string jsonData = JsonUtility.ToJson(request, true);

        string url = "https://b0cd-115-136-106-231.ngrok-free.app/api/v1/food/check";
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

            //BakeBreadManager.Instance.userIngredientDic.Clear();
            // ���� ������ ���� ���
            foreach (var userIngredient in response.message.userRewards)
            {
                string rewardName = userIngredient.rewardName;
                int rewardCount = userIngredient.rewardCount;
                BakeBreadManager.Instance.userIngredientDic.Add(rewardName, rewardCount); // ���� ������ ���� ��� : ����
                Debug.Log($"{rewardName}�� {rewardCount}�� ������ �ִ�.");
            }
            UIManager.Instance.blackBGImage.SetActive(false);
            BakeBreadManager.Instance.StartBakeBreadGame();
            BakeBreadManager.Instance.isPlay = true;
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
    #endregion

    public void EndBakeBread(string selectedBreadName, Dictionary<string, int> remainIngredientDic, int bakedBreadCount)
    {
        FoodUpdateRequest request = new FoodUpdateRequest();

        request.nickname = InfoManager.Instance.nickName;
        request.foodName = selectedBreadName;

        request.remainingIngredients = new List<FoodUpdateRemainingIngredients>(); // �ʱ�ȭ
        foreach (var item in remainIngredientDic)
        {
            FoodUpdateRemainingIngredients remain = new FoodUpdateRemainingIngredients();
            remain.ingredientName = item.Key;
            remain.remainingQuantity = item.Value;
            Debug.Log(remain.ingredientName + "  " + remain.remainingQuantity);
            request.remainingIngredients.Add(remain);
        }

        request.breadCount = bakedBreadCount;

        string json = JsonUtility.ToJson(request);

        string url = "https://b0cd-115-136-106-231.ngrok-free.app/api/v1/food/update";
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnCompleteFoodUpdate;
        requester.onFailed = OnFailedFoodUpdate;

        HttpManager.Instance.SendRequest(requester);

    }

    public void OnCompleteFoodUpdate(DownloadHandler result)
    {
        Debug.Log("���� ������Ʈ ����� �Ϸ��߽��ϴ�.");
        FoodUpdateResponse response = JsonUtility.FromJson<FoodUpdateResponse>(result.text);
        Debug.Log($"result code : {response.resultCode}, save time : {response.message.saveTime}, is save : {response.message.saved}");
    }

    public void OnFailedFoodUpdate(DownloadHandler result)
    {
        Debug.Log("���� ������Ʈ ��� ����");
    }
}

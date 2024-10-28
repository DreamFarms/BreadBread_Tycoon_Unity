using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

#region 통신 클래스
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

    #region 빵 만들기 전 통신
    // 임시로 유저 저장 통신 후
    // 빵 임의로 만들기 통신 시작
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
        Debug.Log("빵 제작 여부 통신을 성공했습니다");

        FoodCheckResponse response = JsonUtility.FromJson<FoodCheckResponse>(result.text);

        Debug.Log(response.message.userRewards.ToString());
        
        // 빵 제작 가능시
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

            // 빵 레시피
            foreach (var foodIngredient in response.message.foodIngredients)
            {
                string ingredientName = foodIngredient.name;
                int ingredientCount = foodIngredient.quantity;
                BakeBreadManager.Instance.targetIngredientDic.Add(ingredientName, ingredientCount); // 빵 1개를 만들 때 필요한 재료 : 개수
            }

            //BakeBreadManager.Instance.userIngredientDic.Clear();
            // 실제 유저가 가진 재료
            foreach (var userIngredient in response.message.userRewards)
            {
                string rewardName = userIngredient.rewardName;
                int rewardCount = userIngredient.rewardCount;
                BakeBreadManager.Instance.userIngredientDic.Add(rewardName, rewardCount); // 실제 유저가 가진 재료 : 개수
                Debug.Log($"{rewardName}를 {rewardCount}개 가지고 있다.");
            }
            UIManager.Instance.blackBGImage.SetActive(false);
            BakeBreadManager.Instance.StartBakeBreadGame();
            BakeBreadManager.Instance.isPlay = true;
        }
        else // 빵 제작 불가능시
        {
            Debug.Log("재료가 부족합니다. 빵 못만듦");
            UIManager.Instance.SetErrorImage("재료가 부족하여\n 빵을 만들 수 없습니다.", "네!");
            UIManager.Instance.errorImage.SetActive(true);
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("실패했습니다.");
    }
    #endregion

    public void EndBakeBread(string selectedBreadName, Dictionary<string, int> remainIngredientDic, int bakedBreadCount)
    {
        FoodUpdateRequest request = new FoodUpdateRequest();

        request.nickname = InfoManager.Instance.nickName;
        request.foodName = selectedBreadName;

        request.remainingIngredients = new List<FoodUpdateRemainingIngredients>(); // 초기화
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
        Debug.Log("음식 업데이트 통신을 완료했습니다.");
        FoodUpdateResponse response = JsonUtility.FromJson<FoodUpdateResponse>(result.text);
        Debug.Log($"result code : {response.resultCode}, save time : {response.message.saveTime}, is save : {response.message.saved}");
    }

    public void OnFailedFoodUpdate(DownloadHandler result)
    {
        Debug.Log("음식 업데이트 통신 실패");
    }
}

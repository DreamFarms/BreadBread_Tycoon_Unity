using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RecipeGameStartResponse
{
    public string resultCode;
    public RecipeGameStartResponseMessage message;
}

[System.Serializable]
public class RecipeGameStartResponseMessage
{
    public List<RecipeGameStartResponseMessageIngredient> ingredients;
    public List<string> unlockedRecipes;
}

[System.Serializable]
public class RecipeGameStartResponseMessageIngredient
{
    public string ingredientName;
    public int count;
}

[System.Serializable]
public class RecipeGameResultRequest
{
    public string nickname;
    public List<RecipeGameResultRequestIngredient> ingredients;
}

[System.Serializable]
public class RecipeGameResultRequestIngredient
{
    public string ingredientName;
    public int quantity;
}

[System.Serializable]
public class RecipeGameResultResponse
{
    public string resultCode;
    public RecipeGameResultResponseMessage message;
}

[System.Serializable]
public class RecipeGameResultResponseMessage
{
    public bool result;
    public string breadName;
}

public class RecipeConnection : MonoBehaviour
{
    [SerializeField]
    // 임시 
    private string url = "https://2627-115-136-106-231.ngrok-free.app/";
    private string endPoint = "api/v1/recipes/start?nickname=";
    private string nickName = "123"; // 임시

    public void StartRecipeConnection()
    {
        string getUrl = url + endPoint + nickName;
        HttpRequester requester = new HttpRequester(RequestType.GET, getUrl);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void RecipeGameResultConnection()
    {
        string getUrl = url + "api/v1/recipes/check";
        RecipeGameResultRequest request = new RecipeGameResultRequest();
        request.nickname = nickName;
        request.ingredients = new List<RecipeGameResultRequestIngredient>(); // 리스트 초기화

        // 임시
        RecipeGameResultRequestIngredient test1 = new RecipeGameResultRequestIngredient();
        test1.ingredientName = "Flour_Red";
        test1.quantity = 2;
        request.ingredients.Add(test1);

        RecipeGameResultRequestIngredient test2 = new RecipeGameResultRequestIngredient();
        test2.ingredientName = "Butter";
        test2.quantity = 1;  // 원래 1
        request.ingredients.Add(test2);

        RecipeGameResultRequestIngredient test3 = new RecipeGameResultRequestIngredient();
        test3.ingredientName = "Egg";
        test3.quantity = 1;
        request.ingredients.Add(test3);

        RecipeGameResultRequestIngredient test4 = new RecipeGameResultRequestIngredient();
        test4.ingredientName = "Salt";
        test4.quantity = 1;
        request.ingredients.Add(test4);

        Debug.Log(request.ingredients.Count.ToString());
        string jsonData = JsonUtility.ToJson(request, true);
        Debug.Log(jsonData.ToString());

        HttpRequester requester = new HttpRequester(RequestType.POST, getUrl, jsonData);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete(DownloadHandler result)
    {
        Debug.Log("완료");
        Debug.Log(result.text);
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("실패");
    }
}

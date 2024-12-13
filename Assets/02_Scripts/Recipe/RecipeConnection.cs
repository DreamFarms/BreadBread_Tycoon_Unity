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
    [SerializeField] private string StartRecipeEndPoint = "api/v1/recipes/start?nickname=";
    [SerializeField] private string recipeGameEndPoint = "api/v1/recipes/check";

    

    public void StartRecipeConnection()
    {
        string url = GameManager.Instance.Url + StartRecipeEndPoint + GameManager.Instance.nickName;
        HttpRequester requester = new HttpRequester(RequestType.GET, url);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    // 재료를 제출해서 레시피를 알아내는 메서드
    public void RecipeGameResultConnection(List<string> selectedIngredients)
    {
        string url = GameManager.Instance.Url + recipeGameEndPoint;
        RecipeGameResultRequest request = new RecipeGameResultRequest();
        request.nickname = GameManager.Instance.nickName;

        request.ingredients = new List<RecipeGameResultRequestIngredient>(); // 리스트 초기화

        foreach(string ingredientName in selectedIngredients)
        {
            RecipeGameResultRequestIngredient requestIngredient = new RecipeGameResultRequestIngredient();
            requestIngredient.ingredientName = ingredientName;
            requestIngredient.quantity = 1; // 1개 고정(이후 밸런스 피드백 후 개수 조절 필요)
            request.ingredients.Add(requestIngredient);
        }

        string jsonData = JsonUtility.ToJson(request, true);

        HttpRequester requester = new HttpRequester(RequestType.POST, url, jsonData);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete(DownloadHandler result)
    {
        Debug.Log("완료");
        Debug.Log(result.text);

        // 동작 위치 이동 필요해 보임
        // "resultCode":"SUCCESS","message":{"ingredients":[{"ingredientName":"Flour","count":10},{"ingredientName":"Flour_Green","count":10},{"ingredientName":"Flour_Red","count":10},{"ingredientName":"Salt","count":10},{"ingredientName":"Sugar","count":10},{"ingredientName":"Butter","count":10},{"ingredientName":"Egg","count":10},{"ingredientName":"Milk","count":10},{"ingredientName":"Strawberry","count":10}],"unlockedRecipes":[]}}
        string jsonResult = result.text;
        RecipeGameStartResponse response = JsonUtility.FromJson<RecipeGameStartResponse>(jsonResult);

        foreach(RecipeGameStartResponseMessageIngredient ingredient in response.message.ingredients)
        {
            string name = ingredient.ingredientName;
            int count = ingredient.count;
            RecipeGameManager.Instance.IngredientCountDic.Add(name, count);
        }



    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("실패");
    }
}

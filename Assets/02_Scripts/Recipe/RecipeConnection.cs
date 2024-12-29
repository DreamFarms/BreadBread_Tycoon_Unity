using JetBrains.Annotations;
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
    public List<UnLockedRecipe> unlockedRecipes;
}

[System.Serializable]
public class UnLockedRecipe
{
    public string foodName;
    public string category;
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
    public int resultState; // -1 : �̹� ã��, 0 : ��ã��, 1 : ���� ã��
    public string breadName;
    public string category;
}

public class RecipeConnection : MonoBehaviour, IConnection
{
    [SerializeField] private string StartRecipeEndPoint = "api/v1/recipes/start?nickname=";
    [SerializeField] private string recipeGameEndPoint = "api/v1/recipes/check";

    

    public void StartRecipeConnection()
    {
        string url = GameManager.Instance.Url + StartRecipeEndPoint + GameManager.Instance.nickName;
        HttpRequester requester = new HttpRequester(RequestType.GET, url);
        requester.onComplete = OnComplete<RecipeGameStartResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    // ��Ḧ �����ؼ� �����Ǹ� �˾Ƴ��� �޼���
    public void RecipeGameResultConnection(List<string> selectedIngredients)
    {
        string url = GameManager.Instance.Url + recipeGameEndPoint;
        RecipeGameResultRequest request = new RecipeGameResultRequest();
        request.nickname = GameManager.Instance.nickName;

        request.ingredients = new List<RecipeGameResultRequestIngredient>(); // ����Ʈ �ʱ�ȭ

        foreach(string ingredientName in selectedIngredients)
        {
            RecipeGameResultRequestIngredient requestIngredient = new RecipeGameResultRequestIngredient();
            requestIngredient.ingredientName = ingredientName;
            requestIngredient.quantity = 1; // 1�� ����(���� �뷱�� �ǵ�� �� ���� ���� �ʿ�)
            request.ingredients.Add(requestIngredient);
        }

        string jsonData = JsonUtility.ToJson(request, true);

        HttpRequester requester = new HttpRequester(RequestType.POST, url, jsonData);
        requester.onComplete = OnComplete<RecipeGameResultResponse>;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete<T>(DownloadHandler result) where T : new()
    {
        T typeClass = new T();
        typeClass = JsonUtility.FromJson<T>(result.text);

        switch(typeClass)
        {
            case RecipeGameStartResponse _:
                RecipeGameStartResponse startResponse = typeClass as RecipeGameStartResponse;

                if(startResponse != null)
                {
                    foreach (RecipeGameStartResponseMessageIngredient ingredient in startResponse.message.ingredients)
                    {
                        string name = ingredient.ingredientName;
                        int count = ingredient.count;
                        RecipeGameManager.Instance.IngredientCountDic.Add(name, count);
                    }
                    
                    foreach(UnLockedRecipe findedRecipeName in startResponse.message.unlockedRecipes)
                    {
                        RecipeGameManager.Instance.findedRecipes.Add(findedRecipeName.foodName);
                    }
                }
                break;

            case RecipeGameResultResponse _:
                RecipeGameResultResponse resultResponse = typeClass as RecipeGameResultResponse;

                // �ڵ带 ����ϰ� ���� �� �ʿ䰡 ����
                // fasle, true�� ���� �Ѱ��ִ� ����ۿ� ������? ���
                if (resultResponse != null)
                {
                    string breadName = resultResponse.message.breadName;
                    if(resultResponse.message.resultState == 1)
                    {
                        Debug.Log("���� ���� �� : " + breadName);
                        RecipeUIManager.Instance.ActiveRewordUI(breadName, 1);
                    }
                    else if(resultResponse.message.resultState == 0)
                    {
                        Debug.Log("�����ǰ� �����ϴ�.");
                        breadName = null;
                        RecipeUIManager.Instance.ActiveRewordUI(breadName, 0);
                    }
                    else
                    {
                        Debug.Log("�̹� ã�� �����Դϴ�.");
                        RecipeUIManager.Instance.ActiveRewordUI(breadName, -1);
                    }

                }
                    break;
        }
    }

    public void OnFailed(DownloadHandler result)
    {
        Debug.Log("����");
    }
}

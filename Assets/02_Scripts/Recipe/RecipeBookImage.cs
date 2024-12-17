using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookImage : MonoBehaviour
{
    public Button closeButton;

    public GameObject recipeBookPage; // prefab
    public GameObject basicPage; // go
    public GameObject category01, category02;
    public List<string> findedRecipes = new List<string>();

    private void Awake()
    {
        category01.SetActive(true);
        category02.SetActive(false);



    }

    public void InitRecipeBook()
    {
        findedRecipes = RecipeGameManager.Instance.findedRecipes;

        if (findedRecipes.Count == 0)
        {
            RecipeBookPage page = basicPage.GetComponent<RecipeBookPage>();
            page.recipeImage.gameObject.SetActive(false);
            page.recipeName.text = null;
            page.recipeInfo.text = "아직 발견한 레시피가 없습니다.";
            basicPage.SetActive(true);
        }
        else
        {
            // todo
            // 찾은 레시피가 존재하면 어떤 행위를 할 것인지 추가
            foreach (string findedRecipeName in findedRecipes)
            {
                RecipeBookPage page = basicPage.GetComponent<RecipeBookPage>();
                page.recipeImage.gameObject.SetActive(true);
                page.recipeImage.sprite = Resources.Load<Sprite>("Breads/" + findedRecipeName);
                page.recipeName.text = GameManager.Instance.menuInfoDic[findedRecipeName].koName;
                page.recipeInfo.text = GameManager.Instance.menuInfoDic[findedRecipeName].description;
                basicPage.SetActive(true);
            }
        }
    }

    public void AddRecipeOnRecipeBook()
    {
        // 수정 필요
        GameObject go = GameObject.Instantiate(recipeBookPage, category01.transform);
        RecipeBookPage page = go.GetComponent<RecipeBookPage>();
        page.recipeImage.sprite = Resources.Load<Sprite>("Icebox_Strawberry");
        page.recipeName.text = "딸기 아이스박스";
        page.recipeInfo.text = "맞아요. 바바(baba)가 광고하는\n 그 아이스크림 케이크!";
        go.name = "딸기 아이스박스 레시피";
        go.SetActive(true); 
    }
}

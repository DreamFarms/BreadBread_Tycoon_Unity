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
            page.recipeInfo.text = "���� �߰��� �����ǰ� �����ϴ�.";
            basicPage.SetActive(true);
        }
        else
        {
            // todo
            // ã�� �����ǰ� �����ϸ� � ������ �� ������ �߰�
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
        // ���� �ʿ�
        GameObject go = GameObject.Instantiate(recipeBookPage, category01.transform);
        RecipeBookPage page = go.GetComponent<RecipeBookPage>();
        page.recipeImage.sprite = Resources.Load<Sprite>("Icebox_Strawberry");
        page.recipeName.text = "���� ���̽��ڽ�";
        page.recipeInfo.text = "�¾ƿ�. �ٹ�(baba)�� �����ϴ�\n �� ���̽�ũ�� ����ũ!";
        go.name = "���� ���̽��ڽ� ������";
        go.SetActive(true); 
    }
}

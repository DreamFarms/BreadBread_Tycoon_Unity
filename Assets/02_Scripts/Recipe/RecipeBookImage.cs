using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookImage : MonoBehaviour
{
    public Button closeButton;

    public GameObject recipeBookPageGo; // prefab
    public GameObject basicPage; // go
    public GameObject category01, category02; // assign
    public GameObject currentCategory;
    public List<string> findedRecipes = new List<string>();

    private void Awake()
    {
        category01.SetActive(true);
        category02.SetActive(false);
        currentCategory = category01;
        
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
                GameObject page = Instantiate(recipeBookPageGo, category01.transform); // �ӽ÷� category01�� ����

                // page ����
                RecipeBookPage recipeBookPage = page.GetComponent<RecipeBookPage>();
                recipeBookPage.recipeImage.gameObject.SetActive(true);
                recipeBookPage.SetRecipeBookPage(findedRecipeName);

                // page�� ī�װ��� ����
                category01.GetComponent<Category>().AddPages(recipeBookPage);
            }
            category01.GetComponent<Category>().InitPage();

        }
    }

    public void AddRecipeOnRecipeBook()
    {
        // ���� �ʿ�
        GameObject go = GameObject.Instantiate(recipeBookPageGo, category01.transform);
        RecipeBookPage page = go.GetComponent<RecipeBookPage>();
        page.recipeImage.sprite = Resources.Load<Sprite>("Icebox_Strawberry");
        page.recipeName.text = "���� ���̽��ڽ�";
        page.recipeInfo.text = "�¾ƿ�. �ٹ�(baba)�� �����ϴ�\n �� ���̽�ũ�� ����ũ!";
        go.name = "���� ���̽��ڽ� ������";
        go.SetActive(true); 
    }

    public void OnClickNextPageButton()
    {
        Category category = currentCategory.GetComponent<Category>();
        category.NextPage();
    }
}
